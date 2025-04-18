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
public enum EEffectType
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
    Hands,
    Melee,
    Staff,
    Ranged,
    Shield,
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
    public EEffectType DamageType;
    public EWeaponType WeaponType;
    public EWeaponSize WeaponSize;
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;


    public FWeaponParameters(EEffectType damageType, EWeaponType weaponType, EWeaponSize weaponSize, Vector3 positionOffset, Vector3 rotationOffset)
    {

        DamageType = damageType;
        WeaponType = weaponType;
        WeaponSize = weaponSize;
        PositionOffset = positionOffset;
        RotationOffset = rotationOffset;
    }
    public FWeaponParameters( EEffectType damageType,EWeaponType weaponType, EWeaponSize weaponSize)
    {

        DamageType = damageType;
        WeaponType = weaponType;
        WeaponSize = weaponSize;
        PositionOffset = Vector3.zero;
        RotationOffset = Vector3.zero;


    }



}
[Serializable]
public struct FMeleeWeaponParameters
{
    public int Damage;
    public float Speed;
    public bool CanPierce; 

    public FMeleeWeaponParameters(int damage, float speed,bool piece)
    {
        Damage = damage;
        Speed = speed;
        CanPierce = piece;
    }
}
public enum EGameEffect
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
    Stun,
    Slow,
    Blind,
    Grounded,
    Bump,
    weakness,

}
[Serializable]
public struct FGameEffect
{
    public EGameEffect Effect;
    public float Value;
    public float EffectDuration;
    public EEffectType EffectType; 

    public FGameEffect(EGameEffect effect, float value, float effectDuration)
    {
        Effect = effect;
        Value = value;
        EffectDuration = effectDuration;
        EffectType = EEffectType.Physical;
    }

    public FGameEffect(EGameEffect effect, float value)
    {
        Effect = effect;
        Value = value;
        EffectDuration = 0;
        EffectType = EEffectType.Physical;
    }
    public FGameEffect(EGameEffect effect, float value, float effectDuration, EEffectType damageType)
    {
        Effect = effect;
        Value = value;
        EffectDuration = effectDuration;
        EffectType = damageType;
    }
    public FGameEffect(EGameEffect effect, float value, EEffectType damageType)
    {
        Effect = effect;
        Value = value;
        EffectDuration = 0;
        EffectType = damageType;
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
    public EArmorType ArmorType;
    public EArmorPlace ArmorPlace;

    public FArmorParameters(int magicArmor, int armor, EArmorType armorType, EArmorPlace armorPlace)
    {
        MagicArmor = magicArmor;
        Armor = armor;
        ArmorType = armorType;
        ArmorPlace = armorPlace;
    }
}
#endregion
