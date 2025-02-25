using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ESlotToGrabbableDictionary : SerializableDictionary<PlayerLoadout.ESlot, Grabbable> { }
[Serializable]
public class EslotToSoItemDictionary : SerializableDictionary<PlayerLoadout.ESlot, SO_Item> { }

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
    [SerializeField]
    public EslotToSoItemDictionary m_LoadoutItems = new EslotToSoItemDictionary();
    [Space(5)]
    [SerializeField] SO_Item m_StandByItem;

    [Space(10)]
    [Header("Equipped Items")]

    //[SerializeField] ESlot m_SelectedSlot = ESlot.MainWeapon;

    [Space(5)]


    [SerializeField]
    public ESlotToGrabbableDictionary m_EquippedItems = new ESlotToGrabbableDictionary();

    



    [Space(10)]
    [Header("Player sockets")]
    [SerializeField] internal Transform m_PlayerLeftHandSocket;
    [SerializeField] internal Transform m_PlayerRightHandSocket;
    [SerializeField] internal Transform m_PlayerPocket;
    [SerializeField] internal Transform m_DropSocket;
    [Header("Player Armor sockets")]
    [SerializeField] internal Transform m_HelmetSocket;
    [SerializeField] internal Transform m_ChestSocket;
    [SerializeField] internal Transform m_LeftShoulderSocket;
    [SerializeField] internal Transform m_RightShoulderSocket;


    private void Awake()
    {

        m_TinyPlayer = GetComponent<TinyPlayer>();

        m_EquippedItems.Add(ESlot.MainWeapon, null);
        m_EquippedItems.Add(ESlot.SecondaryWeapon, null);
       
        m_EquippedItems.Add(ESlot.Slot_1, null); 
        m_EquippedItems.Add(ESlot.Slot_2, null);
        m_EquippedItems.Add(ESlot.Slot_3, null);
        m_EquippedItems.Add(ESlot.Slot_4, null);
        //
        m_EquippedItems.Add(ESlot.Helmet, null);
        m_EquippedItems.Add(ESlot.Chest, null);
        m_EquippedItems.Add(ESlot.Shoulders, null);
        //
        //SelectSlot(m_SelectedSlot);

        //
        m_LoadoutItems.Add(ESlot.MainWeapon, null);
        m_LoadoutItems.Add(ESlot.SecondaryWeapon, null);
        m_LoadoutItems.Add(ESlot.Slot_1, null);
        m_LoadoutItems.Add(ESlot.Slot_2, null);
        m_LoadoutItems.Add(ESlot.Slot_3, null);
        m_LoadoutItems.Add(ESlot.Slot_4, null);
        m_LoadoutItems.Add(ESlot.Helmet, null);
        m_LoadoutItems.Add(ESlot.Chest, null);
        m_LoadoutItems.Add(ESlot.Shoulders, null);

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


        if (item is BasicWeapon)
        {

            BasicWeapon weapon = (BasicWeapon)item;

            EquipWeapon(weapon, out ESlot hand);

            m_EquippedItems[hand].m_Rigidbody.isKinematic = true;
            m_EquippedItems[hand].m_Collider.enabled = false;
            m_EquippedItems[hand].transform.SetParent(m_PlayerRightHandSocket, false);

            m_EquippedItems[hand].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            //m_SelectedSlot = ESlot.MainWeapon;
            //SelectSlot(ESlot.MainWeapon);

            LocalUI.Instance.ChangeSlotIcon(ESlot.MainWeapon, m_EquippedItems[hand].SO_Item.ItemIcon);



            return;
        }
        else if (item is Armor)
        {
            Debug.Log("equip armor of type armor "); 
            Armor armor = (Armor)item;
            EquipArmor(armor,out ESlot slot, out Transform socket);
            if(armor is Shoulders_Armor)
            {
                Shoulders_Armor shoulders = (Shoulders_Armor)armor;

                EquipShoulders(shoulders);

                shoulders.ShowVisuals(false);


            }
            m_EquippedItems[slot].m_Rigidbody.isKinematic = true;
            m_EquippedItems[slot].m_Collider.enabled = false;
            m_EquippedItems[slot].transform.SetParent(socket, false);
            m_EquippedItems[slot].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);



            LocalUI.Instance.ChangeSlotIcon(slot, m_EquippedItems[slot].SO_Item.ItemIcon);

        }
        else if (item is InventoryItem)
        {
            Debug.Log("equip armor of type invetory item ");
            EquipInventoryItem((InventoryItem)item, out ESlot slot);

            m_EquippedItems[slot].m_Rigidbody.isKinematic = true;
            m_EquippedItems[slot].m_Collider.enabled = false;
            m_EquippedItems[slot].transform.SetParent(m_PlayerPocket, false);
            m_EquippedItems[slot].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            LocalUI.Instance.ChangeSlotIcon(slot, m_EquippedItems[slot].SO_Item.ItemIcon);

        }



    }

    public void DropItemInHand(float throwForce = 0f)
    {
        Debug.Log("drop item in hand"); 
        DropItemOnSlot(ESlot.MainWeapon, throwForce);

        if (m_EquippedItems[ESlot.MainWeapon] != null)
        {
            DropItemOnSlot(ESlot.MainWeapon, throwForce);
            return; 
        }
        if (m_EquippedItems[ESlot.SecondaryWeapon] != null)
        {
            DropItemOnSlot(ESlot.SecondaryWeapon, throwForce);
        }

    }

    public void DropWeapons(float throwForce = 5f)
    {
        if(m_EquippedItems[ESlot.MainWeapon] != null)
        {
            DropWeapon(m_EquippedItems[ESlot.MainWeapon], throwForce);
            ClearOnSlot(ESlot.MainWeapon);
        }
        if (m_EquippedItems[ESlot.SecondaryWeapon] != null)
        {
            DropWeapon(m_EquippedItems[ESlot.SecondaryWeapon], throwForce);
            ClearOnSlot(ESlot.SecondaryWeapon);
        }
    }
    void DropItemOnSlot(int slot, float throwForce = 5f)
    {
        if (m_EquippedItems[(ESlot)slot] == null) return;


        if (m_EquippedItems[(ESlot)slot] is BasicWeapon)
        {
            DropWeapon(m_EquippedItems[(ESlot)slot], throwForce);
        }
        else
        {
            DropItem(m_EquippedItems[(ESlot)slot]);
        }
        ClearOnSlot((ESlot)slot);

    }

    void DropItemOnSlot(ESlot slot, float throwForce = 5f)
    {
        Debug.Log("drop item on slot");
        if (m_EquippedItems[slot] == null) return;

        if (m_EquippedItems[slot] is BasicWeapon)
        {
            DropWeapon(m_EquippedItems[slot], throwForce);
        }
        else
        {
            DropItem(m_EquippedItems[slot]); 
        }



        ClearOnSlot(slot);
    }

    void DropWeapon(Grabbable item, float throwForce = 5f)
    {
        Debug.Log("dropping item"); 
        item.transform.SetParent(null, true);
        item.m_Rigidbody.isKinematic = false;
        item.m_Rigidbody.AddForce(throwForce * transform.forward, ForceMode.VelocityChange);
        item.m_Rigidbody.AddTorque(-transform.right * 1f, ForceMode.VelocityChange);
        item.Release(); 
        item.m_Collider.enabled = true;
    }

    void DropItem(Grabbable item)
    {
        Debug.Log("dropping item");
        item.transform.SetParent(null, true);
        item.transform.position = m_DropSocket.position;    
        item.m_Rigidbody.isKinematic = false;
        item.m_Rigidbody.AddForce(transform.forward, ForceMode.VelocityChange);
        item.m_Rigidbody.AddTorque(-transform.right * 1f, ForceMode.VelocityChange);
        item.Release();
        item.m_Collider.enabled = true;
    }

    void EquipWeaponLoadout(SO_Weapon weapon)
    {

        if(weapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed)
        {
            m_LoadoutItems[ESlot.MainWeapon] = weapon;
            m_LoadoutItems[ESlot.SecondaryWeapon] = null;

        }
        else if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Right_Handed)
        {
            m_LoadoutItems[ESlot.MainWeapon] = weapon;
        }
        else if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Left_Handed)
        {
            if (m_LoadoutItems[ESlot.MainWeapon] != null && ((SO_Weapon)m_LoadoutItems[ESlot.MainWeapon]).WeaponSize == SO_Weapon.EWeaponSize.Two_Handed) m_LoadoutItems[ESlot.MainWeapon] = null;
            m_LoadoutItems[ESlot.SecondaryWeapon] = weapon; 
        }

    }

    public void EquipArmorInLoadout(SO_Armor armor)
    {

        switch (armor.ArmorPLace)
        {
            case SO_Armor.EArmorPlace.Helmet:
                m_LoadoutItems[ESlot.Helmet] = armor;
                break;
            case SO_Armor.EArmorPlace.Chest:
                m_LoadoutItems[ESlot.Chest] = armor;
                break;
            case SO_Armor.EArmorPlace.Shoulders:
                m_LoadoutItems[ESlot.Shoulders] = armor;
                break;
            default:
                break;
        }
    }

    void EquipInventoryItemInLoadout(SO_Item item)
    {
        //find place and put in slot
        Debug.Log("equip inventory item in loadout");

        if(TryFindEmptyLoadoutSlot(out ESlot slot))
        {
            m_LoadoutItems[slot] = item;

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

        m_LoadoutItems[(ESlot)slot] = m_StandByItem;
      

        CloseItemSelection();
    }

    void EquipWeapon(BasicWeapon weapon, out ESlot hand)
    {
        hand = ESlot.MainWeapon;

        
        
        if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed)
        {
            if (m_EquippedItems[ESlot.MainWeapon] != null || m_EquippedItems[ESlot.SecondaryWeapon] != null)
            {
                DropWeapons();
            }
            m_EquippedItems[ESlot.MainWeapon]= weapon;
            m_EquippedItems[ESlot.SecondaryWeapon]= null;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(true);

        }

        else if (weapon.WeaponSize == SO_Weapon.EWeaponSize.Left_Handed)
        {
            DropItemOnSlot(ESlot.SecondaryWeapon);
            //if (m_MainWeapon.WeaponSize == SO_Weapon.EWeaponSize.Two_Handed) m_MainWeapon = null;
            if ((m_EquippedItems[ESlot.MainWeapon] != null) &&
                m_EquippedItems[ESlot.MainWeapon].GetComponent<BasicWeapon>().WeaponSize == SO_Weapon.EWeaponSize.Two_Handed) m_EquippedItems[ESlot.MainWeapon] = null;
            m_EquippedItems[ESlot.SecondaryWeapon] = weapon;
            hand = ESlot.SecondaryWeapon;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(false);
            
        }
        else if(weapon.WeaponSize == SO_Weapon.EWeaponSize.Right_Handed)
        {
            DropItemOnSlot(ESlot.MainWeapon);
            m_EquippedItems[ESlot.MainWeapon] = weapon;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(false);
        }

        m_TinyPlayer.m_PlayerWeapons.SetWeaponParameters(weapon);


    }

    void EquipArmor(Armor armor, out ESlot slot, out Transform socket)
    {
        slot = ESlot.Helmet; 
        socket = null;

       
       if (armor is Head_Armor)
        {
            slot = ESlot.Helmet;
            if (m_EquippedItems[slot] != null) DropItemOnSlot(slot);
            m_EquippedItems[ESlot.Helmet] = armor;
            socket = m_HelmetSocket;
        }
        else if (armor is Chest_Armor)
        {
            slot = ESlot.Chest;
            if (m_EquippedItems[slot] != null) DropItemOnSlot(slot);
            m_EquippedItems[ESlot.Chest] = armor;
            socket = m_ChestSocket;
        }
        else if( armor is Shoulders_Armor)
        {
            Debug.Log("equipping shoulders"); 
            slot= ESlot.Shoulders;
            if (m_EquippedItems[slot] != null)
            {
                m_EquippedItems[slot].GetComponent<Shoulders_Armor>().ShowVisuals(true);
                DropItemOnSlot(slot);

            }
            m_EquippedItems[ESlot.Shoulders] = armor;
            socket = m_LeftShoulderSocket;
        }

    }

    void EquipInventoryItem(InventoryItem item, out ESlot slot)
    {
        if (TryFindEmptyEquippedSlot(out slot))
        {
            m_EquippedItems[slot] = item;

        }

    }
    public Grabbable GetGrabbableInHand()
    {
        return m_EquippedItems[ESlot.MainWeapon];    
    }

    void ClearOnSlot(ESlot slot)
    {
        LocalUI.Instance.ClearSlot(slot);

        m_EquippedItems[slot] = null;

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
        if(m_EquippedItems[slot].TryGetComponent<InventoryItem>(out InventoryItem invItem))
        {
            invItem.UseInventoryItem();
        }
        else
        {
            Debug.Log("this is not an inventory item "); 
        }
        

        
    }

    bool TryFindEmptyLoadoutSlot(out ESlot slot)
    {
        
        
        if (m_LoadoutItems[ESlot.Slot_1] == null) {slot = ESlot.Slot_1; return true; }
        if (m_LoadoutItems[ESlot.Slot_2] == null) { slot = ESlot.Slot_2; return true; }
        if (m_LoadoutItems[ESlot.Slot_3] == null) { slot = ESlot.Slot_3; return true; }
        if (m_LoadoutItems[ESlot.Slot_4] == null) { slot = ESlot.Slot_4; return true; }

        slot = 0; 
        return false; 
    }

    bool TryFindEmptyEquippedSlot(out ESlot slot)
    {


        if (m_EquippedItems[ESlot.Slot_1] == null) { slot = ESlot.Slot_1; return true; }
        if (m_EquippedItems[ESlot.Slot_2] == null) { slot = ESlot.Slot_2; return true; }
        if (m_EquippedItems[ESlot.Slot_3] == null) { slot = ESlot.Slot_3; return true; }
        if (m_EquippedItems[ESlot.Slot_4] == null) { slot = ESlot.Slot_4; return true; }

        slot = ESlot.Slot_1;
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

        LocalUI.Instance.RefreshInventoryUI(m_LoadoutItems, m_InventoryType);
    }
    void ShowEquippedUI()
    {
        Debug.Log("show Equipped ui");
        Dictionary<ESlot, SO_Item> items = new Dictionary<ESlot, SO_Item>
        {
            {ESlot.MainWeapon, GetEquippedSO_Item(ESlot.MainWeapon)},
            {ESlot.SecondaryWeapon,GetEquippedSO_Item(ESlot.SecondaryWeapon)},
            {ESlot.Slot_1, GetEquippedSO_Item(ESlot.Slot_1)},
            {ESlot.Slot_2, GetEquippedSO_Item(ESlot.Slot_2)},
            {ESlot.Slot_3, GetEquippedSO_Item(ESlot.Slot_3)},
            {ESlot.Slot_4, GetEquippedSO_Item(ESlot.Slot_4)},
            {ESlot.Helmet, GetEquippedSO_Item(ESlot.Helmet)},
            {ESlot.Chest, GetEquippedSO_Item(ESlot.Chest)},
            {ESlot.Shoulders, GetEquippedSO_Item(ESlot.Shoulders)},
        };

        LocalUI.Instance.RefreshInventoryUI(items,m_InventoryType); 
    }

    SO_Item GetEquippedSO_Item(ESlot slot) => m_EquippedItems[slot] == null ? null : m_EquippedItems[slot].SO_Item;
 


    public void EquipLoadout()
    {
        StartCoroutine(DelayedEquipLoadout());

    }

    void EquipLoadoutSlot(ESlot slot)
    {
        Transform socket = m_PlayerPocket;
        switch (slot)
        {
            case ESlot.MainWeapon:
                socket = m_PlayerRightHandSocket;
                break;
            case ESlot.SecondaryWeapon:
                socket = m_PlayerLeftHandSocket;
                break;
            case ESlot.Slot_1:
                break;
            case ESlot.Slot_2:
                break;
            case ESlot.Slot_3:
                break;
            case ESlot.Slot_4:
                break;
            case ESlot.Helmet:
                socket = m_HelmetSocket;
                break;
            case ESlot.Chest:
                socket = m_ChestSocket;
                break;
            case ESlot.Shoulders:
                socket = m_LeftShoulderSocket;
                break;
            case ESlot.count:
                break;
            default:
                break;
        }
        if (m_LoadoutItems[slot] != null)
        {
            m_EquippedItems[slot] = Instantiate(m_LoadoutItems[slot].Usable_GameObject, socket).GetComponent<Grabbable>();
            if (m_EquippedItems[slot].TryGetComponent<Shoulders_Armor>(out Shoulders_Armor shoulders))
            {
                EquipShoulders(shoulders);

            }
        }
            
    }

    IEnumerator DelayedEquipLoadout()
    {
        yield return new WaitForSeconds(0.1f);
        //yield return new WaitUntil(() => );

        for (int i = 0; i < m_LoadoutItems.Count; i++)
        {
            EquipLoadoutSlot((ESlot)i);
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


        for (int i = 0; i < m_EquippedItems.Count; i++)
        {
            UnloadCheckEquippedItemOnSlot((ESlot)i); 
        }


        SwitchStuffUI(EInventoryType.Loadout);

        yield return new WaitForSeconds(0.1f);

        Transform[] sockets = {m_ChestSocket, m_HelmetSocket, m_LeftShoulderSocket, m_RightShoulderSocket, m_PlayerLeftHandSocket, m_PlayerRightHandSocket, m_PlayerPocket }; 
        CheckAndDestroyInSockets(sockets);

        m_TinyPlayer.m_PlayerWeapons.SetWeaponsNeutralState();

    }
    void UnloadCheckEquippedItemOnSlot(ESlot slot)
    {

        List<GameObject> toBin = new List<GameObject>();

        if (m_EquippedItems[slot] != null) {

            if (m_EquippedItems[slot].TryGetComponent<Shoulders_Armor>(out Shoulders_Armor shoulders))
            {
                toBin.Add(shoulders.InstantiatedLeftShoulder.gameObject);
                toBin.Add(shoulders.InstantiatedRightShoulder.gameObject);
            }

            toBin.Add(m_EquippedItems[slot].gameObject);
            m_EquippedItems[slot] = null;

            foreach (var item in toBin)
            {
                Debug.Log("destroying equipped :" + item.name);
                Destroy(item);
            }
        }
    }

    void CheckAndDestroyInSockets(Transform[] sockets)
    {
        foreach (Transform socket in sockets)
        {
            if (socket.childCount > 0)
            {
                foreach (object item in socket)
                {
                    if ((GameObject)item != null) Destroy((GameObject)item);

                }
            }
        }
        
    }

    public void ClearLoadout()
    {

        for (int i = 0; i < m_LoadoutItems.Count; i++)
        {
            m_LoadoutItems[(ESlot)i] = null; 
        }


        RefreshStuffUI();

    }

    void SetupItems()
    {
        foreach (var inventoyItem in m_EquippedItems)
        {
            if (inventoyItem.Value == null) continue;
            if (inventoyItem.Value.TryGetComponent<InventoryItem>(out InventoryItem invItem))
            {
                
                invItem.SetupItem();
            }

            if (inventoyItem.Value.TryGetComponent<BasicWeapon>(out BasicWeapon weapon))
            {

                m_TinyPlayer.m_PlayerWeapons.SetWeaponParameters(weapon);
            }

        }


    }
    




}
