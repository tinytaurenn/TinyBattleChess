using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Potion", menuName = "Scriptable Objects/Items/Potions/SO_Potions")]
public class SO_Potion : SO_Item
{
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
    public List<FPotionEffect> Effects;

    public int Charges = 1;
    


}
