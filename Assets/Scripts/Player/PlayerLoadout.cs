using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ESlotToGrabbableDictionary : SerializableDictionary<EStuffSlot, Grabbable> { }
[Serializable]
public class EslotToSoItemDictionary : SerializableDictionary<EStuffSlot, SO_Item> { }

public class EslotToSocket : SerializableDictionary<EStuffSlot, Transform> { }

public class PlayerLoadout : MonoBehaviour
{
    
    

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

    
    EslotToSocket m_EslotToSocket = new EslotToSocket();

    private void Awake()
    {

        m_TinyPlayer = GetComponent<TinyPlayer>();
        //
        m_EquippedItems.Add(EStuffSlot.MainWeapon, null);
        m_EquippedItems.Add(EStuffSlot.SecondaryWeapon, null);
        m_EquippedItems.Add(EStuffSlot.Slot_1, null); 
        m_EquippedItems.Add(EStuffSlot.Slot_2, null);
        m_EquippedItems.Add(EStuffSlot.Slot_3, null);
        m_EquippedItems.Add(EStuffSlot.Slot_4, null);
        m_EquippedItems.Add(EStuffSlot.Helmet, null);
        m_EquippedItems.Add(EStuffSlot.Chest, null);
        m_EquippedItems.Add(EStuffSlot.Shoulders, null);
        //
        m_LoadoutItems.Add(EStuffSlot.MainWeapon, null);
        m_LoadoutItems.Add(EStuffSlot.SecondaryWeapon, null);
        m_LoadoutItems.Add(EStuffSlot.Slot_1, null);
        m_LoadoutItems.Add(EStuffSlot.Slot_2, null);
        m_LoadoutItems.Add(EStuffSlot.Slot_3, null);
        m_LoadoutItems.Add(EStuffSlot.Slot_4, null);
        m_LoadoutItems.Add(EStuffSlot.Helmet, null);
        m_LoadoutItems.Add(EStuffSlot.Chest, null);
        m_LoadoutItems.Add(EStuffSlot.Shoulders, null);
        //
        m_EslotToSocket.Add(EStuffSlot.MainWeapon, m_PlayerRightHandSocket);
        m_EslotToSocket.Add(EStuffSlot.SecondaryWeapon, m_PlayerLeftHandSocket);
        m_EslotToSocket.Add(EStuffSlot.Slot_1, m_PlayerPocket);
        m_EslotToSocket.Add(EStuffSlot.Slot_2, m_PlayerPocket);
        m_EslotToSocket.Add(EStuffSlot.Slot_3, m_PlayerPocket);
        m_EslotToSocket.Add(EStuffSlot.Slot_4, m_PlayerPocket);
        m_EslotToSocket.Add(EStuffSlot.Helmet, m_HelmetSocket);
        m_EslotToSocket.Add(EStuffSlot.Chest, m_ChestSocket);
        m_EslotToSocket.Add(EStuffSlot.Shoulders, m_ChestSocket);


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

            EquipWeapon(weapon, out EStuffSlot hand);
            Debug.Log("weapon equipped, hand is : " + hand); 

            m_EquippedItems[hand].m_Rigidbody.isKinematic = true;
            m_EquippedItems[hand].m_Collider.enabled = false;
            m_EquippedItems[hand].transform.SetParent(m_EslotToSocket[hand], false);

            m_EquippedItems[hand].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            //m_SelectedSlot = ESlot.MainWeapon;
            //SelectSlot(ESlot.MainWeapon);

            LocalUI.Instance.ChangeSlotIcon(hand, m_EquippedItems[hand].SO_Item.ItemIcon);



            return;
        }
        else if (item is Armor)
        {
            Debug.Log("equip armor of type armor "); 
            Armor armor = (Armor)item;
            EquipArmor(armor,out EStuffSlot slot, out Transform socket);
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
            EquipInventoryItem((InventoryItem)item, out EStuffSlot slot);

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
        DropItemOnSlot(EStuffSlot.MainWeapon, throwForce);

        if (m_EquippedItems[EStuffSlot.MainWeapon] != null)
        {
            DropItemOnSlot(EStuffSlot.MainWeapon, throwForce);
            return; 
        }
        if (m_EquippedItems[EStuffSlot.SecondaryWeapon] != null)
        {
            DropItemOnSlot(EStuffSlot.SecondaryWeapon, throwForce);
        }

    }

    public void DropWeapons(float throwForce = 5f)
    {
        if(m_EquippedItems[EStuffSlot.MainWeapon] != null)
        {
            DropWeapon(m_EquippedItems[EStuffSlot.MainWeapon], throwForce);
            ClearOnSlot(EStuffSlot.MainWeapon);
        }
        if (m_EquippedItems[EStuffSlot.SecondaryWeapon] != null)
        {
            DropWeapon(m_EquippedItems[EStuffSlot.SecondaryWeapon], throwForce);
            ClearOnSlot(EStuffSlot.SecondaryWeapon);
        }
    }
    void DropItemOnSlot(int slot, float throwForce = 5f)
    {
        if (m_EquippedItems[(EStuffSlot)slot] == null) return;


        if (m_EquippedItems[(EStuffSlot)slot] is BasicWeapon)
        {
            DropWeapon(m_EquippedItems[(EStuffSlot)slot], throwForce);
        }
        else
        {
            DropItem(m_EquippedItems[(EStuffSlot)slot]);
        }
        ClearOnSlot((EStuffSlot)slot);

    }

