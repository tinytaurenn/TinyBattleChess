using System.Collections.Generic;
using UnityEngine;

public class ItemRetriever : MonoBehaviour
{
    public static ItemRetriever Instance;


    [SerializeField] SO_Items m_SelectedItems;
    Dictionary<string, SO_Item> m_StringToSoItemDico = new Dictionary<string, SO_Item>();

    [SerializeField] int m_NumItems = 0; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }else
        {
            Destroy(gameObject);
            return; 
        }

        DontDestroyOnLoad(gameObject);
        foreach (SO_Item item in m_SelectedItems.Items)
        {
            m_StringToSoItemDico.Add(item.ItemID, item); 
        }

        m_NumItems = m_StringToSoItemDico.Count;
    }
    public SO_Item GetItem(string itemID)
    {
        if (m_StringToSoItemDico.ContainsKey(itemID))
        {
            return m_StringToSoItemDico[itemID];
        }
        return null;
    }
}
