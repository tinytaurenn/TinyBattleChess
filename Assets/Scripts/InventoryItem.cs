using UnityEngine;

public abstract class  InventoryItem : MonoBehaviour
{
    public SO_Item SO_Item
    {
        get { return m_SO_Item; }
        private set { m_SO_Item = value; }
    }

    [SerializeField] SO_Item m_SO_Item;
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
