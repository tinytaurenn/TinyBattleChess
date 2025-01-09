using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    enum ESlot
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

    public SO_BasicWeapon m_MainWeapon; 
    public SO_BasicWeapon m_SecondaryWeapon;
 
    public SO_Item m_Slot_1; 
    public SO_Item m_Slot_2;
    public SO_Item m_Slot_3;
    public SO_Item m_Slot_4;

    //public SO_Armor m_Helmet;
    //public SO_Armor m_Chest;
    //public SO_Armor m_Legs;

    [Space(10)]
    [Header("Equipped Items")]

    [SerializeField] ESlot m_SelectedSlot = ESlot.MainWeapon;

    [Space(5)]

    public BasicWeapon m_EquippedMainWeapon;
    public BasicWeapon m_EquippedSecondaryWeapon;
    //public Armor m_EquippedHelmet;
    //public Armor m_EquippedChest;
    //public Armor m_EquippedLegs;
    public Grabbable m_EquippedSlot_1;
    public Grabbable m_EquippedSlot_2;
    public Grabbable m_EquippedSlot_3;
    public Grabbable m_EquippedSlot_4;

    [Space(10)]
    [Header("Player sockets")]
    [SerializeField] internal Transform m_PlayerLeftHandSocket;
    [SerializeField] internal Transform m_PlayerRightHandSocket;



    public void EquipItemInLoadout(SO_Item item)
    {
        
        if (item.GetType() == typeof(SO_BasicWeapon))
        {
            SO_BasicWeapon weapon = (SO_BasicWeapon)item;
            EquipWeaponLoadout(weapon);
            return; 
        }
        
    }

    public void EquipGrabbableItem(Grabbable item)
    {


        if (item.GetType() == typeof(BasicWeapon))
        {

            BasicWeapon weapon = (BasicWeapon)item;

            EquipWeapon(weapon, out bool rightHand);

            if (rightHand)
            {
                m_EquippedMainWeapon.m_Rigidbody.isKinematic = true;
                m_EquippedMainWeapon.m_Collider.enabled = false;
                m_EquippedMainWeapon.transform.SetParent(m_PlayerRightHandSocket, false);

                m_EquippedMainWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                m_SelectedSlot = ESlot.MainWeapon;
                //m_InHandSlot = GetSlotFromGrabbable(m_EquippedMainWeapon);

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
        DropItemOnSlot(m_SelectedSlot, throwForce);

    }
    void DropItemOnSlot(int slot, float throwForce = 0f)
    {
        if (GetGrabbableFromSlot((ESlot)slot) == null) return;


        DropItem(GetGrabbableFromSlot((ESlot)slot));
        ClearOnSlot((ESlot)slot);
    }

    void DropItemOnSlot(ESlot slot, float throwForce = 0f)
    {
        Debug.Log("drop item on slot");
        if (GetGrabbableFromSlot(slot) == null) return;

       
        DropItem(GetGrabbableFromSlot(slot)); 
        ClearOnSlot(slot);
    }

    void DropItem(Grabbable item)
    {
        Debug.Log("dropping item"); 
        item.transform.SetParent(null, true);
        item.m_Rigidbody.isKinematic = false;
        item.m_Rigidbody.AddForce(1f * transform.forward, ForceMode.VelocityChange);
        item.m_Rigidbody.AddTorque(-transform.right * 1f, ForceMode.VelocityChange);
        item.Release(); 
        item.m_Collider.enabled = true;
    }

    void EquipWeaponLoadout(SO_BasicWeapon weapon)
    {
        if(m_MainWeapon != null && m_MainWeapon.WeaponSize == SO_BasicWeapon.EWeaponSize.Two_Handed)
        {
            m_MainWeapon = null; 
            
        }
        if(weapon.WeaponSize == SO_BasicWeapon.EWeaponSize.Two_Handed)
        {
            m_MainWeapon = weapon;
            m_SecondaryWeapon = null;
            
            return; 
        }

        if (weapon.WeaponSize == SO_BasicWeapon.EWeaponSize.LeftOnly)
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

    void EquipWeapon(BasicWeapon weapon, out bool rightHand)
    {
        rightHand = true;
        if (m_EquippedMainWeapon != null && m_EquippedMainWeapon.GetWeaponSize() == SO_BasicWeapon.EWeaponSize.Two_Handed)
        {
            m_EquippedMainWeapon = null;

        }
        if (weapon.GetWeaponSize() == SO_BasicWeapon.EWeaponSize.Two_Handed)
        {
            m_EquippedMainWeapon = weapon;
            m_EquippedSecondaryWeapon = null;

            return;
        }

        if (weapon.GetWeaponSize() == SO_BasicWeapon.EWeaponSize.LeftOnly)
        {
            m_EquippedSecondaryWeapon = weapon;
            rightHand = false; 
        }
        else
        {
            if (m_EquippedMainWeapon != null && m_EquippedSecondaryWeapon == null)
            {
                m_EquippedSecondaryWeapon = weapon;
                rightHand = false;
            }
            else
            {
                m_EquippedMainWeapon = weapon;

            }
        }

    }
    public Grabbable GetGrabbableInHand()
    {
        return GetGrabbableFromSlot(m_SelectedSlot);    
    }

    Grabbable GetGrabbableFromSlot(ESlot slot)
    {
        switch (slot)
        {
            case ESlot.MainWeapon:
                return m_EquippedMainWeapon;
               
            case ESlot.SecondaryWeapon:
                return m_EquippedSecondaryWeapon;
                
            case ESlot.Slot_1:
                return m_EquippedSlot_1;

            case ESlot.Slot_2:
                return m_EquippedSlot_2;
            case ESlot.Slot_3:
                return m_EquippedSlot_3;
            case ESlot.Slot_4:
                return m_EquippedSlot_4;
                
            default:
                return m_EquippedMainWeapon;

        }
    }

    void ClearOnSlot(ESlot slot)
    {
        switch (slot)
        {
            case ESlot.MainWeapon:
                m_EquippedMainWeapon = null;
                return; 
            case ESlot.SecondaryWeapon:
                m_EquippedSecondaryWeapon = null;
                return;
            case ESlot.Slot_1:
                m_EquippedSlot_1 = null;
                return;
            case ESlot.Slot_2:
                m_EquippedSlot_2 = null;
                return;

            case ESlot.Slot_3:
                m_EquippedSlot_3 = null;
                return;
            case ESlot.Slot_4:
                m_EquippedSlot_4 = null;
                return;
            default:
                m_EquippedMainWeapon = null;
                return;
        }
    }

    public void DropEverything()
    {
        for (int i = 0; i < (int)ESlot.count; i++)
        {
            DropItemOnSlot(i);
        }
    
    }

}
