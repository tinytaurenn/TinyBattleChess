using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class  InventoryItem : Grabbable
{
    [SerializeField] GameObject m_ItemProjectile; 
    public GameObject ItemProjectile { get { return m_ItemProjectile; } set { m_ItemProjectile = value; } }

    [SerializeField] bool m_Throwable = false;

    public bool Throwable { get { return m_Throwable; } set { m_Throwable = value; } }

    public bool Throwed { get; set; } = false;
    [SerializeField] protected Renderer m_Renderer;

    [SerializeField] int m_UseAmount = 3;

    public event Action<int,EStuffSlot> OnItemUsed;

    public EStuffSlot AssignedSlot { get; set; } = EStuffSlot.Slot_1;
    public int UseAmount { get { return m_UseAmount; } set { m_UseAmount = value; } }

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


    public virtual bool UseInventoryItem()
    {
        Debug.Log("using iventory item");
        return false; 


    }

    public virtual void ThrowItem(Vector3 pos)
    {
       

    }

    public void OnUsedItem()
    {
        OnItemUsed?.Invoke(UseAmount,AssignedSlot);
    }


    public virtual void SetupItem()
    {
        m_Renderer.enabled = false; 
    }

    private void OnDestroy()
    {
        OnItemUsed = null;
    }
}
