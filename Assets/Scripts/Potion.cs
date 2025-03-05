using System.Collections.Generic;
using UnityEngine;
using static SO_Potion;

public class Potion : InventoryItem
{
    [SerializeField] List<FPotionEffect> m_PotionEffect;
    public List<FPotionEffect> PotionEffects { get { return m_PotionEffect; } set { m_PotionEffect = value; }  }
    [SerializeField] int m_PotionCharges = 1;
    public int PotionCharges { get { return m_PotionCharges; } set { m_PotionCharges = value; } }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UseInventoryItem()
    {
        base.UseInventoryItem();

        Debug.Log("using potion"); 
    }

    public override void SetupItem()
    {
        base.SetupItem();
    }
}
