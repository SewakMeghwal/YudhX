using System;
using UnityEngine;
using BattleRoyale.Player;
using BattleRoyale.CameraSystem;

namespace BattleRoyale.Multiplayer
{
    public class NetworkPlayer : MonoBehaviour
    {
        [Header("Network Identity")]
        [SerializeField] private ulong networkId;
        [SerializeField] private bool isLocalPlayer;
        [SerializeField] private NetworkConfig config;

        [Header("Player Components")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private CharacterController characterController;

        // Remote Interpolation Buffers
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private float lastStateTimestamp;

        public ulong NetworkId => networkId;
        public bool IsLocalPlayer => isLocalPlayer;

        public void InitializeNetworkOwner(ulong id, bool isLocal, NetworkConfig netConfig)
        {
            networkId = id;
            isLocalPlayer = isLocal;
            config = netConfig;

            if (playerController == null) playerController = GetComponent<PlayerController>();
            if (inputHandler == null) inputHandler = GetComponent<PlayerInputHandler>();
            if (characterController == null) characterController = GetComponent<CharacterController>();

            if (!isLocalPlayer)
            {
                // Remote Proxy: Disable local input reading and physics controller driving
                if (inputHandler != null) inputHandler.SetInputEnabled(false);
                if (characterController != null) characterController.enabled = false;
            }
            else
            {
                // Local Owner: Bind ThirdPersonCamera
                ThirdPersonCamera mainCam = FindObjectOfType<ThirdPersonCamera>();
                if (mainCam != null)
                {
                    mainCam.SetTarget(transform);
                }
            }

            targetPosition = transform.position;
            targetRotation = transform.rotation;
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                // Local player drives movement via PlayerController & CharacterController
                return;
            }

            // Remote Proxy Player Position/Rotation Interpolation
            float lerpSpeed = (config != null) ? (1.0f / config.InterpolationBufferTime) : 15.0f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        }

        public void ReceiveServerStateUpdate(NetworkPlayerState state)
        {
            if (isLocalPlayer) return;

            // Server-authoritative anti-cheat speed validation check
            if (config != null)
            {
                float travelDistance = Vector3.Distance(targetPosition, state.Position);
                float deltaTime = Mathf.Max(0.01f, state.Timestamp - lastStateTimestamp);
                float calculatedSpeed = travelDistance / deltaTime;

                if (calculatedSpeed > config.MaxAllowedSpeedThreshold)
                {
                    Debug.LogWarning($"[Anti-Cheat] Player {networkId} exceeded speed limit: {calculatedSpeed:F1} m/s");
                }
            }

            targetPosition = state.Position;
            targetRotation = Quaternion.Euler(0f, state.YawRotation, 0f);
            lastStateTimestamp = state.Timestamp;
        }

        public NetworkPlayerState GetCurrentState()
        {
            return new NetworkPlayerState(
                networkId,
                transform.position,
                transform.eulerAngles.y,
                0f,
                characterController != null ? characterController.velocity.magnitude : 0f,
                characterController != null ? characterController.isGrounded : true,
                false,
                Time.time
            );
        }
    }
}
