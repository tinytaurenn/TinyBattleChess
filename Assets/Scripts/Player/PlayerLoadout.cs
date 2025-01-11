using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ESlotToWeaponDictionary : SerializableDictionary<PlayerLoadout.ESlot, BasicWeapon> { }

[Serializable]
public class ESlotToInventoryItemDictionary : SerializableDictionary<PlayerLoadout.ESlot, InventoryItem> { }
public class PlayerLoadout : MonoBehaviour
{
    public enum ESlot
    {
        MainWeapon = 0,
        SecondaryWeapon = 1,
        Slot_1 = 2,
        Slot_2 = 3,
        Slot_3 = 4,
        Slot_4 = 5,
        count = 6
    }
 
    [Header("Loadout Items")]

    public SO_Weapon m_MainWeapon; 
    public SO_Weapon m_SecondaryWeapon;
 
    public SO_Item m_Slot_1; 
    public SO_Item m_Slot_2;
    public SO_Item m_Slot_3;
    public SO_Item m_Slot_4;

    //public SO_Armor m_Helmet;
    //public SO_Armor m_Chest;
    //public SO_Armor m_Legs;

    [Space(10)]
    [Header("Equipped Items")]

    //[SerializeField] ESlot m_SelectedSlot = ESlot.MainWeapon;

    [Space(5)]

    //public BasicWeapon m_EquippedMainWeapon;
    //public BasicWeapon m_EquippedSecondaryWeapon;
    //public Armor m_EquippedHelmet;
    //public Armor m_EquippedChest;
    //public Armor m_EquippedLegs;
    //
    //public Grabbable m_EquippedSlot_1;
    //public Grabbable m_EquippedSlot_2;
    //public Grabbable m_EquippedSlot_3;
    //public Grabbable m_EquippedSlot_4;

    [SerializeField]
    public ESlotToWeaponDictionary m_EquippedWeapons = new ESlotToWeaponDictionary();
    [SerializeField]
    public ESlotToInventoryItemDictionary m_EquippedItems = new ESlotToInventoryItemDictionary();


    [Space(10)]
    [Header("Player sockets")]
    [SerializeField] internal Transform m_PlayerLeftHandSocket;
    [SerializeField] internal Transform m_PlayerRightHandSocket;


    private void Awake()
    {

        m_EquippedWeapons.Add(ESlot.MainWeapon, null);
        m_EquippedWeapons.Add(ESlot.SecondaryWeapon, null);
        m_EquippedItems.Add(ESlot.Slot_1, null); 
        m_EquippedItems.Add(ESlot.Slot_2, null);
        m_EquippedItems.Add(ESlot.Slot_3, null);
        m_EquippedItems.Add(ESlot.Slot_4, null);


        //SelectSlot(m_SelectedSlot);
    }

    public void EquipItemInLoadout(SO_Item item)
    {
        
        if (item.GetType() == typeof(SO_Weapon))
        {
            Debug.Log("equip SO  weapon in loadout : " + item.GetType().ToString());
            SO_Weapon weapon = (SO_Weapon)item;
            EquipWeaponLoadout(weapon);
            return; 
        }
        Debug.Log("equip SO  item in loadout : " + item.GetType().ToString());
        EquipInventoryItemInLoadout(item);
        return;

    }

    public void EquipGrabbableItem(Grabbable item)
    {


        if (item.GetType() == typeof(BasicWeapon))
        {

            BasicWeapon weapon = (BasicWeapon)item;

            EquipWeapon(weapon, out bool rightHand);

            if (rightHand)
            {
                m_EquippedWeapons[ESlot.MainWeapon].m_Rigidbody.isKinematic = true;
                m_EquippedWeapons[ESlot.MainWeapon].m_Collider.enabled = false;
                m_EquippedWeapons[ESlot.MainWeapon].transform.SetParent(m_PlayerRightHandSocket, false);

                m_EquippedWeapons[ESlot.MainWeapon].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                //m_SelectedSlot = ESlot.MainWeapon;
                //SelectSlot(ESlot.MainWeapon);
                
                LocalUI.Instance.ChangeSlotIcon(ESlot.MainWeapon, m_EquippedWeapons[ESlot.MainWeapon].SO_Item.ItemIcon);

            }
            else
            {
                //m_InHandItem = m_EquippedSecondaryWeapon;
                //m_InHandSlot = GetSlotFromGrabbable(m_EquippedSecondaryWeapon);
            }
            


            return;
        }

        
    }

    public void DropItemInHand(float throwForce = 0f)
    {
        Debug.Log("drop item in hand"); 
        DropItemOnSlot(ESlot.MainWeapon, throwForce);

    }

