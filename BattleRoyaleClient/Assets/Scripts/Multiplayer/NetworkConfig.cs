using UnityEngine;

namespace BattleRoyale.Multiplayer
{
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "BattleRoyale/Multiplayer/Network Config")]
    public class NetworkConfig : ScriptableObject
    {
        [Header("Connection Settings")]
        [SerializeField] private string serverIp = "127.0.0.1";
        [SerializeField] private ushort serverPort = 7777;
        [SerializeField] private ushort maxConnections = 8;

        [Header("Tick Rate & Buffer")]
        [SerializeField] private int sendTickRate = 30; // 30 updates per second
        [SerializeField] private float interpolationBufferTime = 0.1f; // 100ms smooth lerp buffer

        [Header("Authority & Validation")]
        [SerializeField] private float maxAllowedSpeedThreshold = 12.0f; // Speed cheat check (m/s)

        public string ServerIp => serverIp;
        public ushort ServerPort => serverPort;
        public ushort MaxConnections => maxConnections;
        public int SendTickRate => sendTickRate;
        public float InterpolationBufferTime => interpolationBufferTime;
        public float MaxAllowedSpeedThreshold => maxAllowedSpeedThreshold;
        public float TickIntervalSeconds => 1.0f / Mathf.Max(1, sendTickRate);
    }
}
