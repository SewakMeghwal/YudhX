using UnityEngine;
using BattleRoyale.InventorySystem;
using BattleRoyale.Weapons;

namespace BattleRoyale.Loot
{
    public class LootPickup : MonoBehaviour, IInteractable
    {
        [Header("Loot Configuration")]
        [SerializeField] private InventoryItemData itemData;
        [SerializeField] private LootRarity rarity = LootRarity.Common;
        [SerializeField] private int quantity = 1;
        [SerializeField] private WeaponData weaponData; // Optional if item is a weapon

        [Header("Visuals")]
        [SerializeField] private MeshRenderer meshRenderer;

        public InventoryItemData ItemData => itemData;
        public LootRarity Rarity => rarity;
        public int Quantity => quantity;

        public string InteractionPrompt
        {
            get
            {
                if (itemData == null) return "Pick Up Item";
                string qtyText = quantity > 1 ? $" ({quantity})" : "";
                return $"Press F to Pick Up {itemData.ItemName}{qtyText}";
            }
        }

        public void Initialize(InventoryItemData item, int qty, LootRarity itemRarity, WeaponData wData = null)
        {
            itemData = item;
            quantity = qty;
            rarity = itemRarity;
            weaponData = wData;
        }

        public bool CanInteract(GameObject interactor)
        {
            return itemData != null && quantity > 0;
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;

            // Check if player has Backpack Inventory
            PlayerBackpackInventory backpack = interactor.GetComponent<PlayerBackpackInventory>();
            PlayerWeaponInventory weaponInv = interactor.GetComponent<PlayerWeaponInventory>();

            // Case A: Weapon Pickup
            if (weaponData != null && weaponInv != null)
            {
                var weaponObj = new GameObject(weaponData.WeaponName);
                var weaponComp = weaponObj.AddComponent<Weapon>();
                weaponComp.Initialize(weaponData);

                if (weaponInv.EquipWeapon(weaponComp, WeaponSlotIndex.Primary))
                {
                    Destroy(gameObject);
                    return;
                }
            }

            // Case B: Backpack Item Pickup
            if (backpack != null)
            {
                bool added = backpack.AddItem(itemData, quantity, out int remaining);
                if (added)
                {
                    if (remaining <= 0)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        quantity = remaining; // Partial pickup overflow
                    }
                }
            }
        }

        public void OnFocusEnter()
        {
            // Focus visual highlight (e.g. outline shader / color tweak)
            if (meshRenderer != null)
            {
                meshRenderer.material.color *= 1.2f;
            }
        }

        public void OnFocusExit()
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.color /= 1.2f;
            }
        }
    }
}
