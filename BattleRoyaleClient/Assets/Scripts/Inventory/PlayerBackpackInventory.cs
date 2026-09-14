using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.InventorySystem
{
    public class PlayerBackpackInventory : MonoBehaviour, IInventory
    {
        [Header("Backpack Capacity Settings")]
        [SerializeField] private float baseWeightCapacity = 50.0f;
        [SerializeField] private int maxSlots = 20;

        private readonly List<InventorySlot> items = new List<InventorySlot>();
        private float currentWeight;

        public event Action OnInventoryChanged;
        public float CurrentWeight => currentWeight;
        public float MaxWeight => baseWeightCapacity;
        public IReadOnlyList<InventorySlot> Items => items;

        public bool HasCapacity(float additionalWeight)
        {
            return (currentWeight + additionalWeight) <= baseWeightCapacity;
        }

        public bool AddItem(InventoryItemData item, int quantity, out int remainingUnadded)
        {
            remainingUnadded = quantity;
            if (item == null || quantity <= 0) return false;

            float itemWeight = item.ItemWeight * quantity;
            if (!HasCapacity(itemWeight))
            {
                // Calculate max possible quantity within capacity
                float availableWeight = Mathf.Max(0f, baseWeightCapacity - currentWeight);
                int maxPossibleQty = Mathf.FloorToInt(availableWeight / item.ItemWeight);
                if (maxPossibleQty <= 0) return false;
                quantity = maxPossibleQty;
            }

            // 1. Try stacking into existing non-full slots
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item == item && items[i].Quantity < item.MaxStackSize)
                {
                    int spaceLeft = item.MaxStackSize - items[i].Quantity;
                    int addQty = Mathf.Min(spaceLeft, quantity);

                    InventorySlot slot = items[i];
                    slot.Quantity += addQty;
                    items[i] = slot;

                    quantity -= addQty;
                    currentWeight += item.ItemWeight * addQty;

                    if (quantity <= 0)
                    {
                        remainingUnadded = 0;
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }

            // 2. Add into new slot if available
            while (quantity > 0 && items.Count < maxSlots)
            {
                int addQty = Mathf.Min(item.MaxStackSize, quantity);
                items.Add(new InventorySlot(item, addQty));

                quantity -= addQty;
                currentWeight += item.ItemWeight * addQty;
            }

            remainingUnadded = quantity;
            OnInventoryChanged?.Invoke();
            return remainingUnadded < quantity;
        }

        public bool RemoveItem(InventoryItemData item, int quantity)
        {
            if (item == null || quantity <= 0) return false;
            if (GetItemCount(item) < quantity) return false;

            int neededToRemove = quantity;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].Item == item)
                {
                    if (items[i].Quantity <= neededToRemove)
                    {
                        neededToRemove -= items[i].Quantity;
                        currentWeight -= item.ItemWeight * items[i].Quantity;
                        items.RemoveAt(i);
                    }
                    else
                    {
                        InventorySlot slot = items[i];
                        slot.Quantity -= neededToRemove;
                        items[i] = slot;

                        currentWeight -= item.ItemWeight * neededToRemove;
                        neededToRemove = 0;
                    }

                    if (neededToRemove <= 0) break;
                }
            }

            currentWeight = Mathf.Max(0f, currentWeight);
            OnInventoryChanged?.Invoke();
            return true;
        }

        public int GetItemCount(InventoryItemData item)
        {
            if (item == null) return 0;

            int total = 0;
            foreach (var slot in items)
            {
                if (slot.Item == item)
                {
                    total += slot.Quantity;
                }
            }
            return total;
        }
    }
}
