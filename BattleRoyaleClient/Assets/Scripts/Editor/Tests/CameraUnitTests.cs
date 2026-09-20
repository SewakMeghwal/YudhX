#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.CameraSystem;

namespace BattleRoyale.Tests
{
    public class CameraUnitTests
    {
        [Test]
        public void CameraSettings_DefaultValues_AreValid()
        {
            var settings = ScriptableObject.CreateInstance<CameraSettings>();

            Assert.Greater(settings.DefaultFOV, settings.AimFOV, "Default FOV should be wider than Aim FOV");
            Assert.Greater(settings.SensitivityX, 0f, "Sensitivity X should be positive");
            Assert.Greater(settings.SensitivityY, 0f, "Sensitivity Y should be positive");
            Assert.Less(settings.MinPitch, settings.MaxPitch, "Min pitch should be less than max pitch");
            Assert.Greater(settings.CollisionRadius, 0f, "Collision radius should be positive");
        }

        [Test]
        public void CameraCollisionHandler_UnoccludedPath_ReturnsDesiredPosition()
        {
            var gameObject = new GameObject("TestCameraCollision");
            var settings = ScriptableObject.CreateInstance<CameraSettings>();
            var handler = gameObject.AddComponent<CameraCollisionHandler>();
            handler.Initialize(settings);

            Vector3 pivot = Vector3.zero;
            Vector3 desired = new Vector3(0, 1.5f, -3f);

            Vector3 result = handler.CalculateUnoccludedPosition(pivot, desired);
            Assert.AreEqual(desired, result, "In open space, unoccluded position should equal desired position");

            Object.DestroyImmediate(gameObject);
        }
    }
}

#endif
