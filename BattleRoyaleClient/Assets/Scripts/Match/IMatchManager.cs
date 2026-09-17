using System;
using System.Collections.Generic;

namespace BattleRoyale.Match
{
    public interface IMatchManager
    {
        MatchState CurrentState { get; }
        int AlivePlayerCount { get; }
        int TotalPlayerCount { get; }
        float MatchTimerSeconds { get; }

        void RegisterPlayer(ulong networkId, string displayName);
        void UnregisterPlayer(ulong networkId);
        void ReportPlayerElimination(ulong victimId, ulong attackerId, string weaponName, bool isHeadshot);

        event Action<MatchState> OnMatchStateChanged;
        event Action<int, float> OnCountdownTick; // Players, time remaining
        event Action<KillFeedEntry> OnKillFeedEvent;
        event Action<MatchPlayerInfo> OnWinnerDeclared;
    }
}
