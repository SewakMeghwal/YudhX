using UnityEngine;

namespace BattleRoyale.CameraSystem
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "BattleRoyale/Camera/Camera Settings")]
    public class CameraSettings : ScriptableObject
    {
        [Header("Target & Offsets")]
        [Tooltip("Pivot offset relative to target player position (height above pivot)")]
        [SerializeField] private Vector3 pivotOffset = new Vector3(0, 1.6f, 0);

        [Tooltip("Camera offset when over the right shoulder")]
        [SerializeField] private Vector3 rightShoulderOffset = new Vector3(0.55f, 0.15f, -2.5f);

        [Tooltip("Camera offset when aiming down sights (ADS)")]
        [SerializeField] private Vector3 aimOffset = new Vector3(0.35f, 0.1f, -1.2f);

        [Header("Rotation & Pitch Limits")]
        [Tooltip("Horizontal sensitivity (deg per mouse unit)")]
        [SerializeField] private float sensitivityX = 2.0f;

        [Tooltip("Vertical sensitivity (deg per mouse unit)")]
        [SerializeField] private float sensitivityY = 2.0f;

        [Tooltip("Minimum pitch angle (looking down)")]
        [SerializeField] private float minPitch = -45.0f;

        [Tooltip("Maximum pitch angle (looking up)")]
        [SerializeField] private float maxPitch = 70.0f;

        [Header("Field Of View (FOV)")]
        [Tooltip("Default field of view in degrees")]
        [SerializeField] private float defaultFOV = 60.0f;

        [Tooltip("Aim down sights FOV in degrees")]
        [SerializeField] private float aimFOV = 45.0f;

        [Tooltip("Sprint FOV boost in degrees")]
        [SerializeField] private float sprintFOV = 66.0f;

        [Tooltip("Speed of FOV transitions")]
        [SerializeField] private float fovLerpSpeed = 10.0f;

        [Header("Damping & Smoothing")]
        [Tooltip("Speed of camera position smoothing")]
        [SerializeField] private float positionDamping = 15.0f;

        [Tooltip("Speed of camera rotation smoothing")]
        [SerializeField] private float rotationDamping = 20.0f;

        [Header("Collision & Occlusion")]
        [Tooltip("SphereCast radius for wall and terrain collision check")]
        [SerializeField] private float collisionRadius = 0.2f;

        [Tooltip("Minimum allowed distance from pivot before zooming in completely")]
        [SerializeField] private float minCollisionDistance = 0.4f;

        [Tooltip("Layers that block camera view")]
        [SerializeField] private LayerMask collisionLayers = ~0;

        // Public Accessors
        public Vector3 PivotOffset => pivotOffset;
        public Vector3 RightShoulderOffset => rightShoulderOffset;
        public Vector3 AimOffset => aimOffset;
        public float SensitivityX => sensitivityX;
        public float SensitivityY => sensitivityY;
        public float MinPitch => minPitch;
        public float MaxPitch => maxPitch;
        public float DefaultFOV => defaultFOV;
        public float AimFOV => aimFOV;
        public float SprintFOV => sprintFOV;
        public float FOVLerpSpeed => fovLerpSpeed;
        public float PositionDamping => positionDamping;
        public float RotationDamping => rotationDamping;
        public float CollisionRadius => collisionRadius;
        public float MinCollisionDistance => minCollisionDistance;
        public LayerMask CollisionLayers => collisionLayers;
    }
}
