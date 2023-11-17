using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1Behavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (MeleeMechanic.instance.isAttacking)
        {
            MeleeMechanic.instance.animator.Play("Attack2_Animation");
            MeleeMechanic.instance.playerAudio.PlayOneShot(MeleeMechanic.instance.slash2, PlayerMove.instance.volume);
        }
        if (MeleeMechanic.instance.isShoot)
        {
            MeleeMechanic.instance.animator.Play("Shoot_Animation");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeleeMechanic.instance.isAttacking = false;
        MeleeMechanic.instance.isShoot = false;
        PlayerMove.instance.runSpeed = 10;
        PlayerMove.instance.jumpingPower = 14;
        PlayerMove.instance.slideSpeed = 5;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
