using System;
using System.Collections.Generic;
using UnityEngine;
using BattleRoyale.InventorySystem;
using BattleRoyale.Loot;

namespace BattleRoyale.Multiplayer.Inventory
{
    public class NetworkLootManager : MonoBehaviour
    {
        [Header("Validation Settings")]
        [SerializeField] private float maxPickupDistance = 3.5f;

        private readonly HashSet<ulong> claimedLootIds = new HashSet<ulong>();
        private static NetworkLootManager instance;

        public static NetworkLootManager Instance => instance;
        public float MaxPickupDistance => maxPickupDistance;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        public void ServerValidatePickupRpc(NetworkLootPickupRequest request, LootPickup targetPickup, PlayerBackpackInventory playerBackpack, GameObject playerObj)
        {
            if (targetPickup == null || playerObj == null) return;

            ulong lootId = (ulong)targetPickup.GetInstanceID();

            // 1. Duplicate Pickup Lock Check (Anti-Cheat check: item duplication prevention)
            if (claimedLootIds.Contains(lootId))
            {
                Debug.LogWarning($"[Anti-Cheat] Player {request.PlayerNetworkId} attempted to double-pickup claimed loot {lootId}");
                return;
            }

            // 2. Proximity Validation Check (Anti-Cheat check: tele-pickup prevention)
            float distance = Vector3.Distance(request.InteractionPosition, targetPickup.transform.position);
            if (distance > maxPickupDistance)
            {
                Debug.LogWarning($"[Anti-Cheat] Player {request.PlayerNetworkId} attempted pickup out of range: {distance:F1}m");
                return;
            }

            // Lock loot item on server
            claimedLootIds.Add(lootId);

            // Execute item transfer
            targetPickup.Interact(playerObj);
        }

        public void ServerValidateDropRpc(NetworkItemDropRequest dropRequest, PlayerBackpackInventory playerBackpack, GameObject groundLootPrefab)
        {
            if (playerBackpack == null) return;

            InventoryItemData item = null;
            foreach (var slot in playerBackpack.Items)
            {
                if (slot.Item != null && slot.Item.ItemId.Equals(dropRequest.ItemId, StringComparison.OrdinalIgnoreCase))
                {
                    item = slot.Item;
                    break;
                }
            }

            if (item != null && playerBackpack.RemoveItem(item, dropRequest.Quantity))
            {
                // Spawn new ground loot pickup
                GameObject droppedObj = groundLootPrefab != null
                    ? Instantiate(groundLootPrefab, dropRequest.DropPosition, Quaternion.identity)
                    : new GameObject($"Dropped_{item.ItemName}");

                droppedObj.transform.position = dropRequest.DropPosition;

                LootPickup pickup = droppedObj.GetComponent<LootPickup>();
                if (pickup == null) pickup = droppedObj.AddComponent<LootPickup>();

                pickup.Initialize(item, dropRequest.Quantity, LootRarity.Common);
            }
        }
    }
}
