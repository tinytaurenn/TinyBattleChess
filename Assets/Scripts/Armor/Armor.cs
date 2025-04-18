
using System.Collections.Generic;
using UnityEngine;

public abstract class Armor : Grabbable
{
    
    [Space(10)]
    [Header("Armor infos")]

    [SerializeField] FArmorParameters m_ArmorParameters = new FArmorParameters(0,0, EArmorType.Leather, EArmorPlace.Chest);  

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

    public List<SO_ArmorEffect> ArmorEffects; 

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
