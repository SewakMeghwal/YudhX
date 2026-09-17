namespace BattleRoyale.Match
{
    public enum MatchState
    {
        MainMenu,
        Matchmaking,
        LobbyWaiting,
        StartingCountdown,
        AirplaneDrop,
        InCombat,
        MatchEnded
    }

    public enum MatchEndReason
    {
        LastPlayerStanding,
        TimeLimitReached,
        ServerShutdown
    }
}
