#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Multiplayer.Server;

namespace BattleRoyale.Tests
{
    public class DedicatedServerUnitTests
    {
        [Test]
        public void ServerCommandLineArgs_ParseArguments_ParsesPortAndBatchmode()
        {
            string[] rawArgs = new string[] { "-batchmode", "-port", "8888", "-maxplayers", "16", "-ip", "192.168.1.50" };
            var cliArgs = new ServerCommandLineArgs(rawArgs);

            Assert.IsTrue(cliArgs.IsBatchMode);
            Assert.AreEqual(8888, cliArgs.Port);
            Assert.AreEqual(16, cliArgs.MaxPlayers);
            Assert.AreEqual("192.168.1.50", cliArgs.ListenIp);
        }

        [Test]
        public void ServerCommandLineArgs_DefaultValues_AreFallback()
        {
            var cliArgs = new ServerCommandLineArgs(new string[0]);

            Assert.IsFalse(cliArgs.IsBatchMode);
            Assert.AreEqual(7777, cliArgs.Port);
            Assert.AreEqual(8, cliArgs.MaxPlayers);
            Assert.AreEqual("0.0.0.0", cliArgs.ListenIp);
        }
    }
}

#endif
