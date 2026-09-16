using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Multiplayer;

namespace BattleRoyale.Tests
{
    public class MultiplayerUnitTests
    {
        [Test]
        public void NetworkConfig_TickInterval_CalculatesCorrectly()
        {
            var config = ScriptableObject.CreateInstance<NetworkConfig>();

            Assert.AreEqual(7777, config.ServerPort);
            Assert.AreEqual(30, config.SendTickRate);
            Assert.AreEqual(1.0f / 30.0f, config.TickIntervalSeconds, 0.001f);
        }

        [Test]
        public void NetworkPlayerState_Packing_PreservesData()
        {
            Vector3 pos = new Vector3(10f, 2f, -15f);
            var state = new NetworkPlayerState(101, pos, 90f, 15f, 5.5f, true, false, 12.5f);

            Assert.AreEqual(101, state.NetworkId);
            Assert.AreEqual(pos, state.Position);
            Assert.AreEqual(90f, state.YawRotation);
            Assert.AreEqual(5.5f, state.HorizontalSpeed);
            Assert.IsTrue(state.IsGrounded);
        }
    }
}
