using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Coherence.Core.NativeTransport;
using static PlayerLoadout;

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

    [SerializeField] TextMeshProUGUI m_UsableText;

    [Space(10)]
    [Header("Selection Panel")]

    [SerializeField] GameObject m_SelectionPanel;
    [SerializeField] Image m_SelectionFade;

    [Serializable]
    struct FSelectionItem
    {
        public Transform StoreItem_Socket;
        public TextMeshProUGUI SelectionItem_NameText; 
        public TextMeshProUGUI SelectionItem_DescriptionText; 

        public FSelectionItem(Transform Socket, TextMeshProUGUI NameText, TextMeshProUGUI DescriptionText)
        {
            StoreItem_Socket = Socket;
            SelectionItem_NameText = NameText;
            SelectionItem_DescriptionText = DescriptionText;
        }

    }

    [SerializeField] List<FSelectionItem> m_SelectionItems = new List<FSelectionItem>();
    [SerializeField] List<GameObject> m_SelectionItemsGO = new List<GameObject>();
    [SerializeField] List<SO_Item> m_SelectionItemsSO = new List<SO_Item>();
     

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

        m_UsableText.enabled = false;

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
    public void ShowUsable()
    {
        m_UsableText.enabled = true;
    }
    public void ShowUsable(string Text)
    {
        m_UsableText.text = Text;
        m_UsableText.enabled = true;
    }
    public void HideUsable()
    {
        m_UsableText.enabled = false;
    }

    public void CloseSelection()
    {
        m_SelectionPanel.SetActive(false);
        m_SelectionFade.enabled = false;

        for (int i = 0; i < m_SelectionItemsGO.Count; i++)
        {
            Destroy(m_SelectionItemsGO[i]);
        }

        m_SelectionItemsGO.Clear();
        m_SelectionItemsSO.Clear();

        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Player);
    }

    public void OpenSelection(List<SO_Item> Items)
    {

        m_SelectionPanel.SetActive(true);
        m_SelectionFade.enabled = true;

        for (int i = 0; i < m_SelectionItems.Count; i++)
        {
            m_SelectionItems[i].SelectionItem_NameText.text = Items[i].ItemName;
            m_SelectionItems[i].SelectionItem_DescriptionText.text = Items[i].ItemDescription;
            GameObject itemGO = Instantiate(Items[i].Chest_GameObject, m_SelectionItems[i].StoreItem_Socket);
            m_SelectionItemsGO.Add(itemGO);
            m_SelectionItemsSO.Add(Items[i]);

           
        }




    }

    public void SelectItem(int selectIndex)
    {
        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipItemInLoadout(m_SelectionItemsSO[selectIndex]);
        //CloseSelection(); 
    }

    public void RefreshInventoryUI(Dictionary<ESlot, SO_Item> items)
    {
        Debug.Log("Refreshing Inventory UI");
        foreach (var SO_Item in items)
        {
            if (SO_Item.Value == null)
            {
                ChangeSlotIcon(SO_Item.Key, null);
            }
            else
            {
                ChangeSlotIcon(SO_Item.Key, SO_Item.Value.ItemIcon);
            }
            
        }
    }
    

    

    
}
