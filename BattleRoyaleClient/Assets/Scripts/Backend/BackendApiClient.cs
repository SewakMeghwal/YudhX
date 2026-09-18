using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BattleRoyale.Backend
{
    public class BackendApiClient : MonoBehaviour, IAuthService
    {
        [SerializeField] private BackendConfig config;

        private string accessToken = string.Empty;
        private string refreshToken = string.Empty;
        private PlayerProfileResponse cachedProfile;

        public bool IsAuthenticated => !string.IsNullOrEmpty(accessToken);
        public string AccessToken => accessToken;
        public PlayerProfileResponse CachedProfile => cachedProfile;

        public event Action OnLoginSuccess;
        public event Action OnLogout;

        public void Register(string username, string email, string password, Action<bool, string> callback)
        {
            if (config == null)
            {
                callback?.Invoke(false, "BackendConfig missing.");
                return;
            }

            RegisterRequest payload = new RegisterRequest(username, email, password);
            string json = JsonUtility.ToJson(payload);
            StartCoroutine(SendPostRequest(config.RegisterUrl, json, false, (success, responseText) =>
            {
                if (success)
                {
                    callback?.Invoke(true, "Registration successful. Please log in.");
                }
                else
                {
                    callback?.Invoke(false, responseText);
                }
            }));
        }

        public void Login(string username, string password, Action<bool, string> callback)
        {
            if (config == null)
            {
                callback?.Invoke(false, "BackendConfig missing.");
                return;
            }

            LoginRequest payload = new LoginRequest(username, password);
            string json = JsonUtility.ToJson(payload);
            StartCoroutine(SendPostRequest(config.LoginUrl, json, false, (success, responseText) =>
            {
                if (success)
                {
                    try
                    {
                        JwtTokenResponse tokenData = JsonUtility.FromJson<JwtTokenResponse>(responseText);
                        accessToken = tokenData.access;
                        refreshToken = tokenData.refresh;

                        OnLoginSuccess?.Invoke();
                        callback?.Invoke(true, "Login successful.");
                    }
                    catch (Exception ex)
                    {
                        callback?.Invoke(false, $"Token parse error: {ex.Message}");
                    }
                }
                else
                {
                    callback?.Invoke(false, responseText);
                }
            }));
        }

        public void FetchProfile(Action<bool, PlayerProfileResponse, string> callback)
        {
            if (config == null || !IsAuthenticated)
            {
                callback?.Invoke(false, default, "Not authenticated.");
                return;
            }

            StartCoroutine(SendGetRequest(config.ProfileUrl, true, (success, responseText) =>
            {
                if (success)
                {
                    try
                    {
                        cachedProfile = JsonUtility.FromJson<PlayerProfileResponse>(responseText);
                        callback?.Invoke(true, cachedProfile, "Profile loaded.");
                    }
                    catch (Exception ex)
                    {
                        callback?.Invoke(false, default, $"Profile parse error: {ex.Message}");
                    }
                }
                else
                {
                    callback?.Invoke(false, default, responseText);
                }
            }));
        }

        public void Logout()
        {
            accessToken = string.Empty;
            refreshToken = string.Empty;
            cachedProfile = default;
            OnLogout?.Invoke();
        }

        private IEnumerator SendPostRequest(string url, string jsonBody, bool requiresAuth, Action<bool, string> onComplete)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                if (requiresAuth && IsAuthenticated)
                {
                    request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
                }

                if (config != null) request.timeout = Mathf.RoundToInt(config.RequestTimeoutSeconds);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onComplete?.Invoke(true, request.downloadHandler.text);
                }
                else
                {
                    string errorMsg = $"HTTP {request.responseCode}: {request.error}";
                    onComplete?.Invoke(false, errorMsg);
                }
            }
        }

        private IEnumerator SendGetRequest(string url, bool requiresAuth, Action<bool, string> onComplete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if (requiresAuth && IsAuthenticated)
                {
                    request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
                }

                if (config != null) request.timeout = Mathf.RoundToInt(config.RequestTimeoutSeconds);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onComplete?.Invoke(true, request.downloadHandler.text);
                }
                else
                {
                    string errorMsg = $"HTTP {request.responseCode}: {request.error}";
                    onComplete?.Invoke(false, errorMsg);
                }
            }
        }
    }
}
