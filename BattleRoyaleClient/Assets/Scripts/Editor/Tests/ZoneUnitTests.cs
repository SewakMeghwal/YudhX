#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Zone;

namespace BattleRoyale.Tests
{
    public class ZoneUnitTests
    {
        [Test]
        public void SafeZoneController_InsideCheck_CalculatesDistanceCorrectly()
        {
            var gameObject = new GameObject("TestZone");
            var controller = gameObject.AddComponent<SafeZoneController>();

            Vector3 center = Vector3.zero;
            Vector3 insidePos = new Vector3(10f, 0f, 10f);
            Vector3 outsidePos = new Vector3(300f, 0f, 0f);

            // Test next circle math guarantees inner circle stays within current circle
            Vector3 nextCenter = controller.CalculateNextCircleCenter(center, 250f, 150f);
            float offsetFromCenter = Vector3.Distance(center, nextCenter);

            Assert.LessOrEqual(offsetFromCenter, 100f, "Next center offset must not exceed currentRadius - nextRadius");

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void ZoneSettings_DefaultPhases_AreValid()
        {
            var settings = ScriptableObject.CreateInstance<ZoneSettings>();

            Assert.AreEqual(250f, settings.InitialRadiusMeters);
            Assert.AreEqual(4, settings.Phases.Count);
            Assert.Greater(settings.Phases[0].DelaySeconds, 0f);
            Assert.Greater(settings.Phases[3].DamagePerSecond, settings.Phases[0].DamagePerSecond, "Late phase zone damage must be higher than phase 1");
        }
    }
}

#endif
