using NUnit.Framework;
using UnityEngine;
using BattleRoyale.UI.Mobile;

namespace BattleRoyale.Tests
{
    public class MobileUnitTests
    {
        [Test]
        public void TouchButton_PressState_TogglesOnPointer()
        {
            var btnObj = new GameObject("TestTouchButton");
            var button = btnObj.AddComponent<TouchButton>();

            Assert.IsFalse(button.IsPressed);

            Object.DestroyImmediate(btnObj);
        }

        [Test]
        public void VirtualJoystick_InitialInput_IsZero()
        {
            var joystickObj = new GameObject("TestJoystick");
            var joystick = joystickObj.AddComponent<VirtualJoystick>();

            Assert.AreEqual(Vector2.zero, joystick.InputVector);

            Object.DestroyImmediate(joystickObj);
        }
    }
}
