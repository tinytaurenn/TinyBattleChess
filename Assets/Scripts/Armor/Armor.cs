using Coherence.Toolkit;
using System;
using UnityEngine;

public abstract class Armor : Grabbable
{
    [Serializable]
    public struct FArmorParameters
    {
        public int MagicArmor;
        public int Armor;
        public int Cost; 
        public SO_Armor.EArmorType ArmorType;
        public SO_Armor.EArmorPlace ArmorPlace;

        public FArmorParameters(int magicArmor,int armor, int cost, SO_Armor.EArmorType armorType, SO_Armor.EArmorPlace armorPlace)
        {
            MagicArmor = magicArmor;
            Armor = armor;
            Cost = cost;
            ArmorType = armorType;
            ArmorPlace = armorPlace;
        }
    }


    [Space(10)]
    [Header("Armor infos")]

    [SerializeField] FArmorParameters m_ArmorParameters = new FArmorParameters(0,0,10, SO_Armor.EArmorType.Leather, SO_Armor.EArmorPlace.Chest);  

    public virtual FArmorParameters ArmorParameters
    {
        get
        {
            return m_ArmorParameters;
        }
        set
        {
            m_ArmorParameters = value;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected virtual int CalculateDamageWithArmor(int baseDamage)
    {
        int calculatedDamage = baseDamage;

        calculatedDamage = Mathf.Clamp(calculatedDamage - ArmorParameters.Armor, 0, calculatedDamage);

        return calculatedDamage; 
    }
}
