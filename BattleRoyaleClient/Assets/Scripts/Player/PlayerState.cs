using System;

namespace BattleRoyale.Player
{
    public enum MovementState
    {
        Idle,
        Walking,
        Running,
        Sprinting,
        Airborne,
        Crouching
    }

    public enum StanceState
    {
        Standing,
        Crouching,
        Prone
    }

    [Serializable]
    public struct PlayerRuntimeState
    {
        public MovementState MovementState;
        public StanceState StanceState;
        public bool IsGrounded;
        public bool IsSprinting;
        public bool IsCrouching;
        public float CurrentSpeed;
        public float VerticalVelocity;

        public override string ToString()
        {
            return $"State: {MovementState} | Stance: {StanceState} | Grounded: {IsGrounded} | Speed: {CurrentSpeed:F1} m/s";
        }
    }
}
