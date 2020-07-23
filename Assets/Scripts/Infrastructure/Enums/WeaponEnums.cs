public enum WeaponCommands{
    hold = 1 << 1,
    point = 1 << 2,
    sheath = 1 << 3,
    store = 1 << 4,
    none = 1 << 5,

    aim = hold | point
}