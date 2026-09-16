using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Multiplayer.Combat;

namespace BattleRoyale.Tests
{
    public class MultiplayerCombatUnitTests
    {
        [Test]
        public void NetworkShotData_Packing_PreservesData()
        {
            Vector3 origin = new Vector3(5f, 1.5f, 10f);
            Vector3 direction = Vector3.forward;

            var shotData = new NetworkShotData(42, origin, direction, "ar_47", true, 100.5f);

            Assert.AreEqual(42, shotData.ShooterId);
            Assert.AreEqual(origin, shotData.Origin);
            Assert.AreEqual("ar_47", shotData.WeaponId);
            Assert.IsTrue(shotData.IsADS);
        }

        [Test]
        public void NetworkHitData_Packing_PreservesDamageData()
        {
            Vector3 hitPoint = new Vector3(0f, 1.8f, 25f);
            var hitData = new NetworkHitData(1, 2, 87.5f, true, hitPoint, false);

            Assert.AreEqual(1, hitData.ShooterId);
            Assert.AreEqual(2, hitData.VictimId);
            Assert.AreEqual(87.5f, hitData.AppliedDamage);
            Assert.IsTrue(hitData.IsHeadshot);
            Assert.IsFalse(hitData.IsFatal);
        }
    }
}
