using UnityEngine;

namespace BattleRoyale.Backend
{
    [CreateAssetMenu(fileName = "BackendConfig", menuName = "BattleRoyale/Backend/Backend Config")]
    public class BackendConfig : ScriptableObject
    {
        [Header("REST API Endpoints")]
        [SerializeField] private string baseApiUrl = "http://127.0.0.1:8000/api";
        [SerializeField] private float requestTimeoutSeconds = 10.0f;

        public string BaseApiUrl => baseApiUrl;
        public float RequestTimeoutSeconds => requestTimeoutSeconds;

        public string RegisterUrl => $"{baseApiUrl}/auth/register/";
        public string LoginUrl => $"{baseApiUrl}/auth/login/";
        public string RefreshUrl => $"{baseApiUrl}/auth/token/refresh/";
        public string ProfileUrl => $"{baseApiUrl}/player/profile/";
        public string MatchesUrl => $"{baseApiUrl}/matches/";
        public string LeaderboardWinsUrl => $"{baseApiUrl}/leaderboard/wins/";
        public string LeaderboardKillsUrl => $"{baseApiUrl}/leaderboard/kills/";
    }
}
