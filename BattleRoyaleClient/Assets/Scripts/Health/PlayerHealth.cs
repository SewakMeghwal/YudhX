using System;
using UnityEngine;

namespace BattleRoyale.HealthSystem
{
    public class PlayerHealth : MonoBehaviour, IDamageable, IHealable
    {
        [Header("Health & DBNO Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxDbnoHealth = 100f;
        [SerializeField] private bool enableDownedState = true;

        [Header("Armor Slots")]
        [SerializeField] private ArmorData equippedHelmet;
        [SerializeField] private float helmetDurability;
        [SerializeField] private ArmorData equippedVest;
        [SerializeField] private float vestDurability;

        private float currentHealth;
        private float currentDbnoHealth;
        private float currentBoost;
        private bool isDowned;
        private bool isDead;

        // Decoupled Events for UI / Network Sync
        public event Action<float, float> OnHealthChanged;           // current, max
        public event Action<float, float> OnDbnoHealthChanged;       // current, max
        public event Action<ArmorSlot, float, float> OnArmorChanged; // slot, currentDurability, maxDurability
        public event Action<DamageInfo> OnDamageTaken;
        public event Action OnPlayerDowned;
        public event Action OnPlayerDied;
        public event Action OnPlayerRevived;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float CurrentDbnoHealth => currentDbnoHealth;
        public bool IsDead => isDead;
        public bool IsDowned => isDowned;

        private void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            currentDbnoHealth = maxDbnoHealth;
            currentBoost = 0f;
            isDowned = false;
            isDead = false;

            if (equippedHelmet != null) helmetDurability = equippedHelmet.MaxDurability;
            if (equippedVest != null) vestDurability = equippedVest.MaxDurability;

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void EquipArmor(ArmorData armor)
        {
            if (armor == null) return;

            if (armor.Slot == ArmorSlot.Helmet)
            {
                equippedHelmet = armor;
                helmetDurability = armor.MaxDurability;
                OnArmorChanged?.Invoke(ArmorSlot.Helmet, helmetDurability, armor.MaxDurability);
            }
            else if (armor.Slot == ArmorSlot.BodyVest)
            {
                equippedVest = armor;
                vestDurability = armor.MaxDurability;
                OnArmorChanged?.Invoke(ArmorSlot.BodyVest, vestDurability, armor.MaxDurability);
            }
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (isDead) return;

            float incomingDamage = damageInfo.RawDamage;

            // Handle Downed DBNO State Damage
            if (isDowned)
            {
                currentDbnoHealth -= incomingDamage;
                OnDbnoHealthChanged?.Invoke(currentDbnoHealth, maxDbnoHealth);

                if (currentDbnoHealth <= 0f)
                {
                    Die();
                }
                return;
            }

            // Apply Armor Reduction
            incomingDamage = ProcessArmorAbsorption(damageInfo.HitLocation, incomingDamage);

            // Apply to main health
            currentHealth -= incomingDamage;
            currentHealth = Mathf.Max(0f, currentHealth);

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnDamageTaken?.Invoke(damageInfo);

            if (currentHealth <= 0f)
            {
                if (enableDownedState)
                {
                    EnterDownedState();
                }
                else
                {
                    Die();
                }
            }
        }

        private float ProcessArmorAbsorption(HitboxLocation hitLocation, float damage)
        {
            if (hitLocation == HitboxLocation.Head && equippedHelmet != null && helmetDurability > 0f)
            {
                float absorbed = damage * equippedHelmet.DamageReductionPercentage;
                helmetDurability -= absorbed;
                if (helmetDurability <= 0f)
                {
                    helmetDurability = 0f;
                    equippedHelmet = null; // Helmet shattered
                }
                OnArmorChanged?.Invoke(ArmorSlot.Helmet, helmetDurability, equippedHelmet != null ? equippedHelmet.MaxDurability : 1f);
                return damage - absorbed;
            }
            else if ((hitLocation == HitboxLocation.Chest || hitLocation == HitboxLocation.Stomach) && equippedVest != null && vestDurability > 0f)
            {
                float absorbed = damage * equippedVest.DamageReductionPercentage;
                vestDurability -= absorbed;
                if (vestDurability <= 0f)
                {
                    vestDurability = 0f;
                    equippedVest = null; // Vest shattered
                }
                OnArmorChanged?.Invoke(ArmorSlot.BodyVest, vestDurability, equippedVest != null ? equippedVest.MaxDurability : 1f);
                return damage - absorbed;
            }

            return damage;
        }

        private void EnterDownedState()
        {
            isDowned = true;
            currentDbnoHealth = maxDbnoHealth;
            OnPlayerDowned?.Invoke();
            OnDbnoHealthChanged?.Invoke(currentDbnoHealth, maxDbnoHealth);
        }

        public void Revive()
        {
            if (!isDowned || isDead) return;

            isDowned = false;
            currentHealth = 15f; // Revive with baseline health
            currentDbnoHealth = maxDbnoHealth;

            OnPlayerRevived?.Invoke();
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void Die()
        {
            isDead = true;
            isDowned = false;
            currentHealth = 0f;
            currentDbnoHealth = 0f;
            OnPlayerDied?.Invoke();
        }

        public bool Heal(float amount)
        {
            if (isDead || isDowned || currentHealth >= maxHealth) return false;

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            return true;
        }

        public bool Boost(float amount)
        {
            if (isDead || isDowned) return false;

            currentBoost = Mathf.Min(100f, currentBoost + amount);
            return true;
        }

        public bool RepairArmor(float amount)
        {
            bool repaired = false;
            if (equippedHelmet != null && helmetDurability < equippedHelmet.MaxDurability)
            {
                helmetDurability = Mathf.Min(equippedHelmet.MaxDurability, helmetDurability + amount);
                OnArmorChanged?.Invoke(ArmorSlot.Helmet, helmetDurability, equippedHelmet.MaxDurability);
                repaired = true;
            }
            if (equippedVest != null && vestDurability < equippedVest.MaxDurability)
            {
                vestDurability = Mathf.Min(equippedVest.MaxDurability, vestDurability + amount);
                OnArmorChanged?.Invoke(ArmorSlot.BodyVest, vestDurability, equippedVest.MaxDurability);
                repaired = true;
            }
            return repaired;
        }
    }
}
