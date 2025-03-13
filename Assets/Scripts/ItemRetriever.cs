using System.Collections.Generic;
using UnityEngine;

public class ItemRetriever : MonoBehaviour
{
    public static ItemRetriever Instance;


    [SerializeField] SO_Items m_SelectedItems;
    [SerializeField] GameEffectsContainers m_SelectedGameEffects;
    Dictionary<string, SO_Item> m_StringToSoItemDico = new Dictionary<string, SO_Item>();
    Dictionary<string, SO_GameEffect_Container> m_StringToSOEffectDico = new Dictionary<string, SO_GameEffect_Container>();

    [SerializeField] int m_NumItems = 0; 
    [SerializeField] int m_NumEffects = 0; 

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

        foreach (SO_GameEffect_Container effect in m_SelectedGameEffects.GameEffectContainers)
        {
            m_StringToSOEffectDico.Add(effect.GameEffectID, effect); 
        }

        m_NumItems = m_StringToSoItemDico.Count;
        m_NumEffects = m_StringToSOEffectDico.Count;
    }
    public SO_Item GetItem(string itemID)
    {
        if (m_StringToSoItemDico.ContainsKey(itemID))
        {
            return m_StringToSoItemDico[itemID];
        }
        return null;
    }

    public SO_GameEffect_Container GetEffect(string effectID)
    {
        if (m_StringToSOEffectDico.ContainsKey(effectID))
        {
            return m_StringToSOEffectDico[effectID];
        }
        return null;
    }
}
