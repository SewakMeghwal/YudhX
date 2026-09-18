using UnityEngine;

namespace BattleRoyale.Multiplayer.Server
{
    public class HeadlessServerBootstrap : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager networkManager;

        private void Awake()
        {
            if (networkManager == null) networkManager = GetComponent<CustomNetworkManager>();
        }

        private void Start()
        {
            ServerCommandLineArgs cliArgs = ServerCommandLineArgs.ParseFromSystem();

            if (Application.isBatchMode || cliArgs.IsBatchMode)
            {
                Debug.Log($"[Dedicated Server] Bootstrapping Headless Server on {cliArgs.ListenIp}:{cliArgs.Port} (Max Players: {cliArgs.MaxPlayers})");
                if (networkManager != null)
                {
                    networkManager.StartServer();
                }
            }
        }
    }
}
