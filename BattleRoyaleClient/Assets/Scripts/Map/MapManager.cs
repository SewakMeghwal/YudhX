using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private MapConfig mapConfig;
        [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        [SerializeField] private List<PoiLocation> poiLocations = new List<PoiLocation>();

        public MapConfig Config => mapConfig;

        private void Awake()
        {
            if (spawnPoints.Count == 0)
            {
                spawnPoints.AddRange(GetComponentsInChildren<SpawnPoint>());
            }
            if (poiLocations.Count == 0)
            {
                poiLocations.AddRange(GetComponentsInChildren<PoiLocation>());
            }
        }

        public bool GetNextAvailableSpawn(out Vector3 spawnPosition, out Quaternion spawnRotation)
        {
            spawnPosition = Vector3.zero;
            spawnRotation = Quaternion.identity;

            foreach (var sp in spawnPoints)
            {
                if (sp != null && !sp.IsOccupied)
                {
                    sp.SetOccupied(true);
                    spawnPosition = sp.transform.position;
                    spawnRotation = sp.transform.rotation;
                    return true;
                }
            }

            // Fallback random spawn within boundary
            if (mapConfig != null)
            {
                float halfSize = mapConfig.MapSizeMeters * 0.4f;
                spawnPosition = mapConfig.MapCenter + new Vector3(UnityEngine.Random.Range(-halfSize, halfSize), 1.0f, UnityEngine.Random.Range(-halfSize, halfSize));
                return true;
            }

            return false;
        }

        public string GetCurrentPOIName(Vector3 playerPosition)
        {
            foreach (var poi in poiLocations)
            {
                if (poi != null && poi.IsInsidePOI(playerPosition))
                {
                    return poi.PoiName;
                }
            }
            return "Wilderness";
        }

        public bool IsWithinMapBounds(Vector3 position)
        {
            if (mapConfig == null) return true;
            return mapConfig.WorldBounds.Contains(position) && position.y >= mapConfig.OutOfBoundsKillHeight;
        }

        public void ResetAllSpawns()
        {
            foreach (var sp in spawnPoints)
            {
                if (sp != null) sp.SetOccupied(false);
            }
        }
    }
}
