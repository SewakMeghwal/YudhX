using UnityEngine;

namespace BattleRoyale.Player
{
    public class PlayerAnimationHooks : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        // Cached Animator Parameter Hashes (Zero Allocation)
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
        private static readonly int JumpTriggerHash = Animator.StringToHash("Jump");

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        public void UpdateLocomotionParameters(float speed, float verticalVelocity, bool isGrounded, bool isCrouching)
        {
            if (animator == null) return;

            animator.SetFloat(SpeedHash, speed);
            animator.SetFloat(VerticalVelocityHash, verticalVelocity);
            animator.SetBool(IsGroundedHash, isGrounded);
            animator.SetBool(IsCrouchingHash, isCrouching);
        }

        public void TriggerJumpAnimation()
        {
            if (animator == null) return;
            animator.SetTrigger(JumpTriggerHash);
        }
    }
}
