using UnityEngine;

namespace BattleRoyale.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "BattleRoyale/Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string weaponId = "ar_alpha";
        [SerializeField] private string weaponName = "Alpha-556 AR";
        [SerializeField] private WeaponType weaponType = WeaponType.AssaultRifle;

        [Header("Ammo & Capacity")]
        [SerializeField] private AmmoType ammoType = AmmoType.Ammo556mm;
        [SerializeField] private int magazineCapacity = 30;
        [SerializeField] private float reloadTimeSeconds = 2.5f;

        [Header("Ballistics & Damage")]
        [SerializeField] private float baseDamage = 35f;
        [SerializeField] private float fireRateRPM = 600f; // Rounds Per Minute
        [SerializeField] private float effectiveRangeMeters = 300f;
        [SerializeField] private FireMode fireMode = FireMode.FullAuto;

        [Header("Accuracy & Recoil")]
        [SerializeField] private float hipfireSpreadAngle = 3.5f;
        [SerializeField] private float adsSpreadAngle = 0.5f;
        [SerializeField] private float verticalRecoilAmount = 1.2f;
        [SerializeField] private float horizontalRecoilAmount = 0.4f;

        [Header("Pellets (Shotguns)")]
        [SerializeField] private int pelletsPerShot = 1;

        [Header("Audio & VFX")]
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip reloadSound;
        [SerializeField] private AudioClip emptySound;
        [SerializeField] private GameObject muzzleFlashPrefab;
        [SerializeField] private GameObject impactVfxPrefab;

        // Calculated Fire Interval (Seconds per shot)
        public float FireInterval => 60f / Mathf.Max(1f, fireRateRPM);

        public string WeaponId => weaponId;
        public string WeaponName => weaponName;
        public WeaponType WeaponType => weaponType;
        public AmmoType AmmoType => ammoType;
        public int MagazineCapacity => magazineCapacity;
        public float ReloadTimeSeconds => reloadTimeSeconds;
        public float BaseDamage => baseDamage;
        public float FireRateRPM => fireRateRPM;
        public float EffectiveRangeMeters => effectiveRangeMeters;
        public FireMode FireMode => fireMode;
        public float HipfireSpreadAngle => hipfireSpreadAngle;
        public float AdsSpreadAngle => adsSpreadAngle;
        public float VerticalRecoilAmount => verticalRecoilAmount;
        public float HorizontalRecoilAmount => horizontalRecoilAmount;
        public int PelletsPerShot => pelletsPerShot;
        public AudioClip FireSound => fireSound;
        public AudioClip ReloadSound => reloadSound;
        public AudioClip EmptySound => emptySound;
        public GameObject MuzzleFlashPrefab => muzzleFlashPrefab;
        public GameObject ImpactVfxPrefab => impactVfxPrefab;
    }
}
