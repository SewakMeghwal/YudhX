using System;
using UnityEngine;

namespace BattleRoyale.CameraSystem
{
    public interface ICameraInput
    {
        Vector2 LookDelta { get; }
        bool IsAiming { get; }
        bool IsShoulderSwitchPressed { get; }

        event Action OnShoulderSwitchedEvent;
    }
}
