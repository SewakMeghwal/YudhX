#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Backend;
using BattleRoyale.Backend.Stats;

namespace BattleRoyale.Tests
{
    public class StatsUnitTests
    {
        [Test]
        public void MatchSubmitRequest_JsonSerialization_FormatsCorrectly()
        {
            var submitReq = new MatchSubmitRequest("match_999", "map_alpha", 8, 450.5f, 1, 6, 1250f, 750);
            string json = JsonUtility.ToJson(submitReq);

            Assert.IsTrue(json.Contains("match_999"));
            Assert.IsTrue(json.Contains("map_alpha"));
            Assert.IsTrue(json.Contains("750"));
        }

        [Test]
        public void LeaderboardListWrapper_JsonParsing_ParsesArray()
        {
            string json = "{\"results\":[{\"id\":1,\"display_name\":\"TopGun\",\"total_wins\":12,\"total_kills\":145}]}";
            var wrapper = JsonUtility.FromJson<LeaderboardListWrapper>(json);

            Assert.IsNotNull(wrapper.results);
            Assert.AreEqual(1, wrapper.results.Length);
            Assert.AreEqual("TopGun", wrapper.results[0].display_name);
        }
    }
}

#endif
