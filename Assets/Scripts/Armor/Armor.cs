using Coherence.Toolkit;
using UnityEngine;

public abstract class Armor : MonoBehaviour
{
    protected CoherenceSync m_Sync;

    public SO_Item SO_Item
    {
        get { return m_SO_Item; }
        private set { m_SO_Item = value; }
    }

    [SerializeField] SO_Item m_SO_Item;

    [Space(10)]
    [Header("Armor infos")]
    [SerializeField] protected SO_Armor.EArmorType m_ArmorType;

    [SerializeField]protected  int m_ArmorValue = 3;

    public abstract SO_Armor.EArmorPlace ArmorPlace { get;}

    protected virtual void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();         
    }
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual int CalculateDamageWithArmor(int baseDamage)
    {
        int calculatedDamage = baseDamage;

        calculatedDamage = Mathf.Clamp(calculatedDamage - m_ArmorValue, 0, calculatedDamage);

        return calculatedDamage; 
    }
}
