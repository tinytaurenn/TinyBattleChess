using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] SO_Chest m_ChestSO;

    [SerializeField] List<SO_Item> m_ChosenItems = new List<SO_Item>();
    

    void Start()
    {
        Debug.Log("chest testing");
        m_ChosenItems = m_ChestSO.GetItemList();
    }

    
    void Update()
    {
        
    }
}
