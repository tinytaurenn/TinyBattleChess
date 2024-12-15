using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    TinyPlayer m_TinyPlayer; 

    [SerializeField] Grabbable m_GrabbedItem;

    [SerializeField] BasicWeapon m_MainWeapon; 
    [SerializeField] BasicWeapon m_SecondaryWeapon;


    private void Awake()
    {
        m_TinyPlayer = GetComponent<TinyPlayer>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (m_GrabbedItem != null)
        {
            Drop();
        }
    }

    #region EquipAndDrop
    public void EquipWeapon(Grabbable weapon)
    {
        
        m_GrabbedItem = weapon;
        m_GrabbedItem.m_Rigidbody.isKinematic = true;
        m_GrabbedItem.m_Collider.enabled = false;
        m_GrabbedItem.transform.SetParent(m_TinyPlayer.m_PlayerRightHandSocket, false);

        m_GrabbedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if(m_GrabbedItem.TryGetComponent<BasicWeapon>(out BasicWeapon basicWeapon))
        {
            m_MainWeapon = basicWeapon;
            
        }

        

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
        m_MainWeapon = null;


    }
    #endregion

}
