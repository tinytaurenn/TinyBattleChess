using UnityEngine;

public class Scroll : InventoryItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] int m_ScrollCharges = 1; 

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

    public override bool UseInventoryItem()
    {
        base.UseInventoryItem();

        Debug.Log("using Scroll");

        return false; 
    }

    public override void SetupItem()
    {
        base.SetupItem();
    }
}
