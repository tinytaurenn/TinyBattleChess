using UnityEngine;

public class Scroll : InventoryItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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

        Debug.Log("using Scroll");
    }

    public override void SetupItem()
    {
        base.SetupItem();
    }
}
