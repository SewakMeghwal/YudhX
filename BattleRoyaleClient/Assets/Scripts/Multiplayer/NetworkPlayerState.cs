using System;
using UnityEngine;

namespace BattleRoyale.Multiplayer
{
    [Serializable]
    public struct NetworkPlayerState
    {
        public ulong NetworkId;
        public Vector3 Position;
        public float YawRotation;
        public float PitchRotation;
        public float HorizontalSpeed;
        public bool IsGrounded;
        public bool IsCrouching;
        public float Timestamp;

        public NetworkPlayerState(ulong id, Vector3 pos, float yaw, float pitch, float speed, bool grounded, bool crouching, float time)
        {
            NetworkId = id;
            Position = pos;
            YawRotation = yaw;
            PitchRotation = pitch;
            HorizontalSpeed = speed;
            IsGrounded = grounded;
            IsCrouching = crouching;
            Timestamp = time;
        }
    }
}
