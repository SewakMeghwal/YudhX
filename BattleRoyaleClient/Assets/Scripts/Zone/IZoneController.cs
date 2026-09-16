using System;
using UnityEngine;

namespace BattleRoyale.Zone
{
    public enum ZoneState
    {
        WaitingToShrink,
        Shrinking,
        Finished
    }

    public interface IZoneController
    {
        ZoneState State { get; }
        int CurrentPhaseIndex { get; }
        float CurrentRadius { get; }
        Vector3 CurrentCenter { get; }
        Vector3 NextCenter { get; }
        float TimeRemainingSeconds { get; }

        bool IsInsideSafeZone(Vector3 position);

        event Action<int, float> OnPhaseTimerTick;             // Phase, seconds left
        event Action<int, Vector3, float> OnZoneShrinkStarted; // Phase, new target center, new target radius
        event Action<int> OnZoneShrinkCompleted;
    }
}
