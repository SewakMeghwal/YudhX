#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Loot;
using BattleRoyale.InventorySystem;

namespace BattleRoyale.Tests
{
    public class LootUnitTests
    {
        [Test]
        public void LootRarityColor_ReturnsValidColors()
        {
            Color commonColor = LootRarityColor.GetColor(LootRarity.Common);
            Color legendaryColor = LootRarityColor.GetColor(LootRarity.Legendary);

            Assert.AreNotEqual(commonColor, legendaryColor, "Legendary color should differ from Common color");
            Assert.AreEqual(1.0f, legendaryColor.r, "Legendary color should have max red channel for gold tint");
        }

        [Test]
        public void LootPickup_InteractionPrompt_FormatsCorrectly()
        {
            var pickupObj = new GameObject("TestPickup");
            var pickup = pickupObj.AddComponent<LootPickup>();

            var item = ScriptableObject.CreateInstance<InventoryItemData>();
            // Use reflection or constructor
            typeof(InventoryItemData).GetField("itemName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(item, "5.56mm Ammo");

            pickup.Initialize(item, 30, LootRarity.Uncommon);

            string prompt = pickup.InteractionPrompt;
            Assert.IsTrue(prompt.Contains("Press F to Pick Up 5.56mm Ammo (30)"));

            Object.DestroyImmediate(pickupObj);
        }
    }
}

#endif
