using UnityEngine;

public class AttackReleaseBehavior : StateMachineBehaviour
{


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Blocked"); 
    }
}
