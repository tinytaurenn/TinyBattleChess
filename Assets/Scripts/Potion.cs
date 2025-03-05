using System.Collections.Generic;
using UnityEngine;
using static SO_Potion;

public class Potion : InventoryItem
{
    public List<FPotionEffect> PotionEffects { get; set; }

    public int PotionCharges { get; set; } = 1; 
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
