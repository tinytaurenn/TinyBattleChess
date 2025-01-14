
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; private set; }

    [SerializeField] List<Chest> m_ChestList = new List<Chest>();

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
    }

    public void ReloadShop(EItemRarity rarity)
    {
        foreach (Chest chest in m_ChestList)
        {
            chest.LoadChest(3,rarity);
        }
    }
    
}
