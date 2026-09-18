using System;
using UnityEngine;
using BattleRoyale.Player;
using BattleRoyale.CameraSystem;

namespace BattleRoyale.UI.Mobile
{
    public class MobileInputHandler : MonoBehaviour, IPlayerInput, ICameraInput
    {
        [Header("UI Controls")]
        [SerializeField] private VirtualJoystick joystick;
        [SerializeField] private TouchCameraPad cameraPad;
        [SerializeField] private TouchButton jumpButton;
        [SerializeField] private TouchButton crouchButton;
        [SerializeField] private TouchButton sprintButton;
        [SerializeField] private TouchButton aimButton;
        [SerializeField] private TouchButton fireButton;

        public Vector2 MoveInput => joystick != null ? joystick.InputVector : Vector2.zero;
        public Vector2 LookInput => cameraPad != null ? cameraPad.LookDelta : Vector2.zero;
        public Vector2 LookDelta => LookInput;

        public bool IsSprintPressed => sprintButton != null && sprintButton.IsPressed;
        public bool IsCrouchPressed => crouchButton != null && crouchButton.IsPressed;
        public bool IsJumpPressed => jumpButton != null && jumpButton.IsPressed;
        public bool IsAiming => aimButton != null && aimButton.IsPressed;
        public bool IsFirePressed => fireButton != null && fireButton.IsPressed;
        public bool IsShoulderSwitchPressed => false;

        public event Action OnJumpPressedEvent;
        public event Action OnCrouchToggledEvent;
        public event Action OnShoulderSwitchedEvent;

        private void Start()
        {
            if (jumpButton != null) jumpButton.OnPressed += () => OnJumpPressedEvent?.Invoke();
            if (crouchButton != null) crouchButton.OnPressed += () => OnCrouchToggledEvent?.Invoke();
        }

        private void OnDestroy()
        {
            if (jumpButton != null) jumpButton.OnPressed -= () => OnJumpPressedEvent?.Invoke();
            if (crouchButton != null) crouchButton.OnPressed -= () => OnCrouchToggledEvent?.Invoke();
        }
    }
}
