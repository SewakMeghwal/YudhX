using System;
using UnityEngine;

namespace BattleRoyale.Weapons
{
    public interface IWeapon
    {
        WeaponData Data { get; }
        int CurrentAmmo { get; }
        bool IsReloading { get; }

        bool CanFire();
        bool TryFire(Vector3 origin, Vector3 direction, bool isADS, LayerMask hitMask, out RaycastHit hitInfo);
        bool TryStartReload(int availableReserveAmmo, out int ammoConsumed);
        void UpdateReload(float deltaTime);

        event Action OnFired;
        event Action OnReloadStarted;
        event Action OnReloadFinished;
        event Action OnEmptyTriggerPressed;
    }
}
