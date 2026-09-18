using System;
using System.Collections.Generic;
using UnityEngine;
using BattleRoyale.HealthSystem;
using BattleRoyale.Loot;

namespace BattleRoyale.Vehicles
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour, IVehicle, IDamageable, IInteractable
    {
        [Header("Configuration")]
        [SerializeField] private VehicleData vehicleData;
        [SerializeField] private List<VehicleSeat> seats = new List<VehicleSeat>();

        [Header("Wheel Colliders")]
        [SerializeField] private WheelCollider frontLeftWheel;
        [SerializeField] private WheelCollider frontRightWheel;
        [SerializeField] private WheelCollider rearLeftWheel;
        [SerializeField] private WheelCollider rearRightWheel;

        private Rigidbody rb;
        private VehicleState currentState = VehicleState.Empty;

        private float currentHealth;
        private float currentFuel;

        public VehicleData Data => vehicleData;
        public VehicleState State => currentState;
        public float CurrentSpeedKmH => rb != null ? rb.velocity.magnitude * 3.6f : 0f;
        public float CurrentHealth => currentHealth;
        public float CurrentFuel => currentFuel;
        public bool IsDead => currentState == VehicleState.Destroyed;
        public bool IsDowned => false;

        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnFuelChanged;
        public event Action OnVehicleExploded;

        public string InteractionPrompt => (vehicleData != null) ? $"Press F to Drive {vehicleData.VehicleName}" : "Press F to Drive Vehicle";

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (seats.Count == 0) seats.AddRange(GetComponentsInChildren<VehicleSeat>());

            if (vehicleData != null)
            {
                currentHealth = vehicleData.MaxHealth;
                currentFuel = vehicleData.MaxFuel;
            }
        }

        private void FixedUpdate()
        {
            if (currentState == VehicleState.Destroyed || vehicleData == null) return;

            // Check if Driver Seat is occupied
            VehicleSeat driverSeat = GetSeat(SeatType.Driver);
            if (driverSeat != null && driverSeat.IsOccupied && currentFuel > 0f)
            {
                ProcessDriverInput();
            }
            else
            {
                ApplyBrakes();
            }
        }

        private void ProcessDriverInput()
        {
            float moveY = Input.GetAxis("Vertical");
            float moveX = Input.GetAxis("Horizontal");
            bool handbrake = Input.GetKey(KeyCode.Space);

            // Steer Angle
            float steerAngle = moveX * vehicleData.MaxSteerAngle;
            if (frontLeftWheel != null) frontLeftWheel.steerAngle = steerAngle;
            if (frontRightWheel != null) frontRightWheel.steerAngle = steerAngle;

            // Acceleration & Braking
            float motor = moveY * vehicleData.MotorTorque;
            if (rearLeftWheel != null) rearLeftWheel.motorTorque = motor;
            if (rearRightWheel != null) rearRightWheel.motorTorque = motor;

            if (handbrake)
            {
                ApplyBrakes();
            }
            else
            {
                ReleaseBrakes();
            }

            // Burn Fuel when driving
            if (Mathf.Abs(moveY) > 0.05f)
            {
                currentFuel = Mathf.Max(0f, currentFuel - vehicleData.FuelBurnRatePerSec * Time.fixedDeltaTime);
                OnFuelChanged?.Invoke(currentFuel, vehicleData.MaxFuel);
            }
        }

        private void ApplyBrakes()
        {
            float brake = (vehicleData != null) ? vehicleData.BrakeTorque : 2000f;
            if (frontLeftWheel != null) frontLeftWheel.brakeTorque = brake;
            if (frontRightWheel != null) frontRightWheel.brakeTorque = brake;
            if (rearLeftWheel != null) rearLeftWheel.brakeTorque = brake;
            if (rearRightWheel != null) rearRightWheel.brakeTorque = brake;
        }

        private void ReleaseBrakes()
        {
            if (frontLeftWheel != null) frontLeftWheel.brakeTorque = 0f;
            if (frontRightWheel != null) frontRightWheel.brakeTorque = 0f;
            if (rearLeftWheel != null) rearLeftWheel.brakeTorque = 0f;
            if (rearRightWheel != null) rearRightWheel.brakeTorque = 0f;
        }

        public bool CanInteract(GameObject interactor)
        {
            return currentState != VehicleState.Destroyed;
        }

        public void Interact(GameObject interactor)
        {
            if (TryEnterVehicle(interactor, out _))
            {
                // Entered vehicle seat successfully
            }
        }

        public void OnFocusEnter() { }
        public void OnFocusExit() { }

        public bool TryEnterVehicle(GameObject player, out SeatType assignedSeat)
        {
            assignedSeat = SeatType.Driver;
            if (currentState == VehicleState.Destroyed) return false;

            foreach (var seat in seats)
            {
                if (!seat.IsOccupied)
                {
                    seat.AssignOccupant(player);
                    assignedSeat = seat.SeatType;
                    currentState = VehicleState.Occupied;

                    // Disable player CharacterController while seated
                    CharacterController cc = player.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = false;

                    return true;
                }
            }

            return false;
        }

        public bool TryExitVehicle(GameObject player, out Vector3 exitPosition)
        {
            exitPosition = transform.position + transform.right * 2.0f;

            foreach (var seat in seats)
            {
                if (seat.OccupantPlayer == player)
                {
                    exitPosition = seat.ExitPoint.position;
                    seat.ClearOccupant();

                    CharacterController cc = player.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = true;
                    player.transform.position = exitPosition;

                    UpdateOccupancyState();
                    return true;
                }
            }

            return false;
        }

        public void Refuel(float amount)
        {
            if (vehicleData == null) return;
            currentFuel = Mathf.Min(vehicleData.MaxFuel, currentFuel + amount);
            OnFuelChanged?.Invoke(currentFuel, vehicleData.MaxFuel);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (currentState == VehicleState.Destroyed) return;

            currentHealth -= damageInfo.RawDamage;
            currentHealth = Mathf.Max(0f, currentHealth);
            OnHealthChanged?.Invoke(currentHealth, vehicleData != null ? vehicleData.MaxHealth : 500f);

            if (currentHealth <= 0f)
            {
                ExplodeVehicle();
            }
        }

        private void ExplodeVehicle()
        {
            currentState = VehicleState.Destroyed;

            if (vehicleData != null && vehicleData.ExplosionVfxPrefab != null)
            {
                Instantiate(vehicleData.ExplosionVfxPrefab, transform.position, Quaternion.identity);
            }

            // Deal lethal explosion damage to all occupants
            foreach (var seat in seats)
            {
                if (seat.IsOccupied)
                {
                    PlayerHealth ph = seat.OccupantPlayer.GetComponent<PlayerHealth>();
                    if (ph != null)
                    {
                        ph.TakeDamage(new DamageInfo(500f, DamageType.Explosion, HitboxLocation.Chest, transform.position, Vector3.up, "VehicleExplosion"));
                    }
                    TryExitVehicle(seat.OccupantPlayer, out _);
                }
            }

            OnVehicleExploded?.Invoke();
        }

        private VehicleSeat GetSeat(SeatType type)
        {
            foreach (var seat in seats)
            {
                if (seat.SeatType == type) return seat;
            }
            return null;
        }

        private void UpdateOccupancyState()
        {
            bool anyOccupied = false;
            foreach (var seat in seats)
            {
                if (seat.IsOccupied)
                {
                    anyOccupied = true;
                    break;
                }
            }
            if (!anyOccupied) currentState = VehicleState.Empty;
        }
    }
}
