namespace BattleRoyale.Weapons
{
    public enum WeaponType
    {
        AssaultRifle,
        SubmachineGun,
        Shotgun,
        Pistol,
        SniperRifle,
        Melee
    }

    public enum AmmoType
    {
        Ammo9mm,
        Ammo556mm,
        Ammo762mm,
        Gauge12,
        Ammo45ACP
    }

    public enum FireMode
    {
        Single,
        Burst,
        FullAuto
    }

    public enum AttachmentSlot
    {
        Muzzle,
        Optics,
        Magazine,
        Grip,
        Stock
    }
}
