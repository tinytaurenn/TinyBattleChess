using Coherence.Toolkit;
using UnityEngine;

[RequireComponent(typeof(CoherenceSync))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class Projectile : MonoBehaviour
{

    protected CoherenceSync m_Sync;
    Rigidbody m_RigidBody; 
    protected Collider  m_Collider;
    public Renderer m_Renderer; 
    public MeshFilter m_MeshFilter; 
    [SerializeField]protected LayerMask m_ExplosionMask; 

    [SerializeField] protected bool m_Exploded = false;

    protected float m_Timer = 0f;
    protected float m_ActivationTime = 0.05f;
    protected virtual void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        
    }

    private void Update()
    {
        if (m_Timer > m_ActivationTime) return;
        m_Timer += Time.deltaTime;
    }


    public virtual void Launch(Vector3 direction, float throwforce)
    {
        m_RigidBody.AddForce(direction * throwforce, ForceMode.Impulse);
    }
    protected abstract void OnHit(Collider other);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            return; 
        }

        if(m_Exploded) return;
        OnHit(other); 
    }

    [Command]
    public abstract void InstantiateExplosion(Vector3 pos);
    [Command]
    public abstract void StopParticles();

}
