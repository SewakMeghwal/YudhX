using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Multiplayer.AntiCheat;

namespace BattleRoyale.Tests
{
    public class AntiCheatUnitTests
    {
        [Test]
        public void AntiCheatSettings_DefaultValues_AreValid()
        {
            var settings = ScriptableObject.CreateInstance<AntiCheatSettings>();

            Assert.AreEqual(12.0f, settings.MaxAllowedSpeedMetersPerSec);
            Assert.AreEqual(5.0f, settings.MaxSingleFrameTeleportMeters);
            Assert.AreEqual(3, settings.MaxAllowedViolationsBeforeKick);
        }

        [Test]
        public void ServerAntiCheatManager_SpeedValidation_DetectsSpeedHack()
        {
            var managerObj = new GameObject("TestAntiCheat");
            var manager = managerObj.AddComponent<ServerAntiCheatManager>();

            Vector3 pos1 = Vector3.zero;
            Vector3 pos2 = new Vector3(50f, 0f, 0f); // 50 meters in 1 second = 50 m/s (Exceeds 12 m/s)

            bool isValid = manager.ValidateMovement(100, pos2, pos1, 1.0f);

            Assert.IsFalse(isValid, "50 m/s movement should fail validation check");

            Object.DestroyImmediate(managerObj);
        }
    }
}
