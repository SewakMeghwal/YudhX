using System;
using BattleRoyale.InventorySystem;
using BattleRoyale.Loot;

namespace BattleRoyale.Multiplayer.Inventory
{
    public interface INetworkedInventory
    {
        void RequestPickup(LootPickup pickup);
        void RequestDropItem(InventoryItemData item, int quantity);
        void SyncWeaponSlot(WeaponSlotIndex slot);

        event Action<ulong, LootPickup> OnLootPickedUpBroadcast;
        event Action<ulong, WeaponSlotIndex> OnWeaponSlotChangedBroadcast;
    }
}
