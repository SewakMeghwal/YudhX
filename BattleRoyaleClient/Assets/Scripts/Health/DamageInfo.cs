using System;
using UnityEngine;

namespace BattleRoyale.HealthSystem
{
    public enum DamageType
    {
        Bullet,
        Explosion,
        SafeZone,
        Fall,
        Melee
    }

    public enum HitboxLocation
    {
        Head,
        Chest,
        Stomach,
        Limb
    }

    [Serializable]
    public struct DamageInfo
    {
        public float RawDamage;
        public DamageType DamageType;
        public HitboxLocation HitLocation;
        public Vector3 HitPosition;
        public Vector3 HitDirection;
        public string AttackerId;
        public bool IsHeadshot => HitLocation == HitboxLocation.Head;

        public DamageInfo(float damage, DamageType type, HitboxLocation location, Vector3 position, Vector3 direction, string attacker = "")
        {
            RawDamage = damage;
            DamageType = type;
            HitLocation = location;
            HitPosition = position;
            HitDirection = direction;
            AttackerId = attacker;
        }
    }
}
