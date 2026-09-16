using System;
using UnityEngine;

namespace BattleRoyale.Multiplayer.Combat
{
    public interface INetworkedCombat
    {
        void RequestFire(Vector3 origin, Vector3 direction, bool isADS);
        void RequestReload();

        event Action<NetworkShotData> OnShotFiredBroadcast;
        event Action<NetworkHitData> OnHitConfirmedBroadcast;
    }
}
