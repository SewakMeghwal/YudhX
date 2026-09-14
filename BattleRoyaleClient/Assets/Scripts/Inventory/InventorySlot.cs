using System;

namespace BattleRoyale.InventorySystem
{
    [Serializable]
    public struct InventorySlot
    {
        public InventoryItemData Item;
        public int Quantity;

        public bool IsEmpty => Item == null || Quantity <= 0;
        public int MaxStack => Item != null ? Item.MaxStackSize : 0;

        public InventorySlot(InventoryItemData item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void Clear()
        {
            Item = null;
            Quantity = 0;
        }
    }
}
