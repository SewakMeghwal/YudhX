using System;

namespace BattleRoyale.HealthSystem
{
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damageInfo);
        bool IsDead { get; }
        bool IsDowned { get; }
    }

    public interface IHealable
    {
        bool Heal(float amount);
        bool Boost(float amount);
        bool RepairArmor(float amount);
    }
}
