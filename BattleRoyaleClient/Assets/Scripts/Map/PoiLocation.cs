using UnityEngine;

namespace BattleRoyale.Map
{
    public class PoiLocation : MonoBehaviour
    {
        [SerializeField] private string poiName = "Central Outpost";
        [SerializeField] private float poiRadius = 50.0f;

        public string PoiName => poiName;
        public float PoiRadius => poiRadius;

        public bool IsInsidePOI(Vector3 position)
        {
            return Vector3.Distance(transform.position, position) <= poiRadius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, poiRadius);
        }
    }
}
