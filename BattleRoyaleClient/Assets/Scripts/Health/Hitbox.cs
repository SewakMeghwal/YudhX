using UnityEngine;

namespace BattleRoyale.HealthSystem
{
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private HitboxLocation location = HitboxLocation.Chest;
        [SerializeField] private float damageMultiplier = 1.0f;
        [SerializeField] private PlayerHealth ownerHealth;

        private void Awake()
        {
            if (ownerHealth == null)
            {
                ownerHealth = GetComponentInParent<PlayerHealth>();
            }

            // Set default location multipliers if not customized
            if (damageMultiplier == 1.0f)
            {
                switch (location)
                {
                    case HitboxLocation.Head: damageMultiplier = 2.5f; break;
                    case HitboxLocation.Chest: damageMultiplier = 1.0f; break;
                    case HitboxLocation.Stomach: damageMultiplier = 1.0f; break;
                    case HitboxLocation.Limb: damageMultiplier = 0.75f; break;
                }
            }
        }

        public void ProcessHit(DamageInfo rawDamageInfo)
        {
            if (ownerHealth == null) return;

            DamageInfo finalDamageInfo = rawDamageInfo;
            finalDamageInfo.HitLocation = location;
            finalDamageInfo.RawDamage *= damageMultiplier;

            ownerHealth.TakeDamage(finalDamageInfo);
        }
    }
}
