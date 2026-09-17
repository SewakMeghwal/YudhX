using UnityEngine;

namespace BattleRoyale.Match.Drop
{
    [CreateAssetMenu(fileName = "ParachuteConfig", menuName = "BattleRoyale/Match/Parachute Config")]
    public class ParachuteConfig : ScriptableObject
    {
        [Header("Airplane Flight Path")]
        [SerializeField] private float flightAltitudeMeters = 250.0f;
        [SerializeField] private float flightSpeedMetersPerSec = 45.0f;

        [Header("Freefall Mechanics")]
        [SerializeField] private float freefallForwardSpeed = 25.0f;
        [SerializeField] private float minFreefallFallSpeed = 35.0f;
        [SerializeField] private float maxFreefallDiveSpeed = 65.0f;

        [Header("Parachute Mechanics")]
        [SerializeField] private float parachuteGlideSpeed = 14.0f;
        [SerializeField] private float parachuteDescentRate = 4.5f;
        [SerializeField] private float autoDeployAltitudeMeters = 60.0f;

        public float FlightAltitudeMeters => flightAltitudeMeters;
        public float FlightSpeedMetersPerSec => flightSpeedMetersPerSec;
        public float FreefallForwardSpeed => freefallForwardSpeed;
        public float MinFreefallFallSpeed => minFreefallFallSpeed;
        public float MaxFreefallDiveSpeed => maxFreefallDiveSpeed;
        public float ParachuteGlideSpeed => parachuteGlideSpeed;
        public float ParachuteDescentRate => parachuteDescentRate;
        public float AutoDeployAltitudeMeters => autoDeployAltitudeMeters;
    }
}
