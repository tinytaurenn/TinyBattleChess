using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

[Serializable]
public class ESlotToWeaponDictionary : SerializableDictionary<PlayerLoadout.ESlot, BasicWeapon> { }

[Serializable]
public class ESlotToInventoryItemDictionary : SerializableDictionary<PlayerLoadout.ESlot, InventoryItem> { }

[Serializable]

public class EArmorTypeToArmorDictionary : SerializableDictionary<PlayerLoadout.ESlot, Armor> { }

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
        Helmet = 6,
        Chest = 7,
        Shoulders = 8,
        count = 9
    }

    public enum EInventoryType
    {
        Loadout, 
        Equipped,
    }
    EInventoryType m_InventoryType = EInventoryType.Loadout;

    TinyPlayer m_TinyPlayer; 
 
    [Header("Loadout Items")]
    [Space(5)]
    [Header("Weapons")]
    public SO_Weapon m_MainWeapon; 
    public SO_Weapon m_SecondaryWeapon;
    [Space(5)]
    [Header("Items")]
    public SO_Item m_Slot_1; 
    public SO_Item m_Slot_2;
    public SO_Item m_Slot_3;
    public SO_Item m_Slot_4;
    [Space(5)]
    [Header("Armor")]
    public SO_Armor m_Helmet;
    public SO_Armor m_Chest;
    public SO_Armor m_Shoulders;
    [Space(5)]
    [SerializeField] SO_Item m_StandByItem;

    [Space(10)]
    [Header("Equipped Items")]

    //[SerializeField] ESlot m_SelectedSlot = ESlot.MainWeapon;

    [Space(5)]

  

    [SerializeField]
    public ESlotToWeaponDictionary m_EquippedWeapons = new ESlotToWeaponDictionary();
    [SerializeField]
    public ESlotToInventoryItemDictionary m_EquippedItems = new ESlotToInventoryItemDictionary();

    [SerializeField]
    public EArmorTypeToArmorDictionary m_EquippedArmor = new EArmorTypeToArmorDictionary();


    [Space(10)]
    [Header("Player sockets")]
    [SerializeField] internal Transform m_PlayerLeftHandSocket;
    [SerializeField] internal Transform m_PlayerRightHandSocket;
    [SerializeField] internal Transform m_PlayerPocket;
    [Header("Player Armor sockets")]
    [SerializeField] internal Transform m_HelmetSocket;
    [SerializeField] internal Transform m_ChestSocket;
    [SerializeField] internal Transform m_LeftShoulderSocket;
    [SerializeField] internal Transform m_RightShoulderSocket;


    private void Awake()
    {

        m_TinyPlayer = GetComponent<TinyPlayer>();

        m_EquippedWeapons.Add(ESlot.MainWeapon, null);
        m_EquippedWeapons.Add(ESlot.SecondaryWeapon, null);
       
        m_EquippedItems.Add(ESlot.Slot_1, null); 
        m_EquippedItems.Add(ESlot.Slot_2, null);
        m_EquippedItems.Add(ESlot.Slot_3, null);
        m_EquippedItems.Add(ESlot.Slot_4, null);
        //
        m_EquippedArmor.Add(ESlot.Helmet, null);
        m_EquippedArmor.Add(ESlot.Chest, null);
        m_EquippedArmor.Add(ESlot.Shoulders, null);
        //
        //SelectSlot(m_SelectedSlot);
    }

    public void EquipItemInLoadout(SO_Item item)
    {
        
        if (item.GetType() == typeof(SO_Weapon))
        {
            Debug.Log("equip SO  weapon in loadout : " + item.GetType().ToString());
            SO_Weapon weapon = (SO_Weapon)item;
            EquipWeaponLoadout(weapon);
            CloseItemSelection();

        }else if(item.GetType() == typeof(SO_Armor))
        {
            Debug.Log("equip SO  armor in loadout : " + item.GetType().ToString());
            SO_Armor armor = (SO_Armor)item;
            EquipArmorInLoadout(armor);
            CloseItemSelection();
        }
        else
        {
            Debug.Log("equip SO  item in loadout : " + item.GetType().ToString());
            EquipInventoryItemInLoadout(item);
            
            
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
                m_EquippedWeapons[ESlot.SecondaryWeapon].m_Rigidbody.isKinematic = true;
                m_EquippedWeapons[ESlot.SecondaryWeapon].m_Collider.enabled = false;
                m_EquippedWeapons[ESlot.SecondaryWeapon].transform.SetParent(m_PlayerLeftHandSocket, false);

                m_EquippedWeapons[ESlot.SecondaryWeapon].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                LocalUI.Instance.ChangeSlotIcon(ESlot.SecondaryWeapon, m_EquippedWeapons[ESlot.SecondaryWeapon].SO_Item.ItemIcon);
            }
            


            return;
        }

        
    }

    public void DropItemInHand(float throwForce = 0f)
    {
        Debug.Log("drop item in hand"); 
        DropItemOnSlot(ESlot.MainWeapon, throwForce);

        if (m_EquippedWeapons[ESlot.MainWeapon] != null)
        {
            DropItemOnSlot(ESlot.MainWeapon, throwForce);
            return; 
        }
        if (m_EquippedWeapons[ESlot.SecondaryWeapon] != null)
        {
            DropItemOnSlot(ESlot.SecondaryWeapon, throwForce);
        }

    }

    public void DropWeapons(float throwForce = 5f)
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
    void DropItemOnSlot(int slot, float throwForce = 5f)
    {
        if (GetGrabbableFromSlot((ESlot)slot) == null) return;


        DropItem(GetGrabbableFromSlot((ESlot)slot), throwForce);
        ClearOnSlot((ESlot)slot);

    }

    void DropItemOnSlot(ESlot slot, float throwForce = 5f)
    {
        Debug.Log("drop item on slot");
        if (GetGrabbableFromSlot(slot) == null) return;

       
        DropItem(GetGrabbableFromSlot(slot), throwForce); 
        ClearOnSlot(slot);
    }

    void DropItem(Grabbable item, float throwForce = 5f)
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

        if(weapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed)
        {
            m_MainWeapon = weapon;
            m_SecondaryWeapon = null;

        }
        else if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Right_Handed)
        {
            m_MainWeapon = weapon;
        }
        else if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Left_Handed)
        {
            if (m_MainWeapon!=null && m_MainWeapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed) m_MainWeapon = null; 
            m_SecondaryWeapon = weapon; 
        }

    }

    public void EquipArmorInLoadout(SO_Armor armor)
    {
        switch (armor.ArmorPLace)
        {
            case SO_Armor.EArmorPlace.Helmet:
                m_Helmet = armor;
                break;
            case SO_Armor.EArmorPlace.Chest:
                m_Chest = armor;
                break;
            case SO_Armor.EArmorPlace.Shoulders:
                m_Shoulders = armor;
                break;
            default:
                break;
        }
    }

    void EquipInventoryItemInLoadout(SO_Item item)
    {
        //find place and put in slot
        Debug.Log("equip inventory item in loadout");

        if(TryFindEmptySlot(out ESlot slot))
        {

            if (slot == ESlot.Slot_1) { m_Slot_1 = item; }
            else if (slot == ESlot.Slot_2) { m_Slot_2 = item; }
            else if (slot == ESlot.Slot_3) { m_Slot_3 = item; }
            else if (slot == ESlot.Slot_4) { m_Slot_4 = item;  }

            CloseItemSelection();

        }
        else
        {
            //select and replace
            Debug.Log("no place left in loadout, replacing"); 
            m_StandByItem = item;
            m_TinyPlayer.m_PlayerControls.EnterReplaceInventorySlotControls();
            LocalUI.Instance.OnReplaceItem(); 
            
        }


    }

    public void ReplaceInventorySlotLoadout(int slot)
    {
        Debug.Log("replace inventory slot");
        switch (slot)
        {
            case 0:
                m_Slot_1 = m_StandByItem;
                break;
            case 1:
                m_Slot_2 = m_StandByItem;
                break;
            case 2:
                m_Slot_3 = m_StandByItem;
                break;
            case 3:
                m_Slot_4 = m_StandByItem;
                break;
            default:
                break;
        }

        CloseItemSelection();
    }

    void EquipWeapon(BasicWeapon weapon, out bool rightHand)
    {
        rightHand = true;

        
        if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed)
        {
            if (m_EquippedWeapons[ESlot.MainWeapon] != null || m_EquippedWeapons[ESlot.SecondaryWeapon] != null)
            {
                DropWeapons();
            }
            m_EquippedWeapons[ESlot.MainWeapon]= weapon; 
            m_EquippedWeapons[ESlot.SecondaryWeapon]= null;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(true);

        }

        else if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Left_Handed)
        {
            DropItemOnSlot(ESlot.SecondaryWeapon);
            //if (m_MainWeapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed) m_MainWeapon = null;
            if ((m_EquippedWeapons[ESlot.MainWeapon] != null) &&
                m_EquippedWeapons[ESlot.MainWeapon].WeaponSize == SO_Weapon.EWeaponSize.Two_Handed) m_EquippedWeapons[ESlot.MainWeapon] = null; 
            m_EquippedWeapons[ESlot.SecondaryWeapon] = weapon;
            rightHand = false;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(false);
            
        }
        else if(weapon.WeaponSize == SO_Weapon.EWeaponSize.Right_Handed)
        {
            DropItemOnSlot(ESlot.MainWeapon);
            m_EquippedWeapons[ESlot.MainWeapon] = weapon;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(false);
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
            Debug.Log("no item equipped in this  slot");
            return; 
        }
        if(!m_TinyPlayer.CanPlayerUseInventoryItem()) return;

        Debug.Log("use item in slot");
        m_EquippedItems[ESlot.MainWeapon].UseInventoryItem(); 

        
    }

    ESlot FindFirstEmptySlot()
    {
        
        if(m_Slot_1 == null) return ESlot.Slot_1;
        if (m_Slot_2 == null) return ESlot.Slot_2;
        if (m_Slot_3 == null) return ESlot.Slot_3;
        if (m_Slot_4 == null) return ESlot.Slot_4;


        return ESlot.Slot_1;
    }
    bool TryFindEmptySlot(out ESlot slot)
    {
        
        
        if (m_Slot_1 == null) {slot = ESlot.Slot_1; return true; }
        if (m_Slot_2 == null) { slot = ESlot.Slot_2; return true; }
        if (m_Slot_3 == null) { slot = ESlot.Slot_3; return true; }
        if (m_Slot_4 == null) { slot = ESlot.Slot_4; return true; }

        slot = 0; 
        return false; 
    }

    void RefreshStuffUI()
    {
        switch (m_InventoryType)
        {
            case EInventoryType.Loadout:
                ShowLoadoutUI();
                break;
            case EInventoryType.Equipped:
                ShowEquippedUI();
                break;
            default:
                break;
        }
    }

    void CloseItemSelection()
    {
        Debug.Log("close item selection from playerloadout");
        
        LocalUI.Instance.CloseSelection();
        RefreshStuffUI(); 
    }

    public void SwitchStuffUI(EInventoryType inventoryType)
    {
        m_InventoryType = inventoryType;

        RefreshStuffUI(); 

    }

    void ShowLoadoutUI()
    {
        Debug.Log("show loadout ui"); 
        Dictionary<ESlot, SO_Item> items = new Dictionary<ESlot, SO_Item>
        {
            {ESlot.MainWeapon, m_MainWeapon},
            {ESlot.SecondaryWeapon, m_SecondaryWeapon},
            {ESlot.Slot_1, m_Slot_1},
            {ESlot.Slot_2, m_Slot_2},
            {ESlot.Slot_3, m_Slot_3},
            {ESlot.Slot_4, m_Slot_4},
            {ESlot.Helmet, m_Helmet},
            {ESlot.Chest, m_Chest},
            {ESlot.Shoulders, m_Shoulders},

        };

        LocalUI.Instance.RefreshInventoryUI(items, m_InventoryType);
    }
    void ShowEquippedUI()
    {
        Debug.Log("show Equipped ui");
        Dictionary<ESlot, SO_Item> items = new Dictionary<ESlot, SO_Item>
        {
            {ESlot.MainWeapon, GetEquippedSO_Weapon(ESlot.MainWeapon)},
            {ESlot.SecondaryWeapon,GetEquippedSO_Weapon(ESlot.SecondaryWeapon)},
            {ESlot.Slot_1, GetEquippedSO_Item(ESlot.Slot_1)},
            {ESlot.Slot_2, GetEquippedSO_Item(ESlot.Slot_2)},
            {ESlot.Slot_3, GetEquippedSO_Item(ESlot.Slot_3)},
            {ESlot.Slot_4, GetEquippedSO_Item(ESlot.Slot_4)},
            {ESlot.Helmet, GetEquippedSO_Armor(ESlot.Helmet)},
            {ESlot.Chest, GetEquippedSO_Armor(ESlot.Chest)},
            {ESlot.Shoulders, GetEquippedSO_Armor(ESlot.Shoulders)},
        };

        LocalUI.Instance.RefreshInventoryUI(items,m_InventoryType); 
    }

    SO_Item GetEquippedSO_Item(ESlot slot) => m_EquippedItems[slot] == null ? null : m_EquippedItems[slot].SO_Item;
    SO_Item GetEquippedSO_Weapon(ESlot slot) => m_EquippedWeapons[slot] == null ? null : m_EquippedWeapons[slot].SO_Item;

    SO_Item GetEquippedSO_Armor(ESlot slot) => m_EquippedArmor[slot] == null ? null : m_EquippedArmor[slot].SO_Item;


    public void EquipLoadout()
    {
        StartCoroutine(DelayedEquipLoadout());

    }

    IEnumerator DelayedEquipLoadout()
    {
        yield return new WaitForSeconds(0.1f);
        //yield return new WaitUntil(() => );
        
        //weapons
        if (m_MainWeapon != null) m_EquippedWeapons[ESlot.MainWeapon] = Instantiate(m_MainWeapon.Usable_GameObject, m_PlayerRightHandSocket).GetComponent<BasicWeapon>();
        if (m_SecondaryWeapon != null) m_EquippedWeapons[ESlot.SecondaryWeapon] = Instantiate(m_SecondaryWeapon.Usable_GameObject, m_PlayerLeftHandSocket).GetComponent<BasicWeapon>();
        //inventory
        if (m_Slot_1 != null) m_EquippedItems[ESlot.Slot_1] = Instantiate(m_Slot_1.Usable_GameObject, m_PlayerPocket).GetComponent<InventoryItem>();
        if (m_Slot_2 != null) m_EquippedItems[ESlot.Slot_2] = Instantiate(m_Slot_2.Usable_GameObject, m_PlayerPocket).GetComponent<InventoryItem>();
        if (m_Slot_3 != null) m_EquippedItems[ESlot.Slot_3] = Instantiate(m_Slot_3.Usable_GameObject, m_PlayerPocket).GetComponent<InventoryItem>();
        if (m_Slot_4 != null) m_EquippedItems[ESlot.Slot_4] = Instantiate(m_Slot_4.Usable_GameObject, m_PlayerPocket).GetComponent<InventoryItem>();
        //armors
        if (m_Helmet != null) m_EquippedArmor[ESlot.Helmet] = Instantiate(m_Helmet.Usable_GameObject, m_HelmetSocket).GetComponent<Armor>();
        if (m_Chest != null) m_EquippedArmor[ESlot.Chest] = Instantiate(m_Chest.Usable_GameObject, m_ChestSocket).GetComponent<Armor>();
        if (m_Shoulders != null)
        {
            m_EquippedArmor[ESlot.Shoulders] = Instantiate(m_Shoulders.Usable_GameObject, m_LeftShoulderSocket).GetComponent<Armor>();
            if (m_EquippedArmor[ESlot.Shoulders].TryGetComponent<Shoulders_Armor>(out Shoulders_Armor shoulders))
            {
                EquipShoulders(shoulders);

            }
        }



        SwitchStuffUI(EInventoryType.Equipped);
        SetupItems();
    }

    void EquipShoulders(Shoulders_Armor shoulders)
    {
        shoulders.InstantiatedLeftShoulder =  Instantiate(shoulders.LeftShoulder, m_LeftShoulderSocket);
        shoulders.InstantiatedLeftShoulder.transform.localPosition = Vector3.zero;
        shoulders.InstantiatedLeftShoulder.transform.localRotation = Quaternion.identity;
        shoulders.InstantiatedRightShoulder =  Instantiate(shoulders.RightShoulder, m_RightShoulderSocket);
        shoulders.InstantiatedRightShoulder.transform.localPosition = Vector3.zero;
        shoulders.InstantiatedRightShoulder.transform.localRotation = Quaternion.identity;

    }

    public void UnloadEquippedStuff()
    {
        StartCoroutine(DelayedUnloadEquippedStuff());
    }

    IEnumerator DelayedUnloadEquippedStuff()
    {

        yield return new WaitForSeconds(0.1f);

        List<GameObject> toBin = new List<GameObject>();

        if (m_EquippedWeapons[ESlot.MainWeapon] != null) { toBin.Add(m_EquippedWeapons[ESlot.MainWeapon].gameObject); m_EquippedWeapons[ESlot.MainWeapon] = null; }
        if (m_EquippedWeapons[ESlot.SecondaryWeapon] != null) { toBin.Add(m_EquippedWeapons[ESlot.SecondaryWeapon].gameObject); m_EquippedWeapons[ESlot.SecondaryWeapon] = null; }
        if (m_EquippedItems[ESlot.Slot_1] != null) { toBin.Add(m_EquippedItems[ESlot.Slot_1].gameObject); m_EquippedItems[ESlot.Slot_1] = null; }
        if (m_EquippedItems[ESlot.Slot_2] != null) { toBin.Add(m_EquippedItems[ESlot.Slot_2].gameObject); m_EquippedItems[ESlot.Slot_2] = null; }
        if (m_EquippedItems[ESlot.Slot_3] != null) { toBin.Add(m_EquippedItems[ESlot.Slot_3].gameObject); m_EquippedItems[ESlot.Slot_3] = null; }
        if (m_EquippedItems[ESlot.Slot_4] != null) { toBin.Add(m_EquippedItems[ESlot.Slot_4].gameObject); m_EquippedItems[ESlot.Slot_4] = null; }
        if (m_EquippedArmor[ESlot.Helmet] != null) { toBin.Add(m_EquippedArmor[ESlot.Helmet].gameObject); m_EquippedArmor[ESlot.Helmet] = null; }
        if (m_EquippedArmor[ESlot.Chest] != null) { toBin.Add(m_EquippedArmor[ESlot.Chest].gameObject); m_EquippedArmor[ESlot.Chest] = null; }
        if (m_EquippedArmor[ESlot.Shoulders] != null)
        {

            if (m_EquippedArmor[ESlot.Shoulders].TryGetComponent<Shoulders_Armor>(out Shoulders_Armor shoulders))
            {
                toBin.Add(shoulders.InstantiatedLeftShoulder.gameObject);
                toBin.Add(shoulders.InstantiatedRightShoulder.gameObject);
            }

            toBin.Add(m_EquippedArmor[ESlot.Shoulders].gameObject);
            m_EquippedArmor[ESlot.Shoulders] = null;
        }
        SwitchStuffUI(EInventoryType.Loadout);

        yield return new WaitForSeconds(0.1f);

        foreach (var item in toBin)
        {
            Destroy(item);
        }
    }

    public void ClearLoadout()
    {
        m_MainWeapon = null;
        m_SecondaryWeapon = null;
        m_Slot_1 = null;
        m_Slot_2 = null;
        m_Slot_3 = null;
        m_Slot_4 = null;
        m_Helmet = null;
        m_Chest = null;
        m_Shoulders = null;

        RefreshStuffUI();

    }

    void SetupItems()
    {
        foreach (var item in m_EquippedItems)
        {
            if (item.Value == null) continue;
            item.Value.SetupItem();
        }
    }
    




}
