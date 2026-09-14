using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Weapons;

namespace BattleRoyale.Tests
{
    public class WeaponUnitTests
    {
        [Test]
        public void WeaponFactory_CreatesAllFourInitialWeapons()
        {
            var ar = WeaponFactory.CreateAssaultRifleData();
            var smg = WeaponFactory.CreateSmgData();
            var shotgun = WeaponFactory.CreateShotgunData();
            var pistol = WeaponFactory.CreatePistolData();

            Assert.AreEqual(WeaponType.AssaultRifle, ar.WeaponType);
            Assert.AreEqual(WeaponType.SubmachineGun, smg.WeaponType);
            Assert.AreEqual(WeaponType.Shotgun, shotgun.WeaponType);
            Assert.AreEqual(WeaponType.Pistol, pistol.WeaponType);

            Assert.AreEqual(30, ar.MagazineCapacity);
            Assert.AreEqual(35, smg.MagazineCapacity);
            Assert.AreEqual(8, shotgun.MagazineCapacity);
            Assert.AreEqual(15, pistol.MagazineCapacity);

            Assert.AreEqual(8, shotgun.PelletsPerShot, "Shotgun should fire 8 pellets per shell");
        }

        [Test]
        public void Weapon_TryFire_DepletesAmmo()
        {
            var gameObject = new GameObject("TestWeapon");
            var weapon = gameObject.AddComponent<Weapon>();
            var data = WeaponFactory.CreateAssaultRifleData();
            weapon.Initialize(data);

            int initialAmmo = weapon.CurrentAmmo;
            bool fired = weapon.TryFire(Vector3.zero, Vector3.forward, false, ~0, out _);

            Assert.IsTrue(fired);
            Assert.AreEqual(initialAmmo - 1, weapon.CurrentAmmo);

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void WeaponRecoil_UpdateRecoil_RecoversToZero()
        {
            var recoil = new WeaponRecoil();
            recoil.ApplyRecoil(2.0f, 0.5f);

            Assert.AreNotEqual(Vector3.zero, recoil.CurrentRecoil);

            for (int i = 0; i < 30; i++)
            {
                recoil.UpdateRecoil(0.1f);
            }

            Assert.Less(recoil.CurrentRecoil.magnitude, 0.1f, "Recoil should recover smoothly towards zero");
        }
    }
}
