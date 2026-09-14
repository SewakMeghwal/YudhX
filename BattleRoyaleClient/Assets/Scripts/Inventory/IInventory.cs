using System;
using BattleRoyale.Weapons;

namespace BattleRoyale.InventorySystem
{
    public interface IInventory
    {
        bool AddItem(InventoryItemData item, int quantity, out int remainingUnadded);
        bool RemoveItem(InventoryItemData item, int quantity);
        int GetItemCount(InventoryItemData item);
        bool HasCapacity(float additionalWeight);
    }

    public enum WeaponSlotIndex
    {
        Primary = 0,
        Secondary = 1,
        Sidearm = 2,
        Melee = 3,
        Holstered = -1
    }

    public interface IWeaponInventory
    {
        Weapon ActiveWeapon { get; }
        WeaponSlotIndex ActiveSlot { get; }

        bool EquipWeapon(Weapon weapon, WeaponSlotIndex slot);
        bool UnequipWeapon(WeaponSlotIndex slot, out Weapon unequippedWeapon);
        void SwitchToSlot(WeaponSlotIndex slot);
        void HolsterWeapon();

        event Action<WeaponSlotIndex, Weapon> OnWeaponEquipped;
        event Action<WeaponSlotIndex, Weapon> OnWeaponSwitched;
        event Action OnWeaponHolstered;
    }
}
