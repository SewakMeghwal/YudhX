using UnityEngine;

namespace BattleRoyale.Loot
{
    public enum LootRarity
    {
        Common,      // Grey
        Uncommon,    // Green
        Rare,        // Blue
        Epic,        // Purple
        Legendary    // Gold
    }

    public static class LootRarityColor
    {
        public static Color GetColor(LootRarity rarity)
        {
            switch (rarity)
            {
                case LootRarity.Common: return new Color(0.75f, 0.75f, 0.75f, 1.0f);
                case LootRarity.Uncommon: return new Color(0.2f, 0.8f, 0.2f, 1.0f);
                case LootRarity.Rare: return new Color(0.2f, 0.5f, 1.0f, 1.0f);
                case LootRarity.Epic: return new Color(0.6f, 0.2f, 0.8f, 1.0f);
                case LootRarity.Legendary: return new Color(1.0f, 0.75f, 0.0f, 1.0f);
                default: return Color.white;
            }
        }
    }
}
