using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] Grabbable m_GrabbedItem; 

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void EquipWeapon(Grabbable weapon)
    {
        m_GrabbedItem = weapon;
        m_GrabbedItem.m_Rigidbody.isKinematic = true;
        m_GrabbedItem.m_Collider.enabled = false;
        m_GrabbedItem.transform.SetParent(transform, false);

        m_GrabbedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

    }

    public void Drop(float throwForce = 0f)
    {
        if(m_GrabbedItem == null)
        {
            return; 
        }

        m_GrabbedItem.transform.SetParent(null, true);
        m_GrabbedItem.m_Rigidbody.isKinematic = false;
        m_GrabbedItem.m_Rigidbody.AddForce(throwForce * transform.forward, ForceMode.VelocityChange);
        m_GrabbedItem.m_Rigidbody.AddTorque(-transform.right * throwForce * 1f, ForceMode.VelocityChange);

        m_GrabbedItem.Release();

        m_GrabbedItem.m_Collider.enabled = true; 
        m_GrabbedItem = null;


    }

    private void OnDisable()
    {
        if(m_GrabbedItem != null)
        {
            Drop(); 
        }
    }
}
