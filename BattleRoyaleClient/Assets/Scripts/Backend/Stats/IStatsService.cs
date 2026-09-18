using System;
using System.Collections.Generic;

namespace BattleRoyale.Backend.Stats
{
    public interface IStatsService
    {
        void SubmitMatchResults(MatchSubmitRequest matchData, Action<bool, string> callback);
        void FetchUserMatchHistory(Action<bool, List<MatchHistoryEntry>, string> callback);
        void FetchLeaderboardWins(Action<bool, List<PlayerProfileResponse>, string> callback);
        void FetchLeaderboardKills(Action<bool, List<PlayerProfileResponse>, string> callback);

        event Action OnMatchSubmitted;
        event Action OnLeaderboardsUpdated;
    }
}
