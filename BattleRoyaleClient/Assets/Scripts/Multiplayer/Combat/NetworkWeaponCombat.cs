using System;
using UnityEngine;
using BattleRoyale.HealthSystem;
using BattleRoyale.InventorySystem;
using BattleRoyale.Weapons;

namespace BattleRoyale.Multiplayer.Combat
{
    [RequireComponent(typeof(NetworkPlayer))]
    [RequireComponent(typeof(PlayerWeaponInventory))]
    public class NetworkWeaponCombat : MonoBehaviour, INetworkedCombat
    {
        [SerializeField] private NetworkPlayer networkPlayer;
        [SerializeField] private PlayerWeaponInventory weaponInventory;
        [SerializeField] private LayerMask hitLayers = ~0;

        private float lastServerFireTime;

        public event Action<NetworkShotData> OnShotFiredBroadcast;
        public event Action<NetworkHitData> OnHitConfirmedBroadcast;

        private void Awake()
        {
            if (networkPlayer == null) networkPlayer = GetComponent<NetworkPlayer>();
            if (weaponInventory == null) weaponInventory = GetComponent<PlayerWeaponInventory>();
        }

        public void RequestFire(Vector3 origin, Vector3 direction, bool isADS)
        {
            Weapon activeWeapon = weaponInventory.ActiveWeapon;
            if (activeWeapon == null || !activeWeapon.CanFire()) return;

            NetworkShotData shotData = new NetworkShotData(
                networkPlayer.NetworkId,
                origin,
                direction,
                activeWeapon.Data.WeaponId,
                isADS,
                Time.time
            );

            // Execute local predicted muzzle FX
            activeWeapon.TryFire(origin, direction, isADS, hitLayers, out _);

            // Send to Server
            ServerReceiveFireRpc(shotData);
        }

        public void RequestReload()
        {
            Weapon activeWeapon = weaponInventory.ActiveWeapon;
            if (activeWeapon != null)
            {
                activeWeapon.TryStartReload(90, out _);
            }
        }

        // Server RPC Handler
        public void ServerReceiveFireRpc(NetworkShotData shotData)
        {
            Weapon activeWeapon = weaponInventory.ActiveWeapon;
            if (activeWeapon == null) return;

            // 1. Server Cadence Validation (Anti-Cheat check: rate-of-fire enforcement)
            float minInterval = activeWeapon.Data.FireInterval * 0.9f; // Allow 10% network jitter tolerance
            if (Time.time - lastServerFireTime < minInterval)
            {
                Debug.LogWarning($"[Anti-Cheat] Player {shotData.ShooterId} fire rate exceeded limit for {activeWeapon.Data.WeaponName}");
                return;
            }
            lastServerFireTime = Time.time;

            // 2. Authoritative Raycast Hit Detection on Server
            if (Physics.Raycast(shotData.Origin, shotData.Direction, out RaycastHit hit, activeWeapon.Data.EffectiveRangeMeters, hitLayers))
            {
                Hitbox hitbox = hit.collider.GetComponent<Hitbox>();
                PlayerHealth targetHealth = hit.collider.GetComponentInParent<PlayerHealth>();

                if (targetHealth != null && !targetHealth.IsDead)
                {
                    bool isHeadshot = (hitbox != null && hitbox.GetComponent<Hitbox>()?.transform.name.Contains("Head") == true);
                    float damage = activeWeapon.Data.BaseDamage * (isHeadshot ? 2.5f : 1.0f);

                    DamageInfo serverDamageInfo = new DamageInfo(
                        damage,
                        DamageType.Bullet,
                        isHeadshot ? HitboxLocation.Head : HitboxLocation.Chest,
                        hit.point,
                        shotData.Direction,
                        shotData.ShooterId.ToString()
                    );

                    // Apply Server-Authoritative Damage
                    targetHealth.TakeDamage(serverDamageInfo);

                    NetworkHitData hitData = new NetworkHitData(
                        shotData.ShooterId,
                        targetHealth.GetComponent<NetworkPlayer>() != null ? targetHealth.GetComponent<NetworkPlayer>().NetworkId : 0,
                        damage,
                        isHeadshot,
                        hit.point,
                        targetHealth.IsDead || targetHealth.IsDowned
                    );

                    // Broadcast Hit Confirmation to all Clients
                    ClientReceiveHitConfirmRpc(hitData);
                }
            }

            // Broadcast Shot Visuals to all Clients
            ClientReceiveShotEffectRpc(shotData);
        }

        public void ClientReceiveShotEffectRpc(NetworkShotData shotData)
        {
            OnShotFiredBroadcast?.Invoke(shotData);
        }

        public void ClientReceiveHitConfirmRpc(NetworkHitData hitData)
        {
            OnHitConfirmedBroadcast?.Invoke(hitData);
        }
    }
}
