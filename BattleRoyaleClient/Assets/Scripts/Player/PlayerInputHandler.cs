using System;
using UnityEngine;

namespace BattleRoyale.Player
{
    public class PlayerInputHandler : MonoBehaviour, IPlayerInput
    {
        [Header("Input Lock Settings")]
        [SerializeField] private bool inputEnabled = true;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsSprintPressed { get; private set; }
        public bool IsCrouchPressed { get; private set; }
        public bool IsJumpPressed { get; private set; }

        public event Action OnJumpPressedEvent;
        public event Action OnCrouchToggledEvent;

        public void SetInputEnabled(bool enabled)
        {
            inputEnabled = enabled;
            if (!enabled)
            {
                MoveInput = Vector2.zero;
                LookInput = Vector2.zero;
                IsSprintPressed = false;
                IsCrouchPressed = false;
                IsJumpPressed = false;
            }
        }

        private void Update()
        {
            if (!inputEnabled) return;

            ReadInputs();
        }

        private void ReadInputs()
        {
            // Horizontal & Vertical movement
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            MoveInput = new Vector2(moveX, moveY).normalized;

            // Mouse / Analog Look
            float lookX = Input.GetAxis("Mouse X");
            float lookY = Input.GetAxis("Mouse Y");
            LookInput = new Vector2(lookX, lookY);

            // Sprint modifier
            IsSprintPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton8);

            // Crouch trigger/toggle
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                IsCrouchPressed = !IsCrouchPressed;
                OnCrouchToggledEvent?.Invoke();
            }

            // Jump trigger
            IsJumpPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space);
            if (IsJumpPressed)
            {
                OnJumpPressedEvent?.Invoke();
            }
        }
    }
}
