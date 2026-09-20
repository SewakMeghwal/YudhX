using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BattleRoyale.Backend.Stats
{
    public class StatsLeaderboardApiClient : MonoBehaviour, IStatsService
    {
        [SerializeField] private BackendConfig config;
        [SerializeField] private BackendApiClient authClient;

        public event Action OnMatchSubmitted;
        public event Action OnLeaderboardsUpdated;

        private void Awake()
        {
            if (authClient == null) authClient = GetComponent<BackendApiClient>();
        }

        public void SubmitMatchResults(MatchSubmitRequest matchData, Action<bool, string> callback)
        {
            if (config == null || authClient == null || !authClient.IsAuthenticated)
            {
                callback?.Invoke(false, "Authentication required.");
                return;
            }

            string json = JsonUtility.ToJson(matchData);
            StartCoroutine(SendAuthenticatedPost(config.MatchesUrl, json, (success, responseText) =>
            {
                if (success)
                {
                    OnMatchSubmitted?.Invoke();
                    callback?.Invoke(true, "Match stats recorded successfully.");
                }
                else
                {
                    callback?.Invoke(false, responseText);
                }
            }));
        }

        public void FetchUserMatchHistory(Action<bool, List<MatchHistoryEntry>, string> callback)
        {
            if (config == null || authClient == null || !authClient.IsAuthenticated)
            {
                callback?.Invoke(false, null, "Authentication required.");
                return;
            }

            StartCoroutine(SendAuthenticatedGet(config.MatchesUrl + "history/", (success, responseText) =>
            {
                if (success)
                {
                    try
                    {
                        string jsonWrapper = $"{{\"results\":{responseText}}}";
                        MatchHistoryListWrapper wrapper = JsonUtility.FromJson<MatchHistoryListWrapper>(jsonWrapper);
                        List<MatchHistoryEntry> list = new List<MatchHistoryEntry>(wrapper.results);
                        callback?.Invoke(true, list, "History loaded.");
                    }
                    catch (Exception ex)
                    {
                        callback?.Invoke(false, null, $"History parse error: {ex.Message}");
                    }
                }
                else
                {
                    callback?.Invoke(false, null, responseText);
                }
            }));
        }

        public void FetchLeaderboardWins(Action<bool, List<PlayerProfileResponse>, string> callback)
        {
            FetchLeaderboard(config != null ? config.LeaderboardWinsUrl : "", callback);
        }

        public void FetchLeaderboardKills(Action<bool, List<PlayerProfileResponse>, string> callback)
        {
            FetchLeaderboard(config != null ? config.LeaderboardKillsUrl : "", callback);
        }

        private void FetchLeaderboard(string url, Action<bool, List<PlayerProfileResponse>, string> callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.Invoke(false, null, "URL invalid.");
                return;
            }

            StartCoroutine(SendAuthenticatedGet(url, (success, responseText) =>
            {
                if (success)
                {
                    try
                    {
                        string jsonWrapper = $"{{\"results\":{responseText}}}";
                        LeaderboardListWrapper wrapper = JsonUtility.FromJson<LeaderboardListWrapper>(jsonWrapper);
                        List<PlayerProfileResponse> list = new List<PlayerProfileResponse>(wrapper.results);
                        OnLeaderboardsUpdated?.Invoke();
                        callback?.Invoke(true, list, "Leaderboard loaded.");
                    }
                    catch (Exception ex)
                    {
                        callback?.Invoke(false, null, $"Leaderboard parse error: {ex.Message}");
                    }
                }
                else
                {
                    callback?.Invoke(false, null, responseText);
                }
            }));
        }

        private IEnumerator SendAuthenticatedPost(string url, string jsonBody, Action<bool, string> onComplete)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                if (authClient != null && authClient.IsAuthenticated)
                {
                    request.SetRequestHeader("Authorization", $"Bearer {authClient.AccessToken}");
                }

                if (config != null) request.timeout = Mathf.RoundToInt(config.RequestTimeoutSeconds);

                yield return request.SendWebRequest();

                if (!request.isNetworkError && !request.isHttpError)
                {
                    onComplete?.Invoke(true, request.downloadHandler.text);
                }
                else
                {
                    onComplete?.Invoke(false, $"HTTP {request.responseCode}: {request.error}");
                }
            }
        }

        private IEnumerator SendAuthenticatedGet(string url, Action<bool, string> onComplete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if (authClient != null && authClient.IsAuthenticated)
                {
                    request.SetRequestHeader("Authorization", $"Bearer {authClient.AccessToken}");
                }

                if (config != null) request.timeout = Mathf.RoundToInt(config.RequestTimeoutSeconds);

                yield return request.SendWebRequest();

                if (!request.isNetworkError && !request.isHttpError)
                {
                    onComplete?.Invoke(true, request.downloadHandler.text);
                }
                else
                {
                    onComplete?.Invoke(false, $"HTTP {request.responseCode}: {request.error}");
                }
            }
        }
    }
}
