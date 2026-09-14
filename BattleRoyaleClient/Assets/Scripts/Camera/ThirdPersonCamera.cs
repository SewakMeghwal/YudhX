using System;
using UnityEngine;

namespace BattleRoyale.CameraSystem
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CameraCollisionHandler))]
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private CameraSettings settings;
        [SerializeField] private Transform targetPlayer;

        [Header("Dependencies")]
        [SerializeField] private CameraInputHandler inputHandler;
        [SerializeField] private CameraCollisionHandler collisionHandler;

        private Camera targetCamera;
        private ShoulderSide currentShoulder = ShoulderSide.Right;
        private CameraMode currentMode = CameraMode.DefaultThirdPerson;

        private float currentYaw;
        private float currentPitch;
        private Vector3 currentOffset;
        private float targetFOV;

        public event Action<CameraRuntimeState> OnCameraStateChanged;

        public Transform TargetPlayer => targetPlayer;

        private void Awake()
        {
            targetCamera = GetComponent<Camera>();
            collisionHandler = GetComponent<CameraCollisionHandler>();
            if (inputHandler == null)
            {
                inputHandler = GetComponent<CameraInputHandler>();
            }
        }

        private void Start()
        {
            if (collisionHandler != null && settings != null)
            {
                collisionHandler.Initialize(settings);
            }

            if (inputHandler != null)
            {
                inputHandler.OnShoulderSwitchedEvent += ToggleShoulderSide;
            }

            if (targetPlayer != null)
            {
                currentYaw = targetPlayer.eulerAngles.y;
            }

            if (settings != null)
            {
                targetFOV = settings.DefaultFOV;
                currentOffset = settings.RightShoulderOffset;
            }
        }

        public void SetTarget(Transform playerTransform)
        {
            targetPlayer = playerTransform;
            if (targetPlayer != null)
            {
                currentYaw = targetPlayer.eulerAngles.y;
            }
        }

        private void LateUpdate()
        {
            if (targetPlayer == null || settings == null) return;

            ProcessCameraInputs();
            UpdateCameraTransform();
        }

        private void ProcessCameraInputs()
        {
            if (inputHandler == null) return;

            Vector2 lookDelta = inputHandler.LookDelta;
            bool isAiming = inputHandler.IsAiming;

            // Rotation accumulation
            currentYaw += lookDelta.x * settings.SensitivityX;
            currentPitch -= lookDelta.y * settings.SensitivityY;
            currentPitch = Mathf.Clamp(currentPitch, settings.MinPitch, settings.MaxPitch);

            // Camera Mode Selection
            if (isAiming)
            {
                currentMode = CameraMode.AimDownSights;
                targetFOV = settings.AimFOV;
            }
            else
            {
                currentMode = CameraMode.DefaultThirdPerson;
                targetFOV = settings.DefaultFOV;
            }
        }

        private void UpdateCameraTransform()
        {
            // 1. Pivot Position (Player + height offset)
            Vector3 pivotPosition = targetPlayer.position + settings.PivotOffset;

            // 2. Desired Offset based on Shoulder and Mode
            Vector3 baseOffset = (currentMode == CameraMode.AimDownSights) ? settings.AimOffset : settings.RightShoulderOffset;
            if (currentShoulder == ShoulderSide.Left)
            {
                baseOffset.x = -baseOffset.x; // Invert horizontal shoulder offset
            }

            // Smooth Offset Lerp
            currentOffset = Vector3.Lerp(currentOffset, baseOffset, settings.PositionDamping * Time.deltaTime);

            // 3. Rotation Quaternion
            Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

            // 4. Calculate Desired Unoccluded Position
            Vector3 desiredWorldPosition = pivotPosition + targetRotation * currentOffset;
            Vector3 finalPosition = collisionHandler.CalculateUnoccludedPosition(pivotPosition, desiredWorldPosition);

            // 5. Apply Position and Rotation smoothly
            transform.position = finalPosition;
            transform.rotation = targetRotation;

            // 6. Smooth Field of View (FOV) Lerp
            if (targetCamera != null)
            {
                targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, targetFOV, settings.FOVLerpSpeed * Time.deltaTime);
            }

            // 7. Fire state update event
            FireStateChangeEvent(finalPosition, pivotPosition);
        }

        public void ToggleShoulderSide()
        {
            currentShoulder = (currentShoulder == ShoulderSide.Right) ? ShoulderSide.Left : ShoulderSide.Right;
        }

        private void FireStateChangeEvent(Vector3 finalPos, Vector3 pivotPos)
        {
            CameraRuntimeState state = new CameraRuntimeState
            {
                Mode = currentMode,
                Shoulder = currentShoulder,
                Pitch = currentPitch,
                Yaw = currentYaw,
                CurrentFOV = targetCamera != null ? targetCamera.fieldOfView : 60f,
                CurrentDistance = Vector3.Distance(finalPos, pivotPos)
            };

            OnCameraStateChanged?.Invoke(state);
        }

        private void OnDestroy()
        {
            if (inputHandler != null)
            {
                inputHandler.OnShoulderSwitchedEvent -= ToggleShoulderSide;
            }
        }
    }
}
