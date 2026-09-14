using System;

namespace BattleRoyale.CameraSystem
{
    public enum CameraMode
    {
        DefaultThirdPerson,
        AimDownSights,
        Sprinting,
        FreeLook
    }

    public enum ShoulderSide
    {
        Right,
        Left
    }

    [Serializable]
    public struct CameraRuntimeState
    {
        public CameraMode Mode;
        public ShoulderSide Shoulder;
        public float Pitch;
        public float Yaw;
        public float CurrentFOV;
        public float CurrentDistance;
    }
}
