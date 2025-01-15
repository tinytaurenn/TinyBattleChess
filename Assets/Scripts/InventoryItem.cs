using UnityEngine;

public abstract class  InventoryItem : MonoBehaviour
{
    public SO_Item SO_Item
    {
        get { return m_SO_Item; } private set { m_SO_Item = value; }
    }

    [SerializeField] SO_Item m_SO_Item;

    [SerializeField] protected Renderer m_Renderer; 

    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update() 
    {
        
    }

    public virtual void UseInventoryItem()
    {
        
    }

    public virtual void SetupItem()
    {
        m_Renderer.enabled = false; 
    }
}
