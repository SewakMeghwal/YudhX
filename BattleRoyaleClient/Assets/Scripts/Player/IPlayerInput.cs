using System;
using UnityEngine;

namespace BattleRoyale.Player
{
    public interface IPlayerInput
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool IsSprintPressed { get; }
        bool IsCrouchPressed { get; }
        bool IsJumpPressed { get; }

        event Action OnJumpPressedEvent;
        event Action OnCrouchToggledEvent;
    }
}
