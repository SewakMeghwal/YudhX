using System;
using UnityEngine;
using BattleRoyale.Map;

namespace BattleRoyale.Match.Drop
{
    public class AirplaneFlightPath : MonoBehaviour
    {
        [SerializeField] private ParachuteConfig config;
        [SerializeField] private MapConfig mapConfig;
        [SerializeField] private Transform airplaneMeshTransform;

        private Vector3 startPoint;
        private Vector3 endPoint;
        private float flightProgress;
        private bool isFlying;

        public Vector3 CurrentAirplanePosition => airplaneMeshTransform != null ? airplaneMeshTransform.position : transform.position;
        public Quaternion CurrentAirplaneRotation => airplaneMeshTransform != null ? airplaneMeshTransform.rotation : transform.rotation;
        public bool IsFlying => isFlying;

        public event Action OnFlightStarted;
        public event Action OnFlightCompleted;

        public void GenerateAndStartFlightPath()
        {
            float mapRadius = (mapConfig != null) ? mapConfig.MapSizeMeters * 0.5f : 250f;
            float altitude = (config != null) ? config.FlightAltitudeMeters : 250f;

            // Generate random entry and exit points on opposite sides of map perimeter
            float randomAngle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), 0f, Mathf.Sin(randomAngle)) * (mapRadius + 50f);

            startPoint = (mapConfig != null ? mapConfig.MapCenter : Vector3.zero) - offset + Vector3.up * altitude;
            endPoint = (mapConfig != null ? mapConfig.MapCenter : Vector3.zero) + offset + Vector3.up * altitude;

            flightProgress = 0f;
            isFlying = true;

            if (airplaneMeshTransform != null)
            {
                airplaneMeshTransform.position = startPoint;
                airplaneMeshTransform.rotation = Quaternion.LookRotation(endPoint - startPoint);
            }

            OnFlightStarted?.Invoke();
        }

        private void Update()
        {
            if (!isFlying) return;

            float distance = Vector3.Distance(startPoint, endPoint);
            float speed = (config != null) ? config.FlightSpeedMetersPerSec : 45f;
            float travelDuration = distance / speed;

            flightProgress += Time.deltaTime / Mathf.Max(0.1f, travelDuration);

            Vector3 currentPos = Vector3.Lerp(startPoint, endPoint, flightProgress);
            if (airplaneMeshTransform != null)
            {
                airplaneMeshTransform.position = currentPos;
            }

            if (flightProgress >= 1.0f)
            {
                isFlying = false;
                OnFlightCompleted?.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
            if (!isFlying) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint, endPoint);
            Gizmos.DrawWireSphere(startPoint, 5f);
            Gizmos.DrawWireSphere(endPoint, 5f);
        }
    }
}
