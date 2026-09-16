namespace BattleRoyale.AI
{
    public enum BotStateType
    {
        Patrol,
        Looting,
        Engaging,
        FleeingZone,
        Healing,
        Dead
    }

    public interface IBotState
    {
        BotStateType StateType { get; }
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
