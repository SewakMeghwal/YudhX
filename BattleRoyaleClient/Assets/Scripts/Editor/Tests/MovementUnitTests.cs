#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Player;

namespace BattleRoyale.Tests
{
    public class MovementUnitTests
    {
        [Test]
        public void MovementSettings_DefaultValues_AreValid()
        {
            var settings = ScriptableObject.CreateInstance<MovementSettings>();

            Assert.Greater(settings.WalkSpeed, 0f, "Walk speed should be positive");
            Assert.Greater(settings.RunSpeed, settings.WalkSpeed, "Run speed should be greater than walk speed");
            Assert.Greater(settings.SprintSpeed, settings.RunSpeed, "Sprint speed should be greater than run speed");
            Assert.Greater(settings.JumpHeight, 0f, "Jump height should be positive");
            Assert.Less(settings.Gravity, 0f, "Gravity should be negative");
        }

        [Test]
        public void PlayerRuntimeState_ToString_FormatsCorrectly()
        {
            var state = new PlayerRuntimeState
            {
                MovementState = MovementState.Running,
                StanceState = StanceState.Standing,
                IsGrounded = true,
                CurrentSpeed = 5.5f
            };

            string output = state.ToString();
            Assert.IsTrue(output.Contains("Running"));
            Assert.IsTrue(output.Contains("5.5"));
        }
    }
}

#endif
