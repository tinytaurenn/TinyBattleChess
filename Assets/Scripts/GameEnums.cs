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
#region Item Enums and Struct
[Flags]
public enum EItemType
{
    Weapon = 1, //rouge brun
    Armor = 2,  // passive without consomption // gris bleu 
    Potion = 4, // passive with consomption //vert pomme
    Scroll = 8, // ability with consomption // orange colruyt 
    Rune = 16 // Ability without consomption // bleu electrique
}

public enum EItemRarity
{
    Common = 0, // white
    Uncommon = 1, // green 
    Rare = 2, //blue
    Epic = 3, // purple
    Legendary = 4 // orange
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
[Flags]//used to select multiples choices
public enum EScrollElem
{
    Neutral = 1,
    Earth = 2,
    Fire = 4,
    Water = 8,
    Air = 16,
    Holy = 32,
    Shadow = 64,
    Nature = 128,
    Arcane = 256,
    Lightning = 512,
    Ice = 1024,
    Poison = 2048,
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