    public void DropWeapons(float throwForce = 0f)
    {
        if(GetGrabbableFromSlot((ESlot)ESlot.MainWeapon)!= null)
        {
            DropItem(GetGrabbableFromSlot(ESlot.MainWeapon), throwForce);
            ClearOnSlot(ESlot.MainWeapon);
        }
        if (GetGrabbableFromSlot((ESlot)ESlot.SecondaryWeapon) != null)
        {
            DropItem(GetGrabbableFromSlot(ESlot.SecondaryWeapon), throwForce);
            ClearOnSlot(ESlot.SecondaryWeapon);
        }
    }
    void DropItemOnSlot(int slot, float throwForce = 0f)
    {
        if (GetGrabbableFromSlot((ESlot)slot) == null) return;


        DropItem(GetGrabbableFromSlot((ESlot)slot), throwForce);
        ClearOnSlot((ESlot)slot);
    }

    void DropItemOnSlot(ESlot slot, float throwForce = 0f)
    {
        Debug.Log("drop item on slot");
        if (GetGrabbableFromSlot(slot) == null) return;

       
        DropItem(GetGrabbableFromSlot(slot), throwForce); 
        ClearOnSlot(slot);
    }

    void DropItem(Grabbable item, float throwForce = 0f)
    {
        Debug.Log("dropping item"); 
        item.transform.SetParent(null, true);
        item.m_Rigidbody.isKinematic = false;
        item.m_Rigidbody.AddForce(throwForce * transform.forward, ForceMode.VelocityChange);
        item.m_Rigidbody.AddTorque(-transform.right * 1f, ForceMode.VelocityChange);
        item.Release(); 
        item.m_Collider.enabled = true;
    }

    void EquipWeaponLoadout(SO_Weapon weapon)
    {
        if(m_MainWeapon != null && m_MainWeapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed)
        {
            m_MainWeapon = null; 
            
        }
        if(weapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed)
        {
            m_MainWeapon = weapon;
            m_SecondaryWeapon = null;
            
            return; 
        }

        if (weapon.WeaponSize == SO_Weapon.EWeaponSize.LeftOnly)
        {
            m_SecondaryWeapon = weapon;
        }
        else
        {
            if (m_MainWeapon != null && m_SecondaryWeapon == null)
            {
                m_SecondaryWeapon = weapon;
            }
            else
            {
                m_MainWeapon = weapon;
            }
        }

    }

    void EquipInventoryItemInLoadout(SO_Item item)
    {
        //find place and put in slot
        Debug.Log("equip inventory item in loadout");
    }

    void EquipWeapon(BasicWeapon weapon, out bool rightHand)
    {
        rightHand = true;

        
        if ( m_EquippedWeapons[ESlot.MainWeapon] != null && m_EquippedWeapons[ESlot.MainWeapon].GetComponent<BasicWeapon>().GetWeaponSize() == SO_Weapon.EWeaponSize.Two_Handed)
        {
            m_EquippedWeapons[ESlot.MainWeapon]= null;

        }
        if (weapon.GetWeaponSize() == SO_Weapon.EWeaponSize.Two_Handed)
        {
            m_EquippedWeapons[ESlot.MainWeapon]= weapon;
            m_EquippedWeapons[ESlot.SecondaryWeapon]= null;

            return;
        }

        if (weapon.GetWeaponSize() == SO_Weapon.EWeaponSize.LeftOnly)
        {
            m_EquippedWeapons[ESlot.SecondaryWeapon] = weapon;
            rightHand = false; 
        }
        else
        {
            if (m_EquippedWeapons[ESlot.MainWeapon] != null && m_EquippedWeapons[ESlot.SecondaryWeapon].GetComponent<BasicWeapon>() == null)
            {
                m_EquippedWeapons[ESlot.SecondaryWeapon]= weapon;
                rightHand = false;
            }
            else
            {
                m_EquippedWeapons[ESlot.MainWeapon]= weapon;

            }
        }

    }
    public Grabbable GetGrabbableInHand()
    {
        return GetGrabbableFromSlot(ESlot.MainWeapon);    
    }

    Grabbable GetGrabbableFromSlot(ESlot slot)
    {
        
        return m_EquippedWeapons[slot];
    }

    void ClearOnSlot(ESlot slot)
    {
        LocalUI.Instance.ClearSlot(slot);

        m_EquippedWeapons[slot] = null;

    }

    public void DropEverything()
    {
        //for (int i = 0; i < (int)ESlot.count; i++)
        //{
        //    DropItemOnSlot(i);
        //}

        DropWeapons(3); 
    
    }

    public void SlotActionPerformed(ESlot slot)
    {
        Debug.Log("slot action performed " + slot);
        if(m_EquippedItems[slot] == null)
        {
            Debug.Log("no item in slot");
            return; 
        }


    }

    
    

}
