using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    public SO_BasicWeapon m_MainWeapon; 
    public SO_BasicWeapon m_SecondaryWeapon;
    //public SO_Armor m_Helmet;
    //public SO_Armor m_Chest;
    //public SO_Armor m_Legs;
    public SO_Item m_Slot_1; 
    public SO_Item m_Slot_2;
    public SO_Item m_Slot_3;
    public SO_Item m_Slot_4;

    public void EquipItemInLoadout(SO_Item item)
    {
        
        if (item.GetType() == typeof(SO_BasicWeapon))
        {
            SO_BasicWeapon weapon = (SO_BasicWeapon)item;
            if (weapon.WeaponSize != SO_BasicWeapon.EWeaponSize.LeftOnly)
            {
                m_MainWeapon = weapon;
            }
            else
            {
                m_SecondaryWeapon = weapon;
            }
        }
        
    }

}
