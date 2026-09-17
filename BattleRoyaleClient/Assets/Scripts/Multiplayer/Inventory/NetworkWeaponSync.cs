using System;
using UnityEngine;
using BattleRoyale.InventorySystem;
using BattleRoyale.Weapons;

namespace BattleRoyale.Multiplayer.Inventory
{
    [RequireComponent(typeof(NetworkPlayer))]
    [RequireComponent(typeof(PlayerWeaponInventory))]
    public class NetworkWeaponSync : MonoBehaviour
    {
        [SerializeField] private NetworkPlayer networkPlayer;
        [SerializeField] private PlayerWeaponInventory weaponInventory;

        public event Action<ulong, WeaponSlotIndex> OnRemoteWeaponSlotChanged;

        private void Awake()
        {
            if (networkPlayer == null) networkPlayer = GetComponent<NetworkPlayer>();
            if (weaponInventory == null) weaponInventory = GetComponent<PlayerWeaponInventory>();
        }

        private void Start()
        {
            if (weaponInventory != null)
            {
                weaponInventory.OnWeaponSwitched += HandleLocalWeaponSwitched;
                weaponInventory.OnWeaponHolstered += HandleLocalWeaponHolstered;
            }
        }

        private void HandleLocalWeaponSwitched(WeaponSlotIndex slot, Weapon weapon)
        {
            if (networkPlayer == null || !networkPlayer.IsLocalPlayer) return;

            // Broadcast slot switch RPC to server & remote clients
            ServerReceiveWeaponSlotRpc(networkPlayer.NetworkId, slot);
        }

        private void HandleLocalWeaponHolstered()
        {
            if (networkPlayer == null || !networkPlayer.IsLocalPlayer) return;

            ServerReceiveWeaponSlotRpc(networkPlayer.NetworkId, WeaponSlotIndex.Holstered);
        }

        public void ServerReceiveWeaponSlotRpc(ulong playerId, WeaponSlotIndex slot)
        {
            // Execute on remote proxy clients
            ClientSyncWeaponSlotRpc(playerId, slot);
        }

        public void ClientSyncWeaponSlotRpc(ulong playerId, WeaponSlotIndex slot)
        {
            if (networkPlayer != null && !networkPlayer.IsLocalPlayer)
            {
                if (weaponInventory != null)
                {
                    weaponInventory.SwitchToSlot(slot);
                }
                OnRemoteWeaponSlotChanged?.Invoke(playerId, slot);
            }
        }

        private void OnDestroy()
        {
            if (weaponInventory != null)
            {
                weaponInventory.OnWeaponSwitched -= HandleLocalWeaponSwitched;
                weaponInventory.OnWeaponHolstered -= HandleLocalWeaponHolstered;
            }
        }
    }
}
