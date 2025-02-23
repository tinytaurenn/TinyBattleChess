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

        //StartCoroutine(DelayJump());
    }

    protected override void Update()
    {
        base.Update();

        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void MovementUpdate()
    {
        base.MovementUpdate();
    }

    public void MoveOnTarget(Transform target)
    {
        Vector3 direction = transform.VectorTowardsTransform(target, true);

        MoveInput = direction;

        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(m_rigidBody.rotation, newRotation, Time.deltaTime * m_RotationSpeed);
    }

    public void MoveOnPosition(Vector3 target)
    {
        Vector3 direction = transform.VectorTowardsPosition(target, true);

        MoveInput = direction;

        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(m_rigidBody.rotation, newRotation, Time.deltaTime * m_RotationSpeed);
    }
    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(3.0f);
        TryJump();
        StartCoroutine(DelayJump());
    }

    


}
