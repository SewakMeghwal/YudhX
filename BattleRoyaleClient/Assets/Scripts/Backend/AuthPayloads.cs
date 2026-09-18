using System;

namespace BattleRoyale.Backend
{
    [Serializable]
    public struct RegisterRequest
    {
        public string username;
        public string email;
        public string password;

        public RegisterRequest(string user, string mail, string pass)
        {
            username = user;
            email = mail;
            password = pass;
        }
    }

    [Serializable]
    public struct LoginRequest
    {
        public string username;
        public string password;

        public LoginRequest(string user, string pass)
        {
            username = user;
            password = pass;
        }
    }

    [Serializable]
    public struct JwtTokenResponse
    {
        public string access;
        public string refresh;
    }

    [Serializable]
    public struct PlayerProfileResponse
    {
        public int id;
        public string username;
        public string display_name;
        public string avatar_url;
        public int level;
        public int total_xp;
        public int total_matches;
        public int total_wins;
        public int total_kills;
        public int total_deaths;
        public float total_damage;
        public float kd_ratio;
        public float win_rate;
    }
}
