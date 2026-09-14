using UnityEngine;

namespace BattleRoyale.HealthSystem
{
    public enum ConsumableType
    {
        Bandage,
        FirstAidKit,
        Medkit,
        EnergyDrink,
        Painkiller
    }

    [CreateAssetMenu(fileName = "ConsumableData", menuName = "BattleRoyale/Health/Consumable Data")]
    public class ConsumableData : ScriptableObject
    {
        [SerializeField] private string itemName = "Medkit";
        [SerializeField] private ConsumableType consumableType = ConsumableType.Medkit;
        [SerializeField] private float useDurationSeconds = 6.0f;
        [SerializeField] private float healthAmount = 100.0f;

        [Tooltip("Max health percentage threshold this item can heal up to (0.75 = 75% for FirstAid)")]
        [Range(0f, 1f)]
        [SerializeField] private float healCapPercentage = 1.0f;

        [Tooltip("Boost energy amount granted (for Energy Drinks / Painkillers)")]
        [SerializeField] private float boostAmount = 0.0f;

        public string ItemName => itemName;
        public ConsumableType ConsumableType => consumableType;
        public float UseDurationSeconds => useDurationSeconds;
        public float HealthAmount => healthAmount;
        public float HealCapPercentage => healCapPercentage;
        public float BoostAmount => boostAmount;
    }
}
