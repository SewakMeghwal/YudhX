using NUnit.Framework;
using UnityEngine;
using BattleRoyale.AI;

namespace BattleRoyale.Tests
{
    public class AiUnitTests
    {
        [Test]
        public void BotConfig_DefaultValues_AreValid()
        {
            var config = ScriptableObject.CreateInstance<BotConfig>();

            Assert.Greater(config.DetectionRadius, 10.0f);
            Assert.Greater(config.ViewAngleDegrees, 30.0f);
            Assert.Greater(config.WanderRadius, 10.0f);
            Assert.Greater(config.ReactionDelaySeconds, 0.0f);
        }

        [Test]
        public void BotController_InitialState_IsPatrol()
        {
            var gameObject = new GameObject("TestBot");
            var bot = gameObject.AddComponent<BotController>();

            Assert.AreEqual(BotStateType.Patrol, bot.CurrentState);

            Object.DestroyImmediate(gameObject);
        }
    }
}
