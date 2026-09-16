using UnityEngine;
using BattleRoyale.InventorySystem;

namespace BattleRoyale.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private LootTableData lootTable;
        [SerializeField] private GameObject defaultLootPrefab;
        [Range(0f, 1f)] [SerializeField] private float spawnChance = 0.85f;

        private void Start()
        {
            SpawnLoot();
        }

        public void SpawnLoot()
        {
            if (lootTable == null) return;
            if (UnityEngine.Random.value > spawnChance) return; // Empty loot spot roll

            if (lootTable.GetRandomLoot(out InventoryItemData item, out int quantity, out LootRarity rarity))
            {
                GameObject pickupObj = defaultLootPrefab != null
                    ? Instantiate(defaultLootPrefab, transform.position, Quaternion.identity)
                    : new GameObject($"Loot_{item.ItemName}");

                pickupObj.transform.position = transform.position;

                LootPickup pickup = pickupObj.GetComponent<LootPickup>();
                if (pickup == null)
                {
                    pickup = pickupObj.AddComponent<LootPickup>();
                }

                pickup.Initialize(item, quantity, rarity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.35f);
        }
    }
}
