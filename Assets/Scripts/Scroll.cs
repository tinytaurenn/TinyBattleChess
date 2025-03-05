using UnityEngine;
using static SO_Scroll;

public class Scroll : InventoryItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] EScrollElem m_ScrollElems;
    public EScrollElem ScrollElems { get { return m_ScrollElems; } set { m_ScrollElems = value; } }

    [SerializeField] int m_ScrollCharges = 1; 
    public int ScrollCharges { get { return m_ScrollCharges; } set { m_ScrollCharges = value; } }

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
