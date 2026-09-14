using System;
using UnityEngine;
using BattleRoyale.HealthSystem;

namespace BattleRoyale.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Transform muzzleTransform;
        [SerializeField] private AudioSource weaponAudioSource;

        private int currentAmmo;
        private float nextFireTime;
        private bool isReloading;
        private float reloadTimer;

        public WeaponData Data => weaponData;
        public int CurrentAmmo => currentAmmo;
        public bool IsReloading => isReloading;

        public event Action OnFired;
        public event Action OnReloadStarted;
        public event Action OnReloadFinished;
        public event Action OnEmptyTriggerPressed;

        private void Awake()
        {
            if (weaponAudioSource == null)
            {
                weaponAudioSource = GetComponent<AudioSource>();
            }

            if (weaponData != null)
            {
                currentAmmo = weaponData.MagazineCapacity;
            }
        }

        public void Initialize(WeaponData data)
        {
            weaponData = data;
            if (weaponData != null)
            {
                currentAmmo = weaponData.MagazineCapacity;
            }
        }

        public bool CanFire()
        {
            if (weaponData == null) return false;
            if (isReloading) return false;
            if (Time.time < nextFireTime) return false;
            if (currentAmmo <= 0) return false;

            return true;
        }

        public bool TryFire(Vector3 origin, Vector3 direction, bool isADS, LayerMask hitMask, out RaycastHit mainHitInfo)
        {
            mainHitInfo = default;

            if (weaponData == null) return false;

            if (currentAmmo <= 0)
            {
                PlayAudio(weaponData.EmptySound);
                OnEmptyTriggerPressed?.Invoke();
                return false;
            }

            if (!CanFire()) return false;

            // Fire cadence limit
            nextFireTime = Time.time + weaponData.FireInterval;
            currentAmmo--;

            // Spawn Muzzle Flash VFX
            if (weaponData.MuzzleFlashPrefab != null && muzzleTransform != null)
            {
                Instantiate(weaponData.MuzzleFlashPrefab, muzzleTransform.position, muzzleTransform.rotation);
            }

            PlayAudio(weaponData.FireSound);

            int pelletCount = Mathf.Max(1, weaponData.PelletsPerShot);
            float spreadAngle = isADS ? weaponData.AdsSpreadAngle : weaponData.HipfireSpreadAngle;
            bool hitAnything = false;

            for (int i = 0; i < pelletCount; i++)
            {
                Vector3 spreadDirection = ApplySpread(direction, spreadAngle);

                if (Physics.Raycast(origin, spreadDirection, out RaycastHit hit, weaponData.EffectiveRangeMeters, hitMask))
                {
                    if (!hitAnything)
                    {
                        mainHitInfo = hit;
                        hitAnything = true;
                    }

                    // Process Impact Damage
                    ProcessHitTarget(hit, spreadDirection);

                    // Impact Particle VFX
                    if (weaponData.ImpactVfxPrefab != null)
                    {
                        Instantiate(weaponData.ImpactVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }

            OnFired?.Invoke();
            return true;
        }

        private Vector3 ApplySpread(Vector3 direction, float maxAngleDegrees)
        {
            if (maxAngleDegrees <= 0.01f) return direction;

            float randomRadius = UnityEngine.Random.Range(0f, maxAngleDegrees);
            float randomAngle = UnityEngine.Random.Range(0f, 360f);

            Quaternion spreadRotation = Quaternion.AngleAxis(randomRadius, Vector3.up) * Quaternion.AngleAxis(randomAngle, Vector3.forward);
            return Quaternion.LookRotation(direction) * spreadRotation * Vector3.forward;
        }

        private void ProcessHitTarget(RaycastHit hit, Vector3 hitDirection)
        {
            // Check for body part Hitbox component first
            Hitbox hitbox = hit.collider.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                DamageInfo info = new DamageInfo(
                    weaponData.BaseDamage,
                    DamageType.Bullet,
                    HitboxLocation.Chest,
                    hit.point,
                    hitDirection
                );
                hitbox.ProcessHit(info);
                return;
            }

            // Fallback direct IDamageable check
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                DamageInfo info = new DamageInfo(
                    weaponData.BaseDamage,
                    DamageType.Bullet,
                    HitboxLocation.Chest,
                    hit.point,
                    hitDirection
                );
                damageable.TakeDamage(info);
            }
        }

        public bool TryStartReload(int availableReserveAmmo, out int ammoConsumed)
        {
            ammoConsumed = 0;
            if (weaponData == null || isReloading || currentAmmo >= weaponData.MagazineCapacity || availableReserveAmmo <= 0)
            {
                return false;
            }

            int neededAmmo = weaponData.MagazineCapacity - currentAmmo;
            ammoConsumed = Mathf.Min(neededAmmo, availableReserveAmmo);

            isReloading = true;
            reloadTimer = weaponData.ReloadTimeSeconds;

            PlayAudio(weaponData.ReloadSound);
            OnReloadStarted?.Invoke();
            return true;
        }

        public void UpdateReload(float deltaTime)
        {
            if (!isReloading) return;

            reloadTimer -= deltaTime;
            if (reloadTimer <= 0f)
            {
                isReloading = false;
                currentAmmo = weaponData.MagazineCapacity;
                OnReloadFinished?.Invoke();
            }
        }

        private void PlayAudio(AudioClip clip)
        {
            if (clip != null && weaponAudioSource != null)
            {
                weaponAudioSource.PlayOneShot(clip);
            }
        }
    }
}
