using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Match.Drop;

namespace BattleRoyale.Tests
{
    public class ParachuteUnitTests
    {
        [Test]
        public void ParachuteConfig_DefaultValues_AreValid()
        {
            var config = ScriptableObject.CreateInstance<ParachuteConfig>();

            Assert.AreEqual(250f, config.FlightAltitudeMeters);
            Assert.AreEqual(60f, config.AutoDeployAltitudeMeters);
            Assert.Greater(config.MaxFreefallDiveSpeed, config.MinFreefallFallSpeed);
            Assert.Greater(config.ParachuteGlideSpeed, config.ParachuteDescentRate);
        }

        [Test]
        public void PlayerParachuteController_InitialState_IsLanded()
        {
            var gameObject = new GameObject("TestParachutePlayer");
            var controller = gameObject.AddComponent<PlayerParachuteController>();

            Assert.AreEqual(DropState.Landed, controller.CurrentDropState);

            Object.DestroyImmediate(gameObject);
        }
    }
}
