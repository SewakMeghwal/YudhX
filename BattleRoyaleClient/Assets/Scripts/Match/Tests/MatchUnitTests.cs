using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Match;

namespace BattleRoyale.Tests
{
    public class MatchUnitTests
    {
        [Test]
        public void MatchConfig_DefaultValues_AreValid()
        {
            var config = ScriptableObject.CreateInstance<MatchConfig>();

            Assert.AreEqual(2, config.MinRequiredPlayers);
            Assert.AreEqual(8, config.MaxAllowedPlayers);
            Assert.AreEqual(10f, config.LobbyCountdownSeconds);
            Assert.AreEqual(100, config.XpPerKill);
            Assert.AreEqual(500, config.XpPerWin);
        }

        [Test]
        public void MatchController_PlayerElimination_UpdatesRankAndAliveCount()
        {
            var gameObject = new GameObject("TestMatchController");
            var match = gameObject.AddComponent<MatchController>();

            match.RegisterPlayer(1, "Player_Alpha");
            match.RegisterPlayer(2, "Player_Beta");
            match.RegisterPlayer(3, "Player_Gamma");

            Assert.AreEqual(3, match.AlivePlayerCount);

            match.ReportPlayerElimination(3, 1, "AR-47", true);

            Assert.AreEqual(2, match.AlivePlayerCount);

            Object.DestroyImmediate(gameObject);
        }
    }
}
