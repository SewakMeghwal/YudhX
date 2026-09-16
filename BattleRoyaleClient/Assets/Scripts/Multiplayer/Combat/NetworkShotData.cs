using System;
using UnityEngine;

namespace BattleRoyale.Multiplayer.Combat
{
    [Serializable]
    public struct NetworkShotData
    {
        public ulong ShooterId;
        public Vector3 Origin;
        public Vector3 Direction;
        public string WeaponId;
        public bool IsADS;
        public float Timestamp;

        public NetworkShotData(ulong shooterId, Vector3 origin, Vector3 direction, string weaponId, bool isAds, float timestamp)
        {
            ShooterId = shooterId;
            Origin = origin;
            Direction = direction;
            WeaponId = weaponId;
            IsADS = isAds;
            Timestamp = timestamp;
        }
    }

    [Serializable]
    public struct NetworkHitData
    {
        public ulong ShooterId;
        public ulong VictimId;
        public float AppliedDamage;
        public bool IsHeadshot;
        public Vector3 HitPoint;
        public bool IsFatal;

        public NetworkHitData(ulong shooterId, ulong victimId, float damage, bool headshot, Vector3 hitPoint, bool fatal)
        {
            ShooterId = shooterId;
            VictimId = victimId;
            AppliedDamage = damage;
            IsHeadshot = headshot;
            HitPoint = hitPoint;
            IsFatal = fatal;
        }
    }
}
