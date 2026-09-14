using System;
using UnityEngine;

namespace BattleRoyale.CameraSystem
{
    public class CameraInputHandler : MonoBehaviour, ICameraInput
    {
        [Header("Settings")]
        [SerializeField] private bool lockCursorOnStart = true;
        [SerializeField] private bool inputEnabled = true;

        public Vector2 LookDelta { get; private set; }
        public bool IsAiming { get; private set; }
        public bool IsShoulderSwitchPressed { get; private set; }

        public event Action OnShoulderSwitchedEvent;

        private void Start()
        {
            if (lockCursorOnStart)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            if (!inputEnabled)
            {
                LookDelta = Vector2.zero;
                IsAiming = false;
                return;
            }

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            LookDelta = new Vector2(mouseX, mouseY);

            // Right click / Left trigger for ADS
            IsAiming = Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton5);

            // Tab / V key for shoulder switch
            if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Tab))
            {
                OnShoulderSwitchedEvent?.Invoke();
            }
        }

        public void SetInputEnabled(bool enabled)
        {
            inputEnabled = enabled;
            if (!enabled)
            {
                LookDelta = Vector2.zero;
                IsAiming = false;
            }
        }
    }
}
