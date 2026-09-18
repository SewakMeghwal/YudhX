using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Multiplayer.AntiCheat
{
    public class ServerAntiCheatManager : MonoBehaviour, IAntiCheatValidator
    {
        [SerializeField] private AntiCheatSettings settings;

        private readonly Dictionary<ulong, int> violationCounts = new Dictionary<ulong, int>();

        public event Action<ulong, ViolationType, string> OnViolationDetected;
        public event Action<ulong, string> OnPlayerKicked;

        public bool ValidateMovement(ulong playerId, Vector3 currentPos, Vector3 lastPos, float deltaTime)
        {
            if (settings == null || deltaTime <= 0.001f) return true;

            float distance = Vector3.Distance(currentPos, lastPos);
            float calculatedSpeed = distance / deltaTime;

            // 1. Check Teleportation Jump
            if (distance > settings.MaxSingleFrameTeleportMeters)
            {
                RecordViolation(playerId, ViolationType.Teleportation, $"Teleported {distance:F1}m in single frame");
                return false;
            }

            // 2. Check Speed Hack
            if (calculatedSpeed > settings.MaxAllowedSpeedMetersPerSec)
            {
                RecordViolation(playerId, ViolationType.SpeedHack, $"Speed {calculatedSpeed:F1}m/s exceeded max limit {settings.MaxAllowedSpeedMetersPerSec}m/s");
                return false;
            }

            return true;
        }

        public bool ValidateFiringCadence(ulong playerId, float minInterval, float lastFireTime)
        {
            if (settings == null) return true;

            float timeSinceLastShot = Time.time - lastFireTime;
            float minAllowedTime = minInterval * settings.FireRateToleranceFactor;

            if (timeSinceLastShot < minAllowedTime)
            {
                RecordViolation(playerId, ViolationType.FireRateHack, $"Fire cadence {timeSinceLastShot:F3}s below min interval {minAllowedTime:F3}s");
                return false;
            }

            return true;
        }

        public bool ValidateLineOfSight(Vector3 shooterPos, Vector3 victimPos, LayerMask obstacleMask)
        {
            if (settings == null || !settings.EnforceLineOfSightCheck) return true;

            Vector3 direction = (victimPos - shooterPos).normalized;
            float distance = Vector3.Distance(shooterPos, victimPos);

            if (Physics.Raycast(shooterPos, direction, out RaycastHit hit, distance, obstacleMask))
            {
                // Obstructed by solid obstacle before reaching victim
                if (Vector3.Distance(hit.point, victimPos) > 0.5f)
                {
                    return false;
                }
            }

            return true;
        }

        private void RecordViolation(ulong playerId, ViolationType type, string details)
        {
            if (!violationCounts.ContainsKey(playerId))
            {
                violationCounts[playerId] = 0;
            }

            violationCounts[playerId]++;
            int currentViolations = violationCounts[playerId];

            Debug.LogWarning($"[Anti-Cheat Warning #{currentViolations}] Player {playerId}: {type} - {details}");
            OnViolationDetected?.Invoke(playerId, type, details);

            int maxAllowed = (settings != null) ? settings.MaxAllowedViolationsBeforeKick : 3;
            if (currentViolations >= maxAllowed)
            {
                KickPlayer(playerId, $"Kicked by Server Anti-Cheat ({type})");
            }
        }

        private void KickPlayer(ulong playerId, string reason)
        {
            Debug.LogError($"[Anti-Cheat KICK] Player {playerId} kicked: {reason}");
            OnPlayerKicked?.Invoke(playerId, reason);
            violationCounts.Remove(playerId);
        }
    }
}
