using UnityEngine;

namespace BattleRoyale.AI
{
    [CreateAssetMenu(fileName = "BotConfig", menuName = "BattleRoyale/AI/Bot Config")]
    public class BotConfig : ScriptableObject
    {
        [Header("Perception & Detection")]
        [SerializeField] private float detectionRadius = 35.0f;
        [SerializeField] private float viewAngleDegrees = 120.0f;
        [SerializeField] private float hearFootstepRadius = 18.0f;
        [SerializeField] private float reactionDelaySeconds = 0.3f;

        [Header("Combat & Aiming")]
        [SerializeField] private float preferredFiringDistance = 15.0f;
        [SerializeField] private float aimInaccuracySpread = 1.5f;
        [SerializeField] private float burstFireDuration = 0.8f;

        [Header("Loot & Survival")]
        [SerializeField] private float healHealthThreshold = 40.0f;
        [SerializeField] private float wanderRadius = 40.0f;
        [SerializeField] private LayerMask enemyLayers = ~0;

        public float DetectionRadius => detectionRadius;
        public float ViewAngleDegrees => viewAngleDegrees;
        public float HearFootstepRadius => hearFootstepRadius;
        public float ReactionDelaySeconds => reactionDelaySeconds;
        public float PreferredFiringDistance => preferredFiringDistance;
        public float AimInaccuracySpread => aimInaccuracySpread;
        public float BurstFireDuration => burstFireDuration;
        public float HealHealthThreshold => healHealthThreshold;
        public float WanderRadius => wanderRadius;
        public LayerMask EnemyLayers => enemyLayers;
    }
}
