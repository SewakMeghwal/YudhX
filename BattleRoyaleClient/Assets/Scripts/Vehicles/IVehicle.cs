using System;
using UnityEngine;

namespace BattleRoyale.Vehicles
{
    public interface IVehicle
    {
        VehicleData Data { get; }
        VehicleState State { get; }
        float CurrentSpeedKmH { get; }
        float CurrentHealth { get; }
        float CurrentFuel { get; }

        bool TryEnterVehicle(GameObject player, out SeatType assignedSeat);
        bool TryExitVehicle(GameObject player, out Vector3 exitPosition);
        void Refuel(float amount);

        event Action<float, float> OnHealthChanged; // Current, Max
        event Action<float, float> OnFuelChanged;   // Current, Max
        event Action OnVehicleExploded;
    }
}
