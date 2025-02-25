using UnityEngine;

public abstract class  InventoryItem : Grabbable
{


    [SerializeField] protected Renderer m_Renderer; 

    protected override void Awake()
    {
        base.Awake(); 
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update(); 
    }


    public virtual void UseInventoryItem()
    {
        Debug.Log("using iventory item"); 

    }

    public virtual void SetupItem()
    {
        m_Renderer.enabled = false; 
    }
}
