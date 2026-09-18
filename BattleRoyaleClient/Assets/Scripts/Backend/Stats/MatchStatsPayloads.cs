using System;
using System.Collections.Generic;

namespace BattleRoyale.Backend.Stats
{
    [Serializable]
    public struct MatchSubmitRequest
    {
        public string match_id;
        public string map_id;
        public int total_players;
        public float duration_seconds;
        public int placement_rank;
        public int kills;
        public float damage_dealt;
        public int xp_earned;

        public MatchSubmitRequest(string mId, string map, int totalP, float duration, int rank, int k, float dmg, int xp)
        {
            match_id = mId;
            map_id = map;
            total_players = totalP;
            duration_seconds = duration;
            placement_rank = rank;
            kills = k;
            damage_dealt = dmg;
            xp_earned = xp;
        }
    }

    [Serializable]
    public struct MatchHistoryEntry
    {
        public int id;
        public string username;
        public int placement_rank;
        public int kills;
        public float damage_dealt;
        public int xp_earned;
    }

    [Serializable]
    public struct LeaderboardListWrapper
    {
        public PlayerProfileResponse[] results;
    }

    [Serializable]
    public struct MatchHistoryListWrapper
    {
        public MatchHistoryEntry[] results;
    }
}
