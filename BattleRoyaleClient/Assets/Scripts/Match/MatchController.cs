using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Match
{
    public class MatchController : MonoBehaviour, IMatchManager
    {
        [SerializeField] private MatchConfig config;

        private MatchState currentState = MatchState.LobbyWaiting;
        private readonly Dictionary<ulong, MatchPlayerInfo> players = new Dictionary<ulong, MatchPlayerInfo>();

        private float stateTimer;
        private float matchDurationTimer;
        private int aliveCount;

        public MatchState CurrentState => currentState;
        public int AlivePlayerCount => aliveCount;
        public int TotalPlayerCount => players.Count;
        public float MatchTimerSeconds => matchDurationTimer;

        public event Action<MatchState> OnMatchStateChanged;
        public event Action<int, float> OnCountdownTick;
        public event Action<KillFeedEntry> OnKillFeedEvent;
        public event Action<MatchPlayerInfo> OnWinnerDeclared;

        private void Update()
        {
            if (config == null) return;

            switch (currentState)
            {
                case MatchState.LobbyWaiting:
                    if (players.Count >= config.MinRequiredPlayers)
                    {
                        TransitionToState(MatchState.StartingCountdown);
                        stateTimer = config.LobbyCountdownSeconds;
                    }
                    break;

                case MatchState.StartingCountdown:
                    stateTimer -= Time.deltaTime;
                    OnCountdownTick?.Invoke(players.Count, Mathf.Max(0f, stateTimer));

                    if (stateTimer <= 0f)
                    {
                        TransitionToState(MatchState.InCombat);
                    }
                    break;

                case MatchState.InCombat:
                    matchDurationTimer += Time.deltaTime;

                    // Check Match End Condition: 1 Player Remaining
                    if (aliveCount <= 1 && players.Count >= config.MinRequiredPlayers)
                    {
                        EvaluateWinner();
                    }
                    else if (matchDurationTimer >= config.MaxMatchDurationSeconds)
                    {
                        EvaluateWinner();
                    }
                    break;
            }
        }

        public void RegisterPlayer(ulong networkId, string displayName)
        {
            if (players.ContainsKey(networkId)) return;

            MatchPlayerInfo newPlayer = new MatchPlayerInfo(networkId, displayName);
            players.Add(networkId, newPlayer);
            aliveCount++;
        }

        public void UnregisterPlayer(ulong networkId)
        {
            if (!players.ContainsKey(networkId)) return;

            if (players[networkId].IsAlive)
            {
                aliveCount--;
            }
            players.Remove(networkId);
        }

        public void ReportPlayerElimination(ulong victimId, ulong attackerId, string weaponName, bool isHeadshot)
        {
            if (!players.ContainsKey(victimId)) return;

            // 1. Update Victim Info
            MatchPlayerInfo victim = players[victimId];
            if (!victim.IsAlive) return;

            victim.IsAlive = false;
            victim.PlacementRank = aliveCount; // Rank equals current remaining alive players
            players[victimId] = victim;
            aliveCount--;

            // 2. Update Attacker Kills
            string attackerName = "Environment";
            if (players.ContainsKey(attackerId))
            {
                MatchPlayerInfo attacker = players[attackerId];
                attacker.Kills++;
                players[attackerId] = attacker;
                attackerName = attacker.DisplayName;
            }

            // 3. Broadcast Kill Feed Entry
            KillFeedEntry entry = new KillFeedEntry(attackerName, victim.DisplayName, weaponName, isHeadshot);
            OnKillFeedEvent?.Invoke(entry);
        }

        private void EvaluateWinner()
        {
            MatchPlayerInfo winner = default;
            bool foundWinner = false;

            foreach (var player in players.Values)
            {
                if (player.IsAlive)
                {
                    winner = player;
                    winner.PlacementRank = 1;
                    foundWinner = true;
                    break;
                }
            }

            TransitionToState(MatchState.MatchEnded);

            if (foundWinner)
            {
                OnWinnerDeclared?.Invoke(winner);
            }
        }

        private void TransitionToState(MatchState newState)
        {
            currentState = newState;
            OnMatchStateChanged?.Invoke(currentState);
        }

        public int CalculatePlayerXP(MatchPlayerInfo player)
        {
            if (config == null) return 0;

            int killXP = player.Kills * config.XpPerKill;
            int winXP = (player.PlacementRank == 1) ? config.XpPerWin : 0;
            int survXP = Mathf.FloorToInt((matchDurationTimer / 60.0f) * config.XpPerMinuteSurv);

            return killXP + winXP + survXP;
        }
    }
}
