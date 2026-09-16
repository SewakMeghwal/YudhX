using System;
using System.Collections.Generic;
using UnityEngine;
using BattleRoyale.InventorySystem;

namespace BattleRoyale.Loot
{
    [Serializable]
    public struct LootTableEntry
    {
        public InventoryItemData ItemData;
        public LootRarity Rarity;
        public int Weight; // Relative spawn weight
        public int MinQuantity;
        public int MaxQuantity;
    }

    [CreateAssetMenu(fileName = "LootTableData", menuName = "BattleRoyale/Loot/Loot Table Data")]
    public class LootTableData : ScriptableObject
    {
        [SerializeField] private List<LootTableEntry> entries = new List<LootTableEntry>();

        public IReadOnlyList<LootTableEntry> Entries => entries;

        public bool GetRandomLoot(out InventoryItemData item, out int quantity, out LootRarity rarity)
        {
            item = null;
            quantity = 0;
            rarity = LootRarity.Common;

            if (entries == null || entries.Count == 0) return false;

            int totalWeight = 0;
            foreach (var entry in entries)
            {
                totalWeight += Mathf.Max(1, entry.Weight);
            }

            int roll = UnityEngine.Random.Range(0, totalWeight);
            int accumulated = 0;

            foreach (var entry in entries)
            {
                accumulated += Mathf.Max(1, entry.Weight);
                if (roll < accumulated)
                {
                    item = entry.ItemData;
                    rarity = entry.Rarity;
                    quantity = UnityEngine.Random.Range(entry.MinQuantity, entry.MaxQuantity + 1);
                    return item != null;
                }
            }

            return false;
        }
    }
}
