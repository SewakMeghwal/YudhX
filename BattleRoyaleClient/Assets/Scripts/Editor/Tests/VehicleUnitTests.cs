#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Vehicles;

namespace BattleRoyale.Tests
{
    public class VehicleUnitTests
    {
        [Test]
        public void VehicleData_DefaultValues_AreValid()
        {
            var data = ScriptableObject.CreateInstance<VehicleData>();

            Assert.AreEqual(500f, data.MaxHealth);
            Assert.AreEqual(100f, data.MaxFuel);
            Assert.Greater(data.MotorTorque, 0f);
            Assert.Greater(data.MaxSpeedKmH, 0f);
        }

        [Test]
        public void VehicleSeat_AssignAndClearOccupant_WorksCorrectly()
        {
            var seatObj = new GameObject("TestSeat");
            var seat = seatObj.AddComponent<VehicleSeat>();

            var playerObj = new GameObject("Player");
            seat.AssignOccupant(playerObj);

            Assert.IsTrue(seat.IsOccupied);
            Assert.AreEqual(playerObj, seat.OccupantPlayer);

            seat.ClearOccupant();
            Assert.IsFalse(seat.IsOccupied);

            Object.DestroyImmediate(playerObj);
            Object.DestroyImmediate(seatObj);
        }
    }
}

#endif
