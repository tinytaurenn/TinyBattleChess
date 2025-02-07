using System.Collections;
using UnityEngine;

public class NPC_Movement : CharacterMovement
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(DelayJump());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(2.0f);
        TryJump();
        StartCoroutine(DelayJump());
    }
}
