using System;
using System.Collections.Generic;
using UnityEngine;
using BattleRoyale.Map;

namespace BattleRoyale.Multiplayer
{
    public class CustomNetworkManager : MonoBehaviour, INetworkManager
    {
        [Header("Configuration")]
        [SerializeField] private NetworkConfig config;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private MapManager mapManager;

        private bool isServer;
        private bool isClient;
        private bool isHost;
        private ulong nextNetworkId = 1;

        private readonly Dictionary<ulong, NetworkPlayer> connectedPlayers = new Dictionary<ulong, NetworkPlayer>();
        private float tickTimer;

        public bool IsServer => isServer;
        public bool IsClient => isClient;
        public bool IsHost => isHost;
        public int ConnectedClientCount => connectedPlayers.Count;

        public event Action OnServerStartedEvent;
        public event Action OnClientConnectedEvent;
        public event Action OnClientDisconnectedEvent;

        private void Awake()
        {
            if (mapManager == null) mapManager = FindObjectOfType<MapManager>();
        }

        public void StartHost()
        {
            isServer = true;
            isClient = true;
            isHost = true;
            OnServerStartedEvent?.Invoke();

            // Spawn local host player
            SpawnNetworkPlayer(true);
        }

        public void StartServer()
        {
            isServer = true;
            isClient = false;
            isHost = false;
            OnServerStartedEvent?.Invoke();
        }

        public void StartClient(string ipAddress, ushort port)
        {
            isServer = false;
            isClient = true;
            isHost = false;
            OnClientConnectedEvent?.Invoke();

            // Spawn local client player
            SpawnNetworkPlayer(true);
        }

        public void Disconnect()
        {
            foreach (var kvp in connectedPlayers)
            {
                if (kvp.Value != null) Destroy(kvp.Value.gameObject);
            }
            connectedPlayers.Clear();

            isServer = false;
            isClient = false;
            isHost = false;
            OnClientDisconnectedEvent?.Invoke();
        }

        private NetworkPlayer SpawnNetworkPlayer(bool isLocalOwner)
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            if (mapManager != null)
            {
                mapManager.GetNextAvailableSpawn(out spawnPos, out spawnRot);
            }

            GameObject playerObj = (playerPrefab != null)
                ? Instantiate(playerPrefab, spawnPos, spawnRot)
                : new GameObject($"NetworkPlayer_{nextNetworkId}");

            playerObj.transform.position = spawnPos;
            playerObj.transform.rotation = spawnRot;

            NetworkPlayer netPlayer = playerObj.GetComponent<NetworkPlayer>();
            if (netPlayer == null)
            {
                netPlayer = playerObj.AddComponent<NetworkPlayer>();
            }

            ulong assignedId = nextNetworkId++;
            netPlayer.InitializeNetworkOwner(assignedId, isLocalOwner, config);
            connectedPlayers.Add(assignedId, netPlayer);

            return netPlayer;
        }

        private void Update()
        {
            if (!isServer) return;

            tickTimer += Time.deltaTime;
            float interval = (config != null) ? config.TickIntervalSeconds : (1.0f / 30.0f);

            if (tickTimer >= interval)
            {
                tickTimer = 0f;
                BroadcastServerStateTicks();
            }
        }

        private void BroadcastServerStateTicks()
        {
            foreach (var p1 in connectedPlayers.Values)
            {
                if (p1 == null) continue;
                NetworkPlayerState state = p1.GetCurrentState();

                foreach (var p2 in connectedPlayers.Values)
                {
                    if (p2 != null && !p2.IsLocalPlayer)
                    {
                        p2.ReceiveServerStateUpdate(state);
                    }
                }
            }
        }
    }
}
