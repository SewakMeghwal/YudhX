using UnityEngine;

namespace BattleRoyale.Player.Animation
{
    public class PlayerAnimatorBridge : MonoBehaviour, IPlayerAnimation
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float dampTime = 0.1f;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        public void SetLocomotion(float speed, float moveX, float moveY, float verticalVelocity)
        {
            if (animator == null) return;

            animator.SetFloat(AnimationParameters.Speed, speed, dampTime, Time.deltaTime);
            animator.SetFloat(AnimationParameters.MoveX, moveX, dampTime, Time.deltaTime);
            animator.SetFloat(AnimationParameters.MoveY, moveY, dampTime, Time.deltaTime);
            animator.SetFloat(AnimationParameters.VerticalVelocity, verticalVelocity);
        }

        public void SetStates(bool isGrounded, bool isCrouching, bool isSprinting, bool isAiming)
        {
            if (animator == null) return;

            animator.SetBool(AnimationParameters.IsGrounded, isGrounded);
            animator.SetBool(AnimationParameters.IsCrouching, isCrouching);
            animator.SetBool(AnimationParameters.IsSprinting, isSprinting);
            animator.SetBool(AnimationParameters.IsAiming, isAiming);
        }

        public void TriggerJump()
        {
            if (animator == null) return;
            animator.SetTrigger(AnimationParameters.JumpTrigger);
        }

        public void TriggerLand()
        {
            if (animator == null) return;
            animator.SetTrigger(AnimationParameters.LandTrigger);
        }

        public void TriggerFire()
        {
            if (animator == null) return;
            animator.SetTrigger(AnimationParameters.FireTrigger);
        }

        public void TriggerReload()
        {
            if (animator == null) return;
            animator.SetTrigger(AnimationParameters.ReloadTrigger);
        }

        public void SetDownedState(bool isDowned)
        {
            if (animator == null) return;
            animator.SetBool(AnimationParameters.IsDowned, isDowned);
        }

        public void SetDeadState(bool isDead)
        {
            if (animator == null) return;
            animator.SetBool(AnimationParameters.IsDead, isDead);
        }
    }
}
