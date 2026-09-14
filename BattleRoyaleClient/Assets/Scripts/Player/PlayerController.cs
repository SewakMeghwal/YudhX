using UnityEngine;

namespace BattleRoyale.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private MovementSettings settings;

        [Header("Dependencies")]
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private PlayerAnimationHooks animationHooks;
        [SerializeField] private Transform mainCamera;

        private void Awake()
        {
            if (movement == null) movement = GetComponent<PlayerMovement>();
            if (inputHandler == null) inputHandler = GetComponent<PlayerInputHandler>();
            if (animationHooks == null) animationHooks = GetComponentInChildren<PlayerAnimationHooks>();

            if (mainCamera == null && Camera.main != null)
            {
                mainCamera = Camera.main.transform;
            }
        }

        private void Start()
        {
            if (movement != null && settings != null)
            {
                movement.Initialize(settings, mainCamera);
            }

            // Hook animation events
            if (movement != null && animationHooks != null)
            {
                movement.OnJumpTriggered += animationHooks.TriggerJumpAnimation;
            }
        }

        private void Update()
        {
            if (movement == null || inputHandler == null) return;

            // Execute Movement step using decoupled inputs
            movement.ProcessMovement(
                inputHandler.MoveInput,
                inputHandler.IsSprintPressed,
                inputHandler.IsCrouchPressed,
                inputHandler.IsJumpPressed
            );

            // Update Animations
            if (animationHooks != null)
            {
                PlayerRuntimeState state = movement.CurrentState;
                animationHooks.UpdateLocomotionParameters(
                    state.CurrentSpeed,
                    state.VerticalVelocity,
                    state.IsGrounded,
                    state.IsCrouching
                );
            }
        }

        private void OnDestroy()
        {
            if (movement != null && animationHooks != null)
            {
                movement.OnJumpTriggered -= animationHooks.TriggerJumpAnimation;
            }
        }

        public void BindCamera(Transform cameraTransform)
        {
            mainCamera = cameraTransform;
            if (movement != null && settings != null)
            {
                movement.Initialize(settings, mainCamera);
            }
        }
    }
}
