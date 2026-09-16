using UnityEngine;

namespace BattleRoyale.Map
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "BattleRoyale/Map/Map Configuration")]
    public class MapConfig : ScriptableObject
    {
        [Header("Map Identity")]
        [SerializeField] private string mapId = "map_prototype_island";
        [SerializeField] private string displayName = "Alpha Outpost";

        [Header("World Boundaries (Meters)")]
        [SerializeField] private Vector3 mapCenter = Vector3.zero;
        [SerializeField] private float mapSizeMeters = 500.0f; // 500m x 500m prototype map
        [SerializeField] private float outOfBoundsKillHeight = -50.0f;

        [Header("Match Capacity")]
        [SerializeField] private int minPlayers = 4;
        [SerializeField] private int maxPlayers = 8;

        public string MapId => mapId;
        public string DisplayName => displayName;
        public Vector3 MapCenter => mapCenter;
        public float MapSizeMeters => mapSizeMeters;
        public float OutOfBoundsKillHeight => outOfBoundsKillHeight;
        public int MinPlayers => minPlayers;
        public int MaxPlayers => maxPlayers;

        public Bounds WorldBounds => new Bounds(mapCenter, new Vector3(mapSizeMeters, 200f, mapSizeMeters));
    }
}
