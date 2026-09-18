using System;

namespace BattleRoyale.Backend
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        string AccessToken { get; }

        void Register(string username, string email, string password, Action<bool, string> callback);
        void Login(string username, string password, Action<bool, string> callback);
        void FetchProfile(Action<bool, PlayerProfileResponse, string> callback);
        void Logout();

        event Action OnLoginSuccess;
        event Action OnLogout;
    }
}
