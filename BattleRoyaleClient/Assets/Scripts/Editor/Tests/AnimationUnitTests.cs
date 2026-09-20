#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Player.Animation;

namespace BattleRoyale.Tests
{
    public class AnimationUnitTests
    {
        [Test]
        public void AnimationParameters_Hashes_AreNonZeroAndUnique()
        {
            Assert.AreNotEqual(0, AnimationParameters.Speed);
            Assert.AreNotEqual(0, AnimationParameters.MoveX);
            Assert.AreNotEqual(0, AnimationParameters.MoveY);
            Assert.AreNotEqual(0, AnimationParameters.IsGrounded);
            Assert.AreNotEqual(0, AnimationParameters.JumpTrigger);

            Assert.AreNotEqual(AnimationParameters.Speed, AnimationParameters.MoveX);
            Assert.AreNotEqual(AnimationParameters.IsGrounded, AnimationParameters.IsCrouching);
        }

        [Test]
        public void FootstepAudioConfig_ReturnsNull_WhenUnconfigured()
        {
            var config = ScriptableObject.CreateInstance<FootstepAudioConfig>();

            AudioClip clip = config.GetRandomFootstep("Grass");
            Assert.IsNull(clip, "Empty config should safely return null clip without exceptions");
        }
    }
}

#endif
