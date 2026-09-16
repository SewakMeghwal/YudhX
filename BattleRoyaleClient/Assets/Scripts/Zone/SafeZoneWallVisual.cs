using UnityEngine;

namespace BattleRoyale.Zone
{
    public class SafeZoneWallVisual : MonoBehaviour
    {
        [SerializeField] private Transform wallCylinderTransform;

        public void UpdateZoneVisuals(Vector3 center, float radius)
        {
            if (wallCylinderTransform == null) wallCylinderTransform = transform;

            wallCylinderTransform.position = center;
            float diameter = radius * 2.0f;
            wallCylinderTransform.localScale = new Vector3(diameter, wallCylinderTransform.localScale.y, diameter);
        }
    }
}
