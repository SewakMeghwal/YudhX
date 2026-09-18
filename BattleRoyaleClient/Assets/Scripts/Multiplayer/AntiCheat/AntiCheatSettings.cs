using UnityEngine;

namespace BattleRoyale.Multiplayer.AntiCheat
{
    [CreateAssetMenu(fileName = "AntiCheatSettings", menuName = "BattleRoyale/Multiplayer/AntiCheat Settings")]
    public class AntiCheatSettings : ScriptableObject
    {
        [Header("Movement Validation")]
        [SerializeField] private float maxAllowedSpeedMetersPerSec = 12.0f;
        [SerializeField] private float maxSingleFrameTeleportMeters = 5.0f;

        [Header("Combat Validation")]
        [SerializeField] private float fireRateToleranceFactor = 0.85f; // Allow 15% latency jitter
        [SerializeField] private bool enforceLineOfSightCheck = true;

        [Header("Punishment Thresholds")]
        [SerializeField] private int maxAllowedViolationsBeforeKick = 3;

        public float MaxAllowedSpeedMetersPerSec => maxAllowedSpeedMetersPerSec;
        public float MaxSingleFrameTeleportMeters => maxSingleFrameTeleportMeters;
        public float FireRateToleranceFactor => fireRateToleranceFactor;
        public bool EnforceLineOfSightCheck => enforceLineOfSightCheck;
        public int MaxAllowedViolationsBeforeKick => maxAllowedViolationsBeforeKick;
    }
}
