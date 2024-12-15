using Coherence.Toolkit;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class BasicWeapon : Grabbable, IWeapon
{
    [Space(10)]
    [Header("Weapon Parameters")]
    [SerializeField] int Damage = 10;
    [SerializeField] float Speed = 1.5f;
    [SerializeField] int Cost = 10;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        
    }
}
