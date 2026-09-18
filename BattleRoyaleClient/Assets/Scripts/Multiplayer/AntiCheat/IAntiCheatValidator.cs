using System;
using UnityEngine;

namespace BattleRoyale.Multiplayer.AntiCheat
{
    public interface IAntiCheatValidator
    {
        bool ValidateMovement(ulong playerId, Vector3 currentPos, Vector3 lastPos, float deltaTime);
        bool ValidateFiringCadence(ulong playerId, float minInterval, float lastFireTime);
        bool ValidateLineOfSight(Vector3 shooterPos, Vector3 victimPos, LayerMask obstacleMask);

        event Action<ulong, ViolationType, string> OnViolationDetected;
        event Action<ulong, string> OnPlayerKicked;
    }
}
