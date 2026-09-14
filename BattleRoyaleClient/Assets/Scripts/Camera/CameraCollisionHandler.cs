using UnityEngine;

namespace BattleRoyale.CameraSystem
{
    public class CameraCollisionHandler : MonoBehaviour
    {
        [SerializeField] private CameraSettings settings;

        public void Initialize(CameraSettings cameraSettings)
        {
            settings = cameraSettings;
        }

        public Vector3 CalculateUnoccludedPosition(Vector3 pivotPosition, Vector3 desiredPosition)
        {
            if (settings == null) return desiredPosition;

            Vector3 direction = desiredPosition - pivotPosition;
            float desiredDistance = direction.magnitude;

            if (desiredDistance <= 0.001f) return desiredPosition;

            direction.Normalize();

            if (Physics.SphereCast(
                pivotPosition,
                settings.CollisionRadius,
                direction,
                out RaycastHit hit,
                desiredDistance,
                settings.CollisionLayers,
                QueryTriggerInteraction.Ignore))
            {
                float hitDistance = Mathf.Clamp(hit.distance - settings.CollisionRadius, settings.MinCollisionDistance, desiredDistance);
                return pivotPosition + direction * hitDistance;
            }

            return desiredPosition;
        }
    }
}
