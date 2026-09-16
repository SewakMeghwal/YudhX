using System;

namespace BattleRoyale.Multiplayer
{
    public interface INetworkManager
    {
        bool IsServer { get; }
        bool IsClient { get; }
        bool IsHost { get; }
        int ConnectedClientCount { get; }

        void StartHost();
        void StartServer();
        void StartClient(string ipAddress, ushort port);
        void Disconnect();

        event Action OnServerStartedEvent;
        event Action OnClientConnectedEvent;
        event Action OnClientDisconnectedEvent;
    }
}
