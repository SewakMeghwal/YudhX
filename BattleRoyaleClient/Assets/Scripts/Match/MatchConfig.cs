using UnityEngine;

namespace BattleRoyale.Match
{
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "BattleRoyale/Match/Match Config")]
    public class MatchConfig : ScriptableObject
    {
        [Header("Capacity & Readiness")]
        [SerializeField] private int minRequiredPlayers = 2;
        [SerializeField] private int maxAllowedPlayers = 8;
        [SerializeField] private float lobbyCountdownSeconds = 10.0f;

        [Header("Match Duration")]
        [SerializeField] private float maxMatchDurationSeconds = 1200.0f; // 20 minutes max

        [Header("XP Rewards")]
        [SerializeField] private int xpPerKill = 100;
        [SerializeField] private int xpPerWin = 500;
        [SerializeField] private int xpPerMinuteSurv = 10;

        public int MinRequiredPlayers => minRequiredPlayers;
        public int MaxAllowedPlayers => maxAllowedPlayers;
        public float LobbyCountdownSeconds => lobbyCountdownSeconds;
        public float MaxMatchDurationSeconds => maxMatchDurationSeconds;
        public int XpPerKill => xpPerKill;
        public int XpPerWin => xpPerWin;
        public int XpPerMinuteSurv => xpPerMinuteSurv;
    }
}
