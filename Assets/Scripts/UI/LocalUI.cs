using Coherence.Samples.WorldDialog;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerLoadout;

public class ESlotToSlotUIDictionary : SerializableDictionary<EStuffSlot, SlotUI> { }

public class LocalUI : MonoBehaviour
{
    public static LocalUI Instance { get; private set; }

    [Space(10)]
    [Header(" Pause ")]
    public PauseMenu m_PauseMenu;


    [Space(10)]
    [Header("Player Stats ")]
    [SerializeField] Slider m_PlayerHealth_Slider;
    [SerializeField] Slider m_PlayerGlobalHealth_Slider;
    [SerializeField] TextMeshProUGUI m_PlayerHealth_ValueText;

    [Space(10)]
    [Header("Inventory ")]
    [SerializeField] List<SlotUI> m_InventorySlots;
    [SerializeField] SlotUI m_MainWeaponSlot; 
    [SerializeField] SlotUI m_SecondaryWeaponSlot;

    [SerializeField] SlotUI m_HelmetSlot;
    [SerializeField] SlotUI m_ChestSlot;
    [SerializeField] SlotUI m_ShouldersSlot; 

    [SerializeField] SlotUI m_SelectedSlot;

    //Dictionary<PlayerLoadout.ESlot, SlotUI> m_SlotDictionary = new Dictionary<PlayerLoadout.ESlot, SlotUI>();
    ESlotToSlotUIDictionary m_SlotDictionary = new ESlotToSlotUIDictionary();

    [SerializeField] TextMeshProUGUI m_UsableText;

    [SerializeField] Color m_LoadoutColor;
    [SerializeField] Color m_EquippedColor; 

    [Space(10)]
    [Header("Selection Panel")]

    [SerializeField] GameObject m_SelectionPanel;
    [SerializeField] Image m_SelectionFade;

    [Space(10)]
    [Header("Shop Related")]

    [SerializeField] GameObject m_ShopRelatedGO;
    [SerializeField] TextMeshProUGUI m_GoldText;

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

    [SerializeField] TextMeshProUGUI m_ReplacementText;
    [SerializeField] Color m_ReplacementColor; 



    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        m_SlotDictionary.Add(EStuffSlot.MainWeapon, m_MainWeaponSlot);
        m_SlotDictionary.Add(EStuffSlot.SecondaryWeapon, m_SecondaryWeaponSlot);
        m_SlotDictionary.Add(EStuffSlot.Slot_1, m_InventorySlots[0]);
        m_SlotDictionary.Add(EStuffSlot.Slot_2, m_InventorySlots[1]);
        m_SlotDictionary.Add(EStuffSlot.Slot_3, m_InventorySlots[2]);
        m_SlotDictionary.Add(EStuffSlot.Slot_4, m_InventorySlots[3]);

        m_SlotDictionary.Add(EStuffSlot.Helmet, m_HelmetSlot);
        m_SlotDictionary.Add(EStuffSlot.Chest, m_ChestSlot);
        m_SlotDictionary.Add(EStuffSlot.Shoulders, m_ShouldersSlot);


        m_UsableText.enabled = false;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120; 
        Cursor.lockState = CursorLockMode.Confined;

        foreach (Button item in m_PauseMenu.m_ReturnButton)
        {
            item.onClick.AddListener(m_PauseMenu.OnReturnButton); 
        }
        m_PauseMenu.m_LobbyButton.onClick.AddListener(m_PauseMenu.OnLobbyButton);
        m_PauseMenu.m_SettingsButton.onClick.AddListener(m_PauseMenu.OnSettingsButton);
        m_PauseMenu.m_SensivitySlider.onValueChanged.AddListener(m_PauseMenu.OnSensivitySliderChange);

        


    }
    




    public void TogglePause()
    {
        m_PauseMenu.TogglePause(); 
    }
    
    public void SelectSlot(EStuffSlot slot)
    {
        if(m_SelectedSlot != null) m_SelectedSlot.UnSelectSlot();

        m_SelectedSlot = m_SlotDictionary[slot];
        m_SelectedSlot.SelectSlot();
    }

    public void ChangeSlotIcon(EStuffSlot slot, Sprite Icon)
    {
        m_SlotDictionary[slot].ChangeIcon(Icon);
    }

    public void ClearSlot(EStuffSlot slot)
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
        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerControls.ExitReplaceInventorySlotControls();
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

    public void OnReplaceItem()
    {
        ColorSlots(m_ReplacementColor);
        m_ReplacementText.enabled = true;

    }

    public void RefreshInventoryUI(Dictionary<EStuffSlot, SO_Item> items, PlayerLoadout.EInventoryType inventoryType)
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

        ColorInventoryByInventoryType(inventoryType);
    }
    public void CleanInventoryUI()
    {
        foreach (var slot in m_SlotDictionary)
        {
            slot.Value.ChangeIcon(null);
        }
        ColorInventoryByInventoryType(PlayerLoadout.EInventoryType.Equipped);
    }

    void ColorInventoryByInventoryType(PlayerLoadout.EInventoryType inventoryType)
    {

        m_ReplacementText.enabled = false;
        switch (inventoryType)
        {
            case EInventoryType.Loadout:
                ColorInventory(m_LoadoutColor);
                break;
            case EInventoryType.Equipped:
                ColorInventory(m_EquippedColor);
                break;
            default:
                break;
        }
        
    }

    void ColorInventory(Color color)
    {
        foreach (var slot in m_SlotDictionary)
        {
            slot.Value.ColorSlot(color);
        }
    }

    void ColorSlots(Color color)
    {
        foreach (var slot in m_SlotDictionary)
        {
            if(slot.Key == EStuffSlot.SecondaryWeapon || slot.Key == EStuffSlot.MainWeapon) continue;
            slot.Value.ColorSlot(color);
        }
    }

    public void UpdateGoldAmount(int amount)
    {
        m_GoldText.text = amount.ToString();
    }

    public void ShowShopRelated(bool show)
    {
        m_ShopRelatedGO.SetActive(show); 
        UpdateGoldAmount(ConnectionsHandler.Instance.LocalTinyPlayer.PlayerGold);
    }

    public void UpdatePlayerHealthSlider(int value)
    {
        m_PlayerHealth_Slider.value = value;
    }

    public void UpdateGlobalHealthSlider(int value)
    {
        m_PlayerGlobalHealth_Slider.value = value;
        m_PlayerHealth_ValueText.text = value.ToString();

    }

    #region Connect Dialog

    public void Disconnect()
    {
        CleanInventoryUI(); 
        m_PauseMenu.Disconnect();

    }

    #endregion



}
