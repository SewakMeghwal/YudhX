using System;
using System.Collections.Generic;
using UnityEngine;
using BattleRoyale.HealthSystem;

namespace BattleRoyale.Zone
{
    public class SafeZoneController : MonoBehaviour, IZoneController
    {
        [SerializeField] private ZoneSettings settings;
        [SerializeField] private SafeZoneWallVisual wallVisual;

        private ZoneState state = ZoneState.WaitingToShrink;
        private int currentPhaseIndex = 0;
        private float currentRadius;
        private float targetRadius;
        private Vector3 currentCenter;
        private Vector3 previousCenter;
        private Vector3 nextCenter;

        private float stateTimer;
        private float shrinkDuration;
        private float shrinkTimer;
        private float damageIntervalTimer;

        public ZoneState State => state;
        public int CurrentPhaseIndex => currentPhaseIndex;
        public float CurrentRadius => currentRadius;
        public Vector3 CurrentCenter => currentCenter;
        public Vector3 NextCenter => nextCenter;
        public float TimeRemainingSeconds => stateTimer;

        public event Action<int, float> OnPhaseTimerTick;
        public event Action<int, Vector3, float> OnZoneShrinkStarted;
        public event Action<int> OnZoneShrinkCompleted;

        private void Start()
        {
            if (settings != null)
            {
                InitializeZone();
            }
        }

        public void InitializeZone()
        {
            currentRadius = settings.InitialRadiusMeters;
            currentCenter = transform.position;
            currentPhaseIndex = 0;

            if (settings.Phases != null && settings.Phases.Count > 0)
            {
                PrepareNextPhase();
            }
        }

        private void PrepareNextPhase()
        {
            if (currentPhaseIndex >= settings.Phases.Count)
            {
                state = ZoneState.Finished;
                return;
            }

            ZonePhaseData phase = settings.Phases[currentPhaseIndex];
            state = ZoneState.WaitingToShrink;
            stateTimer = phase.DelaySeconds;
            targetRadius = phase.TargetRadiusMeters;
            previousCenter = currentCenter;

            // Generate next inner circle center inside current bounds
            nextCenter = CalculateNextCircleCenter(currentCenter, currentRadius, targetRadius);

            if (wallVisual != null)
            {
                wallVisual.UpdateZoneVisuals(currentCenter, currentRadius);
            }
        }

        private void Update()
        {
            if (settings == null || state == ZoneState.Finished) return;

            UpdateZoneStateMachine();
            ApplyOutsideZoneDamageTicks();
        }

        private void UpdateZoneStateMachine()
        {
            ZonePhaseData phase = settings.Phases[currentPhaseIndex];

            if (state == ZoneState.WaitingToShrink)
            {
                stateTimer -= Time.deltaTime;
                OnPhaseTimerTick?.Invoke(currentPhaseIndex + 1, Mathf.Max(0f, stateTimer));

                if (stateTimer <= 0f)
                {
                    StartShrinkingPhase(phase);
                }
            }
            else if (state == ZoneState.Shrinking)
            {
                shrinkTimer += Time.deltaTime;
                float progress = Mathf.Clamp01(shrinkTimer / shrinkDuration);

                // Smooth Lerp Radius and Center
                currentRadius = Mathf.Lerp(currentRadius, targetRadius, progress);
                currentCenter = Vector3.Lerp(previousCenter, nextCenter, progress);

                if (wallVisual != null)
                {
                    wallVisual.UpdateZoneVisuals(currentCenter, currentRadius);
                }

                if (progress >= 1.0f)
                {
                    CompleteShrinkingPhase();
                }
            }
        }

        private void StartShrinkingPhase(ZonePhaseData phase)
        {
            state = ZoneState.Shrinking;
            shrinkDuration = phase.ShrinkDurationSeconds;
            shrinkTimer = 0f;
            OnZoneShrinkStarted?.Invoke(currentPhaseIndex + 1, nextCenter, targetRadius);
        }

        private void CompleteShrinkingPhase()
        {
            OnZoneShrinkCompleted?.Invoke(currentPhaseIndex + 1);
            currentPhaseIndex++;
            PrepareNextPhase();
        }

        private void ApplyOutsideZoneDamageTicks()
        {
            damageIntervalTimer += Time.deltaTime;
            if (damageIntervalTimer < 1.0f) return; // 1-second damage tick cadence
            damageIntervalTimer = 0f;

            if (currentPhaseIndex >= settings.Phases.Count) return;
            float dps = settings.Phases[currentPhaseIndex].DamagePerSecond;

            // Find all players outside safe zone
            PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
            foreach (var player in players)
            {
                if (player != null && !player.IsDead && !IsInsideSafeZone(player.transform.position))
                {
                    DamageInfo zoneDamage = new DamageInfo(
                        dps,
                        DamageType.SafeZone,
                        HitboxLocation.Chest,
                        player.transform.position,
                        Vector3.up,
                        "SafeZone"
                    );
                    player.TakeDamage(zoneDamage);
                }
            }
        }

        public bool IsInsideSafeZone(Vector3 position)
        {
            Vector3 flatPos = new Vector3(position.x, 0, position.z);
            Vector3 flatCenter = new Vector3(currentCenter.x, 0, currentCenter.z);
            return Vector3.Distance(flatPos, flatCenter) <= currentRadius;
        }

        public Vector3 CalculateNextCircleCenter(Vector3 currentC, float currentR, float nextR)
        {
            float maxOffset = Mathf.Max(0f, currentR - nextR);
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * maxOffset;
            return currentC + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }
    }
}
