using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Player;
using BattleRoyale.CameraSystem;
using BattleRoyale.HealthSystem;
using BattleRoyale.Weapons;
using BattleRoyale.InventorySystem;
using BattleRoyale.Loot;
using BattleRoyale.Zone;
using BattleRoyale.Match;
using BattleRoyale.Multiplayer;
using BattleRoyale.Backend;

namespace BattleRoyale.Tests
{
    public class MasterIntegrationTests
    {
        [Test]
        public void Milestone01_PlayerMovement_CalculatesState()
        {
            var settings = ScriptableObject.CreateInstance<MovementSettings>();
            Assert.Greater(settings.RunSpeed, settings.WalkSpeed);
            Assert.Greater(settings.SprintSpeed, settings.RunSpeed);
        }

        [Test]
        public void Milestone02_CameraSystem_CalculatesUnoccludedPosition()
        {
            var camObj = new GameObject("TestCam");
            var collision = camObj.AddComponent<CameraCollisionHandler>();
            var settings = ScriptableObject.CreateInstance<CameraSettings>();
            collision.Initialize(settings);

            Vector3 pivot = Vector3.zero;
            Vector3 desired = new Vector3(0, 1.5f, -3.0f);
            Vector3 result = collision.CalculateUnoccludedPosition(pivot, desired);

            Assert.AreEqual(desired, result);
            Object.DestroyImmediate(camObj);
        }

        [Test]
        public void Milestone04_PlayerHealth_ArmorMitigation_Works()
        {
            var playerObj = new GameObject("TestHealth");
            var health = playerObj.AddComponent<PlayerHealth>();

            var armor = ScriptableObject.CreateInstance<ArmorData>();
            health.EquipArmor(armor);

            health.TakeDamage(new DamageInfo(40f, DamageType.Bullet, HitboxLocation.Chest, Vector3.zero, Vector3.forward));
            Assert.Greater(health.CurrentHealth, 60f);

            Object.DestroyImmediate(playerObj);
        }

        [Test]
        public void Milestone05_WeaponSystem_TryFire_DepletesAmmo()
        {
            var weaponObj = new GameObject("TestWeapon");
            var weapon = weaponObj.AddComponent<Weapon>();
            var data = WeaponFactory.CreateAssaultRifleData();
            weapon.Initialize(data);

            int startAmmo = weapon.CurrentAmmo;
            weapon.TryFire(Vector3.zero, Vector3.forward, false, ~0, out _);

            Assert.AreEqual(startAmmo - 1, weapon.CurrentAmmo);
            Object.DestroyImmediate(weaponObj);
        }

        [Test]
        public void Milestone06_InventorySystem_EquipAndSwitch_Works()
        {
            var invObj = new GameObject("TestInv");
            var inv = invObj.AddComponent<PlayerWeaponInventory>();

            var arObj = new GameObject("AR");
            var ar = arObj.AddComponent<Weapon>();
            ar.Initialize(WeaponFactory.CreateAssaultRifleData());

            inv.EquipWeapon(ar, WeaponSlotIndex.Primary);
            Assert.AreEqual(ar, inv.ActiveWeapon);

            Object.DestroyImmediate(arObj);
            Object.DestroyImmediate(invObj);
        }

        [Test]
        public void Milestone09_SafeZone_ShrinkCenter_StaysWithinCurrentBounds()
        {
            var zoneObj = new GameObject("TestZone");
            var zone = zoneObj.AddComponent<SafeZoneController>();

            Vector3 currentCenter = Vector3.zero;
            Vector3 nextCenter = zone.CalculateNextCircleCenter(currentCenter, 250f, 150f);

            float offset = Vector3.Distance(currentCenter, nextCenter);
            Assert.LessOrEqual(offset, 100f);

            Object.DestroyImmediate(zoneObj);
        }

        [Test]
        public void Milestone14_MatchController_TracksEliminations()
        {
            var matchObj = new GameObject("TestMatch");
            var match = matchObj.AddComponent<MatchController>();

            match.RegisterPlayer(1, "Player_1");
            match.RegisterPlayer(2, "Player_2");

            Assert.AreEqual(2, match.AlivePlayerCount);
            match.ReportPlayerElimination(2, 1, "Sub-9", false);
            Assert.AreEqual(1, match.AlivePlayerCount);

            Object.DestroyImmediate(matchObj);
        }

        [Test]
        public void Milestone18_BackendConfig_Urls_AreValid()
        {
            var config = ScriptableObject.CreateInstance<BackendConfig>();
            Assert.IsTrue(config.RegisterUrl.Contains("/auth/register/"));
            Assert.IsTrue(config.ProfileUrl.Contains("/player/profile/"));
        }
    }
}
