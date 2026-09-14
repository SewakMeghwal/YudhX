using UnityEngine;

namespace BattleRoyale.Weapons
{
    public static class WeaponFactory
    {
        public static WeaponData CreateAssaultRifleData()
        {
            var data = ScriptableObject.CreateInstance<WeaponData>();
            SetWeaponDataFields(data, "ar_47", "AR-47 Alpha", WeaponType.AssaultRifle, AmmoType.Ammo556mm, 30, 2.4f, 35f, 600f, 250f, FireMode.FullAuto, 3.0f, 0.4f, 1.2f, 0.4f, 1);
            return data;
        }

        public static WeaponData CreateSmgData()
        {
            var data = ScriptableObject.CreateInstance<WeaponData>();
            SetWeaponDataFields(data, "sub_9", "Sub-9 Prime", WeaponType.SubmachineGun, AmmoType.Ammo9mm, 35, 1.9f, 22f, 850f, 150f, FireMode.FullAuto, 4.5f, 0.8f, 0.8f, 0.6f, 1);
            return data;
        }

        public static WeaponData CreateShotgunData()
        {
            var data = ScriptableObject.CreateInstance<WeaponData>();
            SetWeaponDataFields(data, "scatter_12", "Scatter-12", WeaponType.Shotgun, AmmoType.Gauge12, 8, 3.2f, 15f, 80f, 40f, FireMode.Single, 8.0f, 4.0f, 3.5f, 1.5f, 8);
            return data;
        }

        public static WeaponData CreatePistolData()
        {
            var data = ScriptableObject.CreateInstance<WeaponData>();
            SetWeaponDataFields(data, "pistol_9", "Pistol-9", WeaponType.Pistol, AmmoType.Ammo9mm, 15, 1.5f, 28f, 350f, 100f, FireMode.Single, 2.5f, 0.3f, 0.9f, 0.3f, 1);
            return data;
        }

        private static void SetWeaponDataFields(WeaponData data, string id, string name, WeaponType type, AmmoType ammo, int cap, float reload, float dmg, float rpm, float range, FireMode mode, float hipSpread, float adsSpread, float vertRecoil, float horizRecoil, int pellets)
        {
            var typeObj = typeof(WeaponData);
            typeObj.GetField("weaponId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, id);
            typeObj.GetField("weaponName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, name);
            typeObj.GetField("weaponType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, type);
            typeObj.GetField("ammoType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, ammo);
            typeObj.GetField("magazineCapacity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, cap);
            typeObj.GetField("reloadTimeSeconds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, reload);
            typeObj.GetField("baseDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, dmg);
            typeObj.GetField("fireRateRPM", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, rpm);
            typeObj.GetField("effectiveRangeMeters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, range);
            typeObj.GetField("fireMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, mode);
            typeObj.GetField("hipfireSpreadAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, hipSpread);
            typeObj.GetField("adsSpreadAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, adsSpread);
            typeObj.GetField("verticalRecoilAmount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, vertRecoil);
            typeObj.GetField("horizontalRecoilAmount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, horizRecoil);
            typeObj.GetField("pelletsPerShot", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(data, pellets);
        }
    }
}
