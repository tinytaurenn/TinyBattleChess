using Coherence.Toolkit;
using UnityEngine;

public abstract class Armor : Grabbable
{



    [Space(10)]
    [Header("Armor infos")]
    [SerializeField] protected SO_Armor.EArmorType m_ArmorType;

    [SerializeField]protected  int m_ArmorValue = 3; 

    public abstract SO_Armor.EArmorPlace ArmorPlace { get;}


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

        calculatedDamage = Mathf.Clamp(calculatedDamage - m_ArmorValue, 0, calculatedDamage);

        return calculatedDamage; 
    }
}
