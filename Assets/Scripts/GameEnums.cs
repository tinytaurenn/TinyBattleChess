using System;
using UnityEngine;


#region Entity Enums and Struct
public enum EWeaponDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}
public enum EStuffSlot
{
    MainWeapon = 0,
    SecondaryWeapon = 1,
    Slot_1 = 2,
    Slot_2 = 3,
    Slot_3 = 4,
    Slot_4 = 5,
    Helmet = 6,
    Chest = 7,
    Shoulders = 8,
    count = 9
}
#endregion
#region Other Enums and Struct
public enum EDamageType
{
    Physical = 1,
    Magical = 2,
    Fire = 4,
    Poison = 8,
    Bleed = 16,
    Psy = 32,
}
#endregion
#region Item Enums and Struct
[Flags]
public enum EItemType
{
    Weapon = 1, //rouge brun
    Armor = 2,  // passive without consomption // gris bleu 
    Potion = 4, // passive with consomption //vert pomme
    Scroll = 8, // ability with consomption // orange colruyt 
}

public enum EItemRarity
{
    Common = 0, // white
    Uncommon = 1, // green 
    Rare = 2, //blue
    Epic = 3, // purple
    Legendary = 4 // orange
}
public enum EWeaponType
{
    Sword = 1,
    Axe = 2,
    Mace = 4,
    Spear = 8,
    Bow = 16,
    Crossbow = 32,
    LongBow = 64,
    Staff = 128,
    Shield = 256,
    Dagger = 512
}
public enum EWeaponSize
{
    Right_Handed,
    Left_Handed,
    Two_Handed,

}
[Serializable]
public struct FWeaponParameters
{
    public int Damage;
    public float Speed;
    public int Cost;
    public EDamageType DamageType;
    public EWeaponType WeaponType;
    public EWeaponSize WeaponSize;
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;


    public FWeaponParameters(int damage, float speed, int cost,EDamageType damageType, EWeaponType weaponType, EWeaponSize weaponSize, Vector3 positionOffset, Vector3 rotationOffset)
    {
        Damage = damage;
        Speed = speed;
        Cost = cost;
        DamageType = damageType;
        WeaponType = weaponType;
        WeaponSize = weaponSize;
        PositionOffset = positionOffset;
        RotationOffset = rotationOffset;
    }
    public FWeaponParameters(int damage, float speed, int cost, EDamageType damageType,EWeaponType weaponType, EWeaponSize weaponSize)
    {
        Damage = damage;
        Speed = speed;
        Cost = cost;
        DamageType = damageType;
        WeaponType = weaponType;
        WeaponSize = weaponSize;
        PositionOffset = Vector3.zero;
        RotationOffset = Vector3.zero;


    }

}
public enum EPotionEffect
{
    Healing,
    Regeneration,
    Strength,
    Speed,
    AttackSpeed,
    JumpHeight,
    Fly,
    Parry,
    Invisibility,
    Damage,
    Poison,
    Fire,
    Slow,
    Blind,
    Grounded,
    weakness,

}
[Serializable]
public struct FPotionEffect
{
    public EPotionEffect Effect;
    public float Value;
    public float EffectDuration;

    public FPotionEffect(EPotionEffect effect, float value, float effectDuration)
    {
        Effect = effect;
        Value = value;
        EffectDuration = effectDuration;
    }

    public FPotionEffect(EPotionEffect effect, float value)
    {
        Effect = effect;
        Value = value;
        EffectDuration = 0;
    }
}


public enum EArmorType
{
    Cloth = 0,
    Leather = 1,
    Mail = 2,
    Plate = 3,
}

public enum EArmorPlace
{
    Helmet = 0,
    Chest = 1,
    Shoulders = 2,
}
[Serializable]
public struct FArmorParameters
{
    public int MagicArmor;
    public int Armor;
    public int Cost;
    public EArmorType ArmorType;
    public EArmorPlace ArmorPlace;

    public FArmorParameters(int magicArmor, int armor, int cost, EArmorType armorType, EArmorPlace armorPlace)
    {
        MagicArmor = magicArmor;
        Armor = armor;
        Cost = cost;
        ArmorType = armorType;
        ArmorPlace = armorPlace;
    }
}
#endregion
