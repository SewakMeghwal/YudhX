using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Backend;

namespace BattleRoyale.Tests
{
    public class BackendUnitTests
    {
        [Test]
        public void BackendConfig_Urls_FormatCorrectly()
        {
            var config = ScriptableObject.CreateInstance<BackendConfig>();

            Assert.IsTrue(config.RegisterUrl.EndsWith("/auth/register/"));
            Assert.IsTrue(config.LoginUrl.EndsWith("/auth/login/"));
            Assert.IsTrue(config.ProfileUrl.EndsWith("/player/profile/"));
        }

        [Test]
        public void AuthPayloads_JsonSerialization_FormatsCorrectly()
        {
            var loginReq = new LoginRequest("survivor_one", "SecretPass123!");
            string json = JsonUtility.ToJson(loginReq);

            Assert.IsTrue(json.Contains("survivor_one"));
            Assert.IsTrue(json.Contains("SecretPass123!"));
        }

        [Test]
        public void JwtTokenResponse_JsonDeserialization_ParsesCorrectly()
        {
            string json = "{\"access\":\"mock_access_token_123\",\"refresh\":\"mock_refresh_token_456\"}";
            var response = JsonUtility.FromJson<JwtTokenResponse>(json);

            Assert.AreEqual("mock_access_token_123", response.access);
            Assert.AreEqual("mock_refresh_token_456", response.refresh);
        }
    }
}
