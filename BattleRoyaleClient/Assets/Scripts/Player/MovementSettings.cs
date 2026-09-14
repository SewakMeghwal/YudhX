using UnityEngine;

namespace BattleRoyale.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "BattleRoyale/Player/Movement Settings")]
    public class MovementSettings : ScriptableObject
    {
        [Header("Movement Speeds (m/s)")]
        [Tooltip("Speed when walking normally")]
        [SerializeField] private float walkSpeed = 3.0f;

        [Tooltip("Speed when running")]
        [SerializeField] private float runSpeed = 5.5f;

        [Tooltip("Speed when sprinting")]
        [SerializeField] private float sprintSpeed = 8.0f;

        [Tooltip("Speed when crouching")]
        [SerializeField] private float crouchSpeed = 2.2f;

        [Header("Acceleration & Responsiveness")]
        [Tooltip("Rate of acceleration towards target speed")]
        [SerializeField] private float acceleration = 12.0f;

        [Tooltip("Rate of deceleration when input stops")]
        [SerializeField] private float deceleration = 16.0f;

        [Tooltip("Rotation speed towards movement or camera direction (deg/s)")]
        [SerializeField] private float rotationSpeed = 720.0f;

        [Tooltip("Multiplier for movement speed while airborne")]
        [Range(0f, 1f)]
        [SerializeField] private float airControlMultiplier = 0.4f;

        [Header("Jumping & Gravity")]
        [Tooltip("Height of jump in meters")]
        [SerializeField] private float jumpHeight = 1.35f;

        [Tooltip("Custom gravity value (m/s^2)")]
        [SerializeField] private float gravity = -19.62f;

        [Tooltip("Downward force applied when grounded to keep character glued to slopes")]
        [SerializeField] private float groundedStickyForce = -4.0f;

        [Header("Ground Detection")]
        [Tooltip("Radius of sphere cast for ground checking")]
        [SerializeField] private float groundCheckRadius = 0.28f;

        [Tooltip("Offset relative to character base for ground sphere cast")]
        [SerializeField] private float groundCheckOffset = 0.05f;

        [Tooltip("Layer mask for environment collision")]
        [SerializeField] private LayerMask groundLayers = ~0;

        [Header("Crouching Mechanics")]
        [Tooltip("CharacterController height when standing")]
        [SerializeField] private float standingHeight = 2.0f;

        [Tooltip("CharacterController height when crouching")]
        [SerializeField] private float crouchingHeight = 1.2f;

        [Tooltip("CharacterController center when standing")]
        [SerializeField] private Vector3 standingCenter = new Vector3(0, 1.0f, 0);

        [Tooltip("CharacterController center when crouching")]
        [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.6f, 0);

        [Tooltip("Speed of height interpolation between crouch and stand")]
        [SerializeField] private float crouchTransitionSpeed = 10.0f;

        // Public Accessors
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;
        public float SprintSpeed => sprintSpeed;
        public float CrouchSpeed => crouchSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float RotationSpeed => rotationSpeed;
        public float AirControlMultiplier => airControlMultiplier;
        public float JumpHeight => jumpHeight;
        public float Gravity => gravity;
        public float GroundedStickyForce => groundedStickyForce;
        public float GroundCheckRadius => groundCheckRadius;
        public float GroundCheckOffset => groundCheckOffset;
        public LayerMask GroundLayers => groundLayers;
        public float StandingHeight => standingHeight;
        public float CrouchingHeight => crouchingHeight;
        public Vector3 StandingCenter => standingCenter;
        public Vector3 CrouchingCenter => crouchingCenter;
        public float CrouchTransitionSpeed => crouchTransitionSpeed;
    }
}
