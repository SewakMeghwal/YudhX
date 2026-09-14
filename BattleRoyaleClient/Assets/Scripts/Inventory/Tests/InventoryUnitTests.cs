using NUnit.Framework;
using UnityEngine;
using BattleRoyale.InventorySystem;
using BattleRoyale.Weapons;

namespace BattleRoyale.Tests
{
    public class InventoryUnitTests
    {
        [Test]
        public void PlayerBackpackInventory_AddItem_StacksCorrectly()
        {
            var gameObject = new GameObject("TestBackpack");
            var backpack = gameObject.AddComponent<PlayerBackpackInventory>();

            var ammoData = ScriptableObject.CreateInstance<InventoryItemData>();
            // Add 60 ammo items
            bool success = backpack.AddItem(ammoData, 60, out int remaining);

            Assert.IsTrue(success);
            Assert.AreEqual(0, remaining);
            Assert.AreEqual(60, backpack.GetItemCount(ammoData));

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void PlayerWeaponInventory_EquipAndSwitch_WorksCorrectly()
        {
            var gameObject = new GameObject("TestPlayerWeapons");
            var weaponInventory = gameObject.AddComponent<PlayerWeaponInventory>();

            var arObj = new GameObject("AR_Weapon");
            var arWeapon = arObj.AddComponent<Weapon>();
            arWeapon.Initialize(WeaponFactory.CreateAssaultRifleData());

            bool equipped = weaponInventory.EquipWeapon(arWeapon, WeaponSlotIndex.Primary);

            Assert.IsTrue(equipped);
            Assert.AreEqual(arWeapon, weaponInventory.ActiveWeapon);
            Assert.AreEqual(WeaponSlotIndex.Primary, weaponInventory.ActiveSlot);

            Object.DestroyImmediate(arObj);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void PlayerWeaponInventory_Holster_ClearsActiveWeapon()
        {
            var gameObject = new GameObject("TestPlayerWeapons");
            var weaponInventory = gameObject.AddComponent<PlayerWeaponInventory>();

            var pistolObj = new GameObject("Pistol");
            var pistol = pistolObj.AddComponent<Weapon>();
            pistol.Initialize(WeaponFactory.CreatePistolData());

            weaponInventory.EquipWeapon(pistol, WeaponSlotIndex.Sidearm);
            weaponInventory.HolsterWeapon();

            Assert.IsNull(weaponInventory.ActiveWeapon);
            Assert.AreEqual(WeaponSlotIndex.Holstered, weaponInventory.ActiveSlot);

            Object.DestroyImmediate(pistolObj);
            Object.DestroyImmediate(gameObject);
        }
    }
}