    void DropItemOnSlot(EStuffSlot slot, float throwForce = 5f)
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

        if(weapon.WeaponSize == EWeaponSize.Two_Handed)
        {
            m_LoadoutItems[EStuffSlot.MainWeapon] = weapon;
            m_LoadoutItems[EStuffSlot.SecondaryWeapon] = null;

        }
        else if (weapon.WeaponSize == EWeaponSize.Right_Handed)
        {
            m_LoadoutItems[EStuffSlot.MainWeapon] = weapon;
        }
        else if (weapon.WeaponSize == EWeaponSize.Left_Handed)
        {
            if (m_LoadoutItems[EStuffSlot.MainWeapon] != null && ((SO_Weapon)m_LoadoutItems[EStuffSlot.MainWeapon]).WeaponSize == EWeaponSize.Two_Handed) m_LoadoutItems[EStuffSlot.MainWeapon] = null;
            m_LoadoutItems[EStuffSlot.SecondaryWeapon] = weapon; 
        }

    }

    public void EquipArmorInLoadout(SO_Armor armor)
    {
        
        switch (armor.ArmorPlace)
        {
            case EArmorPlace.Helmet:
                m_LoadoutItems[EStuffSlot.Helmet] = armor;
                break;
            case EArmorPlace.Chest:
                m_LoadoutItems[EStuffSlot.Chest] = armor;
                break;
            case EArmorPlace.Shoulders:
                m_LoadoutItems[EStuffSlot.Shoulders] = armor;
                break;
            default:
                break;
        }
    }

    void EquipInventoryItemInLoadout(SO_Item item)
    {
        //find place and put in slot
        Debug.Log("equip inventory item in loadout");

        if(TryFindEmptyLoadoutSlot(out EStuffSlot slot))
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

        m_LoadoutItems[(EStuffSlot)slot] = m_StandByItem;
      

        CloseItemSelection();
    }

    void EquipWeapon(BasicWeapon weapon, out EStuffSlot hand)
    {
        hand = EStuffSlot.MainWeapon;

        
        
        if (weapon.WeaponSize == EWeaponSize.Two_Handed)
        {
            if (m_EquippedItems[EStuffSlot.MainWeapon] != null || m_EquippedItems[EStuffSlot.SecondaryWeapon] != null)
            {
                DropWeapons();
            }
            m_EquippedItems[EStuffSlot.MainWeapon]= weapon;
            m_EquippedItems[EStuffSlot.SecondaryWeapon]= null;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(true);

        }

        else if (weapon.WeaponSize == EWeaponSize.Left_Handed)
        {
            Debug.Log("equipping left handed weapon");

            if ((m_EquippedItems[EStuffSlot.MainWeapon] != null) &&
                m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>().WeaponSize == EWeaponSize.Two_Handed)
            {
                DropWeapons();

            }
            else
            {
                DropItemOnSlot(EStuffSlot.SecondaryWeapon);
            }


            m_EquippedItems[EStuffSlot.SecondaryWeapon] = weapon;
            hand = EStuffSlot.SecondaryWeapon;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(false);
            
        }
        else if(weapon.WeaponSize == EWeaponSize.Right_Handed)
        {
            DropItemOnSlot(EStuffSlot.MainWeapon);
            m_EquippedItems[EStuffSlot.MainWeapon] = weapon;
            m_TinyPlayer.m_PlayerWeapons.SetTwoHanded(false);
        }

        m_TinyPlayer.m_PlayerWeapons.SetWeaponParameters(weapon);


    }

    void EquipArmor(Armor armor, out EStuffSlot slot, out Transform socket)
    {
        slot = EStuffSlot.Helmet; 
        socket = null;

       
       if (armor is Head_Armor)
        {
            slot = EStuffSlot.Helmet;
            if (m_EquippedItems[slot] != null) DropItemOnSlot(slot);
            m_EquippedItems[EStuffSlot.Helmet] = armor;
            socket = m_HelmetSocket;
        }
        else if (armor is Chest_Armor)
        {
            slot = EStuffSlot.Chest;
            if (m_EquippedItems[slot] != null) DropItemOnSlot(slot);
            m_EquippedItems[EStuffSlot.Chest] = armor;
            socket = m_ChestSocket;
        }
        else if( armor is Shoulders_Armor)
        {
            Debug.Log("equipping shoulders"); 
            slot= EStuffSlot.Shoulders;
            if (m_EquippedItems[slot] != null)
            {
                m_EquippedItems[slot].GetComponent<Shoulders_Armor>().ShowVisuals(true);
                DropItemOnSlot(slot);

            }
            m_EquippedItems[EStuffSlot.Shoulders] = armor;
            socket = m_LeftShoulderSocket;
        }

    }

    void EquipInventoryItem(InventoryItem item, out EStuffSlot slot)
    {
        if (TryFindEmptyEquippedSlot(out slot))
        {
            m_EquippedItems[slot] = item;

        }

    }
    public Grabbable GetGrabbableInHand()
    {
        return m_EquippedItems[EStuffSlot.MainWeapon];    
    }

    void ClearOnSlot(EStuffSlot slot)
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

    public void SlotActionPerformed(EStuffSlot slot)
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

    bool TryFindEmptyLoadoutSlot(out EStuffSlot slot)
    {
        
        
        if (m_LoadoutItems[EStuffSlot.Slot_1] == null) {slot = EStuffSlot.Slot_1; return true; }
        if (m_LoadoutItems[EStuffSlot.Slot_2] == null) { slot = EStuffSlot.Slot_2; return true; }
        if (m_LoadoutItems[EStuffSlot.Slot_3] == null) { slot = EStuffSlot.Slot_3; return true; }
        if (m_LoadoutItems[EStuffSlot.Slot_4] == null) { slot = EStuffSlot.Slot_4; return true; }

        slot = 0; 
        return false; 
    }

    bool TryFindEmptyEquippedSlot(out EStuffSlot slot)
    {


        if (m_EquippedItems[EStuffSlot.Slot_1] == null) { slot = EStuffSlot.Slot_1; return true; }
        if (m_EquippedItems[EStuffSlot.Slot_2] == null) { slot = EStuffSlot.Slot_2; return true; }
        if (m_EquippedItems[EStuffSlot.Slot_3] == null) { slot = EStuffSlot.Slot_3; return true; }
        if (m_EquippedItems[EStuffSlot.Slot_4] == null) { slot = EStuffSlot.Slot_4; return true; }

        slot = EStuffSlot.Slot_1;
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
        Dictionary<EStuffSlot, SO_Item> items = new Dictionary<EStuffSlot, SO_Item>();

        for (int i = 0; i < m_EquippedItems.Count; i++)
        {
            items.Add((EStuffSlot)i, m_EquippedItems[(EStuffSlot)i] == null ? null :  m_EquippedItems[(EStuffSlot)i].SO_Item);
        }

        LocalUI.Instance.RefreshInventoryUI(items, m_InventoryType);

    }

 


    public void EquipLoadout()
    {
        StartCoroutine(DelayedEquipLoadout());

    }

    void EquipLoadoutSlot(EStuffSlot slot)
    {
        Transform socket = m_PlayerPocket;

        socket = m_EslotToSocket[slot];
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
            EquipLoadoutSlot((EStuffSlot)i);
        }


        SwitchStuffUI(EInventoryType.Equipped);
        SetupItems();
    }

    void EquipShoulders(Shoulders_Armor shoulders)
    {
        shoulders.InstantiatedLeftShoulder =  Instantiate(shoulders.m_LeftShoulder, m_LeftShoulderSocket);
        shoulders.InstantiatedLeftShoulder.transform.localPosition = Vector3.zero;
        shoulders.InstantiatedLeftShoulder.transform.localRotation = Quaternion.identity;
        shoulders.InstantiatedRightShoulder =  Instantiate(shoulders.m_RightShoulder, m_RightShoulderSocket);
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
            UnloadCheckEquippedItemOnSlot((EStuffSlot)i); 
        }


        SwitchStuffUI(EInventoryType.Loadout);

        yield return new WaitForSeconds(0.1f);

        Transform[] sockets = {m_ChestSocket, m_HelmetSocket, m_LeftShoulderSocket, m_RightShoulderSocket, m_PlayerLeftHandSocket, m_PlayerRightHandSocket, m_PlayerPocket }; 
        CheckAndDestroyInSockets(sockets);

        m_TinyPlayer.m_PlayerWeapons.SetWeaponsNeutralState();

    }
    void UnloadCheckEquippedItemOnSlot(EStuffSlot slot)
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
            m_LoadoutItems[(EStuffSlot)i] = null; 
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

    public int DamageReduction(int damage)
    {
        float armor = 0; 
        if (m_EquippedItems[EStuffSlot.Chest] != null)
        {
            armor += ((Armor)m_EquippedItems[EStuffSlot.Chest]).ArmorParameters.Armor;

        }
        if (m_EquippedItems[EStuffSlot.Helmet] != null)
        {

            armor += ((Armor)m_EquippedItems[EStuffSlot.Helmet]).ArmorParameters.Armor;
        }
        if (m_EquippedItems[EStuffSlot.Shoulders] != null)
        {

            armor += ((Armor)m_EquippedItems[EStuffSlot.Shoulders]).ArmorParameters.Armor;
        }

        float damageReduction = 100 / (100 + armor);
        Debug.Log("damage reduction  is " + damageReduction);
        damage = (int)((float)damage * damageReduction);
        Debug.Log("damage calculated in reduction is " + damage);


        return damage; 
    }
    




}
