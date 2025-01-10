using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESlotToSlotUIDictionary : SerializableDictionary<PlayerLoadout.ESlot, SlotUI> { }

public class LocalUI : MonoBehaviour
{
    public static LocalUI Instance { get; private set; }

    [SerializeField] List<SlotUI> m_InventorySlots;
    [SerializeField] SlotUI m_MainWeaponSlot; 
    [SerializeField] SlotUI m_SecondaryWeaponSlot;

    [SerializeField] SlotUI m_SelectedSlot;

    //Dictionary<PlayerLoadout.ESlot, SlotUI> m_SlotDictionary = new Dictionary<PlayerLoadout.ESlot, SlotUI>();
    ESlotToSlotUIDictionary m_SlotDictionary = new ESlotToSlotUIDictionary();
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        m_SlotDictionary.Add(PlayerLoadout.ESlot.MainWeapon, m_MainWeaponSlot);
        m_SlotDictionary.Add(PlayerLoadout.ESlot.SecondaryWeapon, m_SecondaryWeaponSlot);
        m_SlotDictionary.Add(PlayerLoadout.ESlot.Slot_1, m_InventorySlots[0]);
        m_SlotDictionary.Add(PlayerLoadout.ESlot.Slot_2, m_InventorySlots[1]);
        m_SlotDictionary.Add(PlayerLoadout.ESlot.Slot_3, m_InventorySlots[2]);
        m_SlotDictionary.Add(PlayerLoadout.ESlot.Slot_4, m_InventorySlots[3]);

    }
    
    public void SelectSlot(PlayerLoadout.ESlot slot)
    {
        if(m_SelectedSlot != null) m_SelectedSlot.UnSelectSlot();

        m_SelectedSlot = m_SlotDictionary[slot];
        m_SelectedSlot.SelectSlot();
    }

    public void ChangeSlotIcon(PlayerLoadout.ESlot slot, Sprite Icon)
    {
        m_SlotDictionary[slot].ChangeIcon(Icon);
    }

    public void ClearSlot(PlayerLoadout.ESlot slot)
    {
        m_SlotDictionary[slot].ChangeIcon(null);
    }
    

    

    
}
