using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public bool m_Rotating = true; 
    public float m_RotationSpeed = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!m_Rotating) return; 

        transform.Rotate(Vector3.up, m_RotationSpeed);

    }
}
