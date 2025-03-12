using PlayerControls;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : Usable
{
    Collider m_Collider; 

    [SerializeField] SO_Chest m_ChestSO;

    [SerializeField] List<SO_Item> m_ChosenItems = new List<SO_Item>();

    public int Cost = 10;

    [Space(10)]
    [Header("UI")]

    [SerializeField] GameObject m_Canvas;
    [SerializeField] TextMeshProUGUI m_CostText;
    [SerializeField] float m_ShowDistance;

    bool m_AlignToPlayer = false; 



    protected override void Awake()
    {
        base.Awake();
        m_Collider = GetComponent<Collider>();
        
    }



    private void FixedUpdate()
    {
        if(m_AlignToPlayer)
        {
            m_Canvas.transform.LookAt(CameraManager.Instance.transform.position);
            m_Canvas.transform.Rotate(0, 180, 0);
        }
    }

    public override  void  TryUse()
    {
        //base.TryUse();

        if(!CanUseChest())
        {
            Debug.Log("cannot use chest");
            return;
        }

        LoadChest(3,EItemRarity.Common);
        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Selecting);
        ConnectionsHandler.Instance.LocalTinyPlayer.PlayerGold -= Cost;
        LocalUI.Instance.OpenSelection(m_ChosenItems); 

        
    }

    bool CanUseChest()
    {
        if (!ConnectionsHandler.Instance.LocalTinyPlayer.CanUseGold) return false;
        if (ConnectionsHandler.Instance.LocalTinyPlayer.PlayerGold < Cost) return false;

        return true; 

    }

    public void LoadChest(int number,EItemRarity rarity)
    {
        m_ChosenItems = m_ChestSO.GetItemsList(number, rarity);
 
    }

    public SO_Item GetItem()
    {
        return m_ChosenItems.RandomInList();
    }
    public SO_Item GetItemFast()
    {
        return m_ChestSO.GetItemFast(EItemRarity.Common);
    }

    //canvas 




    private void OnTriggerEnter(Collider other)
    {

        m_CostText.text = "Cost : " + Cost.ToString();

        if (other.TryGetComponent<TinyPlayer>(out TinyPlayer player))
        {
            if (player != ConnectionsHandler.Instance.LocalTinyPlayer) return;
            m_Canvas.SetActive(true);
            m_AlignToPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<TinyPlayer>(out TinyPlayer player))
        {
            if (player != ConnectionsHandler.Instance.LocalTinyPlayer) return; 
            m_Canvas.SetActive(false);
            m_AlignToPlayer = false;
        }
    }
}
