using UnityEngine;

public class AttackReleaseBehavior : StateMachineBehaviour
{
    [SerializeField] PlayerWeapons m_PlayerWeapons;
    [SerializeField] Mesh workma; 

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("attack release OnStateExit");
    }
}
