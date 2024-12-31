using UnityEngine;

public class DeathZone : MonoBehaviour
{

    Collider m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();



    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.TryGetComponent<TinyPlayer>(out TinyPlayer player))
            {
                player.PlayerDeath(); 
            }
        }
    }

}
