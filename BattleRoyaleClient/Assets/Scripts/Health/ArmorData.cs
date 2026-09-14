using UnityEngine;

namespace BattleRoyale.HealthSystem
{
    public enum ArmorSlot
    {
        Helmet,
        BodyVest
    }

    [CreateAssetMenu(fileName = "ArmorData", menuName = "BattleRoyale/Health/Armor Data")]
    public class ArmorData : ScriptableObject
    {
        [SerializeField] private string armorName = "Level 1 Vest";
        [SerializeField] private ArmorSlot slot = ArmorSlot.BodyVest;
        [SerializeField] private int armorLevel = 1;
        [SerializeField] private float maxDurability = 200f;

        [Tooltip("Percentage of damage absorbed by armor (0.3 = 30% reduction)")]
        [Range(0f, 1f)]
        [SerializeField] private float damageReductionPercentage = 0.3f;

        public string ArmorName => armorName;
        public ArmorSlot Slot => slot;
        public int ArmorLevel => armorLevel;
        public float MaxDurability => maxDurability;
        public float DamageReductionPercentage => damageReductionPercentage;
    }
}
