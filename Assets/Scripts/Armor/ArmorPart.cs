using Coherence.Toolkit;
using System;
using UnityEngine;

public class ArmorPart : MonoBehaviour
{
    CoherenceSync m_Sync;

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_Sync.OnStateAuthority.AddListener (OnStateAuthority);

    }

    private void OnStateAuthority()
    {
        if (!m_Sync.HasStateAuthority) return; 
        if(transform.root.TryGetComponent<CoherenceSync>(out CoherenceSync rootSync))
        {
            if (!rootSync.HasStateAuthority)
            {
                Debug.LogWarning(" no auth onnroot but auth on child");
                Destroy(gameObject); 
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
