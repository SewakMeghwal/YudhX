using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Zone
{
    [Serializable]
    public struct ZonePhaseData
    {
        public int PhaseNumber;
        public float DelaySeconds;
        public float ShrinkDurationSeconds;
        public float TargetRadiusMeters;
        public float DamagePerSecond;
    }

    [CreateAssetMenu(fileName = "ZoneSettings", menuName = "BattleRoyale/Zone/Zone Settings")]
    public class ZoneSettings : ScriptableObject
    {
        [Header("Initial Map Boundary")]
        [SerializeField] private float initialRadiusMeters = 250.0f;

        [Header("Zone Phases Config")]
        [SerializeField] private List<ZonePhaseData> phases = new List<ZonePhaseData>()
        {
            new ZonePhaseData { PhaseNumber = 1, DelaySeconds = 120f, ShrinkDurationSeconds = 60f, TargetRadiusMeters = 150f, DamagePerSecond = 1.0f },
            new ZonePhaseData { PhaseNumber = 2, DelaySeconds = 90f, ShrinkDurationSeconds = 45f, TargetRadiusMeters = 80f, DamagePerSecond = 2.5f },
            new ZonePhaseData { PhaseNumber = 3, DelaySeconds = 75f, ShrinkDurationSeconds = 30f, TargetRadiusMeters = 30f, DamagePerSecond = 5.0f },
            new ZonePhaseData { PhaseNumber = 4, DelaySeconds = 60f, ShrinkDurationSeconds = 20f, TargetRadiusMeters = 0f, DamagePerSecond = 10.0f }
        };

        public float InitialRadiusMeters => initialRadiusMeters;
        public IReadOnlyList<ZonePhaseData> Phases => phases;
    }
}
