namespace BattleRoyale.Player.Animation
{
    public interface IPlayerAnimation
    {
        void SetLocomotion(float speed, float moveX, float moveY, float verticalVelocity);
        void SetStates(bool isGrounded, bool isCrouching, bool isSprinting, bool isAiming);
        void TriggerJump();
        void TriggerLand();
        void TriggerFire();
        void TriggerReload();
        void SetDownedState(bool isDowned);
        void SetDeadState(bool isDead);
    }
}
