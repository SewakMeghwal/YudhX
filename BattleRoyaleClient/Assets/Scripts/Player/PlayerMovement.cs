using System;
using UnityEngine;

namespace BattleRoyale.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private MovementSettings settings;
        [SerializeField] private GroundChecker groundChecker;

        private CharacterController characterController;
        private Transform cameraTransform;

        private Vector3 currentVelocity;
        private float verticalVelocity;
        private bool isCrouching;
        private PlayerRuntimeState currentState;

        public event Action<PlayerRuntimeState> OnStateChanged;
        public event Action OnJumpTriggered;
        public event Action OnLanded;

        public PlayerRuntimeState CurrentState => currentState;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (groundChecker == null)
            {
                groundChecker = GetComponent<GroundChecker>();
            }
        }

        public void Initialize(MovementSettings movementSettings, Transform mainCameraTransform)
        {
            settings = movementSettings;
            cameraTransform = mainCameraTransform;

            if (groundChecker != null && settings != null)
            {
                groundChecker.Initialize(settings);
            }
        }

        public void ProcessMovement(Vector2 moveInput, bool isSprintPressed, bool isCrouchPressed, bool isJumpPressed)
        {
            if (settings == null || characterController == null) return;

            // 1. Ground Checking
            bool isGrounded = groundChecker != null && groundChecker.CheckGround(transform.position);

            // Reset vertical velocity when grounded
            if (isGrounded && verticalVelocity < 0)
            {
                if (verticalVelocity < -8.0f)
                {
                    OnLanded?.Invoke();
                }
                verticalVelocity = settings.GroundedStickyForce;
            }

            // 2. Handle Crouch height & state
            HandleCrouch(isCrouchPressed);

            // 3. Target Speed Calculation
            float targetSpeed = CalculateTargetSpeed(moveInput, isSprintPressed);

            // 4. Direction Relative to Camera
            Vector3 targetDirection = CalculateTargetDirection(moveInput);

            // 5. Horizontal Velocity Acceleration/Deceleration
            float accelRate = (moveInput.magnitude > 0.01f) ? settings.Acceleration : settings.Deceleration;
            if (!isGrounded) accelRate *= settings.AirControlMultiplier;

            Vector3 targetHorizontalVelocity = targetDirection * targetSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetHorizontalVelocity, accelRate * Time.deltaTime);

            // 6. Character Rotation
            if (targetDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, settings.RotationSpeed * Time.deltaTime);
            }

            // 7. Jump & Gravity Physics
            if (isJumpPressed && isGrounded && !isCrouching)
            {
                verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(settings.Gravity) * settings.JumpHeight);
                OnJumpTriggered?.Invoke();
                isGrounded = false;
            }

            // Apply gravity
            if (!isGrounded)
            {
                verticalVelocity += settings.Gravity * Time.deltaTime;
            }

            // 8. Execute Movement via CharacterController
            Vector3 finalMotion = (currentVelocity + Vector3.up * verticalVelocity) * Time.deltaTime;
            characterController.Move(finalMotion);

            // 9. Update Runtime State Container
            UpdatePlayerState(isGrounded, isSprintPressed, moveInput.magnitude > 0.01f);
        }

        private float CalculateTargetSpeed(Vector2 moveInput, bool isSprintPressed)
        {
            if (moveInput.sqrMagnitude < 0.001f) return 0f;

            if (isCrouching) return settings.CrouchSpeed;
            if (isSprintPressed && moveInput.y > 0.1f) return settings.SprintSpeed;
            return settings.RunSpeed;
        }

        private Vector3 CalculateTargetDirection(Vector2 moveInput)
        {
            if (moveInput.sqrMagnitude < 0.001f) return Vector3.zero;

            Vector3 cameraForward = Vector3.forward;
            Vector3 cameraRight = Vector3.right;

            if (cameraTransform != null)
            {
                cameraForward = cameraTransform.forward;
                cameraRight = cameraTransform.right;
                cameraForward.y = 0f;
                cameraRight.y = 0f;
                cameraForward.Normalize();
                cameraRight.Normalize();
            }

            return (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;
        }

        private void HandleCrouch(bool crouchInput)
        {
            isCrouching = crouchInput;

            float targetHeight = isCrouching ? settings.CrouchingHeight : settings.StandingHeight;
            Vector3 targetCenter = isCrouching ? settings.CrouchingCenter : settings.StandingCenter;

            characterController.height = Mathf.Lerp(characterController.height, targetHeight, settings.CrouchTransitionSpeed * Time.deltaTime);
            characterController.center = Vector3.Lerp(characterController.center, targetCenter, settings.CrouchTransitionSpeed * Time.deltaTime);
        }

        private void UpdatePlayerState(bool isGrounded, bool isSprinting, bool isMoving)
        {
            MovementState moveState;

            if (!isGrounded)
            {
                moveState = MovementState.Airborne;
            }
            else if (isCrouching)
            {
                moveState = MovementState.Crouching;
            }
            else if (!isMoving)
            {
                moveState = MovementState.Idle;
            }
            else if (isSprinting)
            {
                moveState = MovementState.Sprinting;
            }
            else
            {
                moveState = MovementState.Running;
            }

            StanceState stance = isCrouching ? StanceState.Crouching : StanceState.Standing;
            float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

            PlayerRuntimeState newState = new PlayerRuntimeState
            {
                MovementState = moveState,
                StanceState = stance,
                IsGrounded = isGrounded,
                IsSprinting = isSprinting,
                IsCrouching = isCrouching,
                CurrentSpeed = currentHorizontalSpeed,
                VerticalVelocity = verticalVelocity
            };

            if (!newState.Equals(currentState))
            {
                currentState = newState;
                OnStateChanged?.Invoke(currentState);
            }
        }
    }
}
