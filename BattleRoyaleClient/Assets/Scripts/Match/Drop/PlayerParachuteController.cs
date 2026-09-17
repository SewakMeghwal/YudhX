using System;
using UnityEngine;
using BattleRoyale.Player;

namespace BattleRoyale.Match.Drop
{
    public class PlayerParachuteController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private ParachuteConfig config;
        [SerializeField] private GameObject parachuteMeshObject;

        [Header("Dependencies")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CharacterController characterController;

        private DropState currentDropState = DropState.Landed;
        private AirplaneFlightPath activeFlightPath;

        public DropState CurrentDropState => currentDropState;

        public event Action<DropState> OnDropStateChanged;

        private void Awake()
        {
            if (playerController == null) playerController = GetComponent<PlayerController>();
            if (characterController == null) characterController = GetComponent<CharacterController>();
        }

        public void BoardAirplane(AirplaneFlightPath flightPath)
        {
            activeFlightPath = flightPath;
            currentDropState = DropState.InAirplane;

            if (characterController != null) characterController.enabled = false;
            if (playerController != null) playerController.enabled = false;

            if (parachuteMeshObject != null) parachuteMeshObject.SetActive(false);

            OnDropStateChanged?.Invoke(currentDropState);
        }

        private void Update()
        {
            switch (currentDropState)
            {
                case DropState.InAirplane:
                    UpdateInAirplaneState();
                    break;

                case DropState.Freefalling:
                    UpdateFreefallState();
                    break;

                case DropState.Parachuting:
                    UpdateParachuteState();
                    break;
            }
        }

        private void UpdateInAirplaneState()
        {
            if (activeFlightPath == null) return;

            // Follow airplane mesh transform position
            transform.position = activeFlightPath.CurrentAirplanePosition;
            transform.rotation = activeFlightPath.CurrentAirplaneRotation;

            // Check for Eject trigger (Press F or flight path end)
            if (Input.GetKeyDown(KeyCode.F) || !activeFlightPath.IsFlying)
            {
                EjectFromAirplane();
            }
        }

        public void EjectFromAirplane()
        {
            currentDropState = DropState.Freefalling;
            if (characterController != null) characterController.enabled = true;

            OnDropStateChanged?.Invoke(currentDropState);
        }

        private void UpdateFreefallState()
        {
            if (config == null || characterController == null) return;

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            // Pitch dive adjustment (Mouse Y tilts dive speed)
            float mousePitch = Input.GetAxis("Mouse Y");
            float fallSpeed = Mathf.Lerp(config.MinFreefallFallSpeed, config.MaxFreefallDiveSpeed, Mathf.Clamp01((mousePitch + 1f) * 0.5f));

            Vector3 moveDir = (transform.forward * moveY + transform.right * moveX).normalized * config.FreefallForwardSpeed;
            Vector3 verticalVelocity = Vector3.down * fallSpeed;

            characterController.Move((moveDir + verticalVelocity) * Time.deltaTime);

            // Manual Parachute Deploy (Space) or Auto Deploy at Threshold Altitude
            float currentAltitude = transform.position.y;
            if (Input.GetKeyDown(KeyCode.Space) || currentAltitude <= config.AutoDeployAltitudeMeters)
            {
                DeployParachute();
            }
        }

        public void DeployParachute()
        {
            currentDropState = DropState.Parachuting;
            if (parachuteMeshObject != null) parachuteMeshObject.SetActive(true);

            OnDropStateChanged?.Invoke(currentDropState);
        }

        private void UpdateParachuteState()
        {
            if (config == null || characterController == null) return;

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            Vector3 glideVector = (transform.forward * Mathf.Max(0.2f, moveY) + transform.right * moveX).normalized * config.ParachuteGlideSpeed;
            Vector3 descentVector = Vector3.down * config.ParachuteDescentRate;

            characterController.Move((glideVector + descentVector) * Time.deltaTime);

            // Ground Touchdown Detection
            if (characterController.isGrounded)
            {
                TouchdownLanded();
            }
        }

        private void TouchdownLanded()
        {
            currentDropState = DropState.Landed;
            if (parachuteMeshObject != null) parachuteMeshObject.SetActive(false);

            if (playerController != null) playerController.enabled = true;

            OnDropStateChanged?.Invoke(currentDropState);
        }
    }
}
