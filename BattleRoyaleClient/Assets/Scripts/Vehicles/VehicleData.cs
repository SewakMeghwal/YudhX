using UnityEngine;

namespace BattleRoyale.Vehicles
{
    [CreateAssetMenu(fileName = "VehicleData", menuName = "BattleRoyale/Vehicles/Vehicle Data")]
    public class VehicleData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string vehicleId = "vehicle_buggy_4x4";
        [SerializeField] private string vehicleName = "Offroad Buggy 4x4";

        [Header("Physics & Performance")]
        [SerializeField] private float motorTorque = 1500.0f;
        [SerializeField] private float brakeTorque = 3000.0f;
        [SerializeField] private float maxSteerAngle = 32.0f;
        [SerializeField] private float maxSpeedKmH = 90.0f;

        [Header("Health & Fuel")]
        [SerializeField] private float maxHealth = 500.0f;
        [SerializeField] private float maxFuel = 100.0f;
        [SerializeField] private float fuelBurnRatePerSec = 0.5f;

        [Header("Audio & VFX")]
        [SerializeField] private AudioClip engineSound;
        [SerializeField] private GameObject explosionVfxPrefab;

        public string VehicleId => vehicleId;
        public string VehicleName => vehicleName;
        public float MotorTorque => motorTorque;
        public float BrakeTorque => brakeTorque;
        public float MaxSteerAngle => maxSteerAngle;
        public float MaxSpeedKmH => maxSpeedKmH;
        public float MaxHealth => maxHealth;
        public float MaxFuel => maxFuel;
        public float FuelBurnRatePerSec => fuelBurnRatePerSec;
        public AudioClip EngineSound => engineSound;
        public GameObject ExplosionVfxPrefab => explosionVfxPrefab;
    }
}
