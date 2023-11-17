using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (MeleeMechanic.instance.isAttacking)
        {
            MeleeMechanic.instance.animator.Play("Aerial_Attack1_Animation");
            MeleeMechanic.instance.playerAudio.PlayOneShot(MeleeMechanic.instance.slash1, PlayerMove.instance.volume);
            PlayerMove.instance.animator.SetBool("isJumping", false);
        }
        if (MeleeMechanic.instance.isShoot)
        {
            MeleeMechanic.instance.animator.Play("Aerial_Shoot_Animation");
            PlayerMove.instance.animator.SetBool("isJumping", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeleeMechanic.instance.isAttacking = false;
        MeleeMechanic.instance.isShoot = false;
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
