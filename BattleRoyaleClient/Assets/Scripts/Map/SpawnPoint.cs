using UnityEngine;

namespace BattleRoyale.Map
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private int spawnId;
        [SerializeField] private bool isOccupied;

        public int SpawnId => spawnId;
        public bool IsOccupied => isOccupied;

        public void SetOccupied(bool occupied)
        {
            isOccupied = occupied;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = isOccupied ? Color.red : Color.cyan;
            Gizmos.DrawWireCube(transform.position + Vector3.up * 1.0f, new Vector3(0.8f, 2.0f, 0.8f));
            Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, transform.forward * 1.2f);
        }
    }
}
