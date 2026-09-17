using System;

namespace BattleRoyale.Match
{
    [Serializable]
    public struct MatchPlayerInfo
    {
        public ulong NetworkId;
        public string DisplayName;
        public int TeamId;
        public int Kills;
        public float DamageDealt;
        public bool IsAlive;
        public int PlacementRank;

        public MatchPlayerInfo(ulong networkId, string name, int teamId = 0)
        {
            NetworkId = networkId;
            DisplayName = name;
            TeamId = teamId;
            Kills = 0;
            DamageDealt = 0f;
            IsAlive = true;
            PlacementRank = 0;
        }
    }

    [Serializable]
    public struct KillFeedEntry
    {
        public string AttackerName;
        public string VictimName;
        public string WeaponName;
        public bool IsHeadshot;

        public KillFeedEntry(string attacker, string victim, string weapon, bool headshot)
        {
            AttackerName = attacker;
            VictimName = victim;
            WeaponName = weapon;
            IsHeadshot = headshot;
        }
    }
}
