using UnityEngine;

namespace BattleRoyale.InventorySystem
{
    public enum ItemCategory
    {
        Weapon,
        Ammo,
        Consumable,
        Armor,
        Attachment,
        Throwable
    }

    [CreateAssetMenu(fileName = "InventoryItemData", menuName = "BattleRoyale/Inventory/Item Data")]
    public class InventoryItemData : ScriptableObject
    {
        [Header("Item Metadata")]
        [SerializeField] private string itemId = "item_generic";
        [SerializeField] private string itemName = "Generic Item";
        [SerializeField] private ItemCategory category = ItemCategory.Ammo;
        [SerializeField] private Sprite icon;

        [Header("Stacking & Storage")]
        [SerializeField] private int maxStackSize = 100;
        [SerializeField] private float itemWeight = 0.1f;

        public string ItemId => itemId;
        public string ItemName => itemName;
        public ItemCategory Category => category;
        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;
        public float ItemWeight => itemWeight;
    }
}
