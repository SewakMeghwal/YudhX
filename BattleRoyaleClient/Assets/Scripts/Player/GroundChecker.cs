using UnityEngine;

namespace BattleRoyale.Player
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private MovementSettings settings;

        public bool IsGrounded { get; private set; }
        public Vector3 GroundNormal { get; private set; } = Vector3.up;
        public float SurfaceAngle { get; private set; }

        public void Initialize(MovementSettings movementSettings)
        {
            settings = movementSettings;
        }

        public bool CheckGround(Vector3 transformPosition)
        {
            if (settings == null) return false;

            Vector3 spherePosition = transformPosition + Vector3.up * settings.GroundCheckOffset;
            
            bool hitGround = Physics.CheckSphere(
                spherePosition,
                settings.GroundCheckRadius,
                settings.GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (Physics.Raycast(spherePosition, Vector3.down, out RaycastHit hitInfo, settings.GroundCheckRadius + 0.1f, settings.GroundLayers))
            {
                GroundNormal = hitInfo.normal;
                SurfaceAngle = Vector3.Angle(Vector3.up, GroundNormal);
            }
            else
            {
                GroundNormal = Vector3.up;
                SurfaceAngle = 0f;
            }

            IsGrounded = hitGround;
            return IsGrounded;
        }

        private void OnDrawGizmosSelected()
        {
            if (settings == null) return;

            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Vector3 spherePosition = transform.position + Vector3.up * settings.GroundCheckOffset;
            Gizmos.DrawWireSphere(spherePosition, settings.GroundCheckRadius);
        }
    }
}
