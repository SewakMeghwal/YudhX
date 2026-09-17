using System;
using UnityEngine;

namespace BattleRoyale.Multiplayer.Inventory
{
    [Serializable]
    public struct NetworkLootPickupRequest
    {
        public ulong PlayerNetworkId;
        public ulong LootNetworkId;
        public Vector3 InteractionPosition;
        public float Timestamp;

        public NetworkLootPickupRequest(ulong playerId, ulong lootId, Vector3 pos, float time)
        {
            PlayerNetworkId = playerId;
            LootNetworkId = lootId;
            InteractionPosition = pos;
            Timestamp = time;
        }
    }

    [Serializable]
    public struct NetworkItemDropRequest
    {
        public ulong PlayerNetworkId;
        public string ItemId;
        public int Quantity;
        public Vector3 DropPosition;

        public NetworkItemDropRequest(ulong playerId, string itemId, int qty, Vector3 dropPos)
        {
            PlayerNetworkId = playerId;
            ItemId = itemId;
            Quantity = qty;
            DropPosition = dropPos;
        }
    }
}
