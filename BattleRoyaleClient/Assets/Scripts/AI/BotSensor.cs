using UnityEngine;
using BattleRoyale.HealthSystem;

namespace BattleRoyale.AI
{
    public class BotSensor : MonoBehaviour
    {
        [SerializeField] private BotConfig config;
        [SerializeField] private Transform eyesTransform;

        public Transform TargetEnemy { get; private set; }

        public void Initialize(BotConfig botConfig)
        {
            config = botConfig;
            if (eyesTransform == null) eyesTransform = transform;
        }

        public Transform ScanForEnemies()
        {
            if (config == null) return null;

            Collider[] hits = Physics.OverlapSphere(transform.position, config.DetectionRadius, config.EnemyLayers);
            Transform closestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var col in hits)
            {
                if (col.gameObject == gameObject) continue; // Skip self

                PlayerHealth health = col.GetComponentInParent<PlayerHealth>();
                if (health != null && !health.IsDead && !health.IsDowned)
                {
                    Vector3 directionToTarget = (col.transform.position - eyesTransform.position).normalized;
                    float angle = Vector3.Angle(eyesTransform.forward, directionToTarget);

                    if (angle <= config.ViewAngleDegrees * 0.5f)
                    {
                        // Check Line of Sight
                        if (Physics.Raycast(eyesTransform.position, directionToTarget, out RaycastHit rayHit, config.DetectionRadius))
                        {
                            if (rayHit.transform == col.transform || rayHit.transform.IsChildOf(col.transform))
                            {
                                float dist = Vector3.Distance(transform.position, col.transform.position);
                                if (dist < closestDistance)
                                {
                                    closestDistance = dist;
                                    closestTarget = col.transform;
                                }
                            }
                        }
                    }
                }
            }

            TargetEnemy = closestTarget;
            return TargetEnemy;
        }

        private void OnDrawGizmosSelected()
        {
            if (config == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, config.DetectionRadius);
        }
    }
}
