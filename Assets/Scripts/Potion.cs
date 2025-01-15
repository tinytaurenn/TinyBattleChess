using UnityEngine;

public class Potion : InventoryItem
{
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
