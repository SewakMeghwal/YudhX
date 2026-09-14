using UnityEngine;

namespace BattleRoyale.Player.Animation
{
    public static class AnimationParameters
    {
        // Locomotion Hashes
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int MoveX = Animator.StringToHash("MoveX");
        public static readonly int MoveY = Animator.StringToHash("MoveY");
        public static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");

        // Bools & Triggers
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int IsCrouching = Animator.StringToHash("IsCrouching");
        public static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
        public static readonly int IsAiming = Animator.StringToHash("IsAiming");
        public static readonly int IsDowned = Animator.StringToHash("IsDowned");
        public static readonly int IsDead = Animator.StringToHash("IsDead");

        // Action Triggers
        public static readonly int JumpTrigger = Animator.StringToHash("Jump");
        public static readonly int LandTrigger = Animator.StringToHash("Land");
        public static readonly int FireTrigger = Animator.StringToHash("Fire");
        public static readonly int ReloadTrigger = Animator.StringToHash("Reload");
    }
}
