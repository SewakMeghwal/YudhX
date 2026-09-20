#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.HealthSystem;

namespace BattleRoyale.Tests
{
    public class HealthUnitTests
    {
        [Test]
        public void PlayerHealth_InitialHealth_Is100()
        {
            var gameObject = new GameObject("TestPlayer");
            var health = gameObject.AddComponent<PlayerHealth>();

            Assert.AreEqual(100f, health.CurrentHealth);
            Assert.IsFalse(health.IsDead);
            Assert.IsFalse(health.IsDowned);

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void PlayerHealth_TakeDamage_ReducesHealth()
        {
            var gameObject = new GameObject("TestPlayer");
            var health = gameObject.AddComponent<PlayerHealth>();

            var damageInfo = new DamageInfo(30f, DamageType.Bullet, HitboxLocation.Chest, Vector3.zero, Vector3.forward);
            health.TakeDamage(damageInfo);

            Assert.AreEqual(70f, health.CurrentHealth);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void PlayerHealth_LethalDamage_TriggersDownedState()
        {
            var gameObject = new GameObject("TestPlayer");
            var health = gameObject.AddComponent<PlayerHealth>();

            var damageInfo = new DamageInfo(120f, DamageType.Bullet, HitboxLocation.Chest, Vector3.zero, Vector3.forward);
            health.TakeDamage(damageInfo);

            Assert.IsTrue(health.IsDowned, "Lethal damage with DBNO enabled should trigger Downed state");
            Assert.IsFalse(health.IsDead, "Player should be downed before being dead");

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void PlayerHealth_ArmorAbsorption_ReducesDamage()
        {
            var gameObject = new GameObject("TestPlayer");
            var health = gameObject.AddComponent<PlayerHealth>();

            var vest = ScriptableObject.CreateInstance<ArmorData>();
            // Use reflection or standard accessor to test vest
            health.EquipArmor(vest);

            float initialHealth = health.CurrentHealth;
            var damageInfo = new DamageInfo(50f, DamageType.Bullet, HitboxLocation.Chest, Vector3.zero, Vector3.forward);
            health.TakeDamage(damageInfo);

            Assert.Greater(health.CurrentHealth, initialHealth - 50f, "Armor should absorb part of incoming chest damage");
            Object.DestroyImmediate(gameObject);
        }
    }
}

#endif
