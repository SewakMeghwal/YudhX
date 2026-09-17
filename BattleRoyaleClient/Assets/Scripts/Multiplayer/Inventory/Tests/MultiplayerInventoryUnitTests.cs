using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Multiplayer.Inventory;

namespace BattleRoyale.Tests
{
    public class MultiplayerInventoryUnitTests
    {
        [Test]
        public void NetworkLootPickupRequest_Packing_PreservesData()
        {
            Vector3 pos = new Vector3(12.5f, 0.5f, -8.0f);
            var request = new NetworkLootPickupRequest(10, 999, pos, 45.2f);

            Assert.AreEqual(10, request.PlayerNetworkId);
            Assert.AreEqual(999, request.LootNetworkId);
            Assert.AreEqual(pos, request.InteractionPosition);
            Assert.AreEqual(45.2f, request.Timestamp);
        }

        [Test]
        public void NetworkLootManager_PickupDistance_IsValid()
        {
            var gameObject = new GameObject("TestNetLootManager");
            var manager = gameObject.AddComponent<NetworkLootManager>();

            Assert.Greater(manager.MaxPickupDistance, 2.0f);
            Assert.Less(manager.MaxPickupDistance, 5.0f);

            Object.DestroyImmediate(gameObject);
        }
    }
}
