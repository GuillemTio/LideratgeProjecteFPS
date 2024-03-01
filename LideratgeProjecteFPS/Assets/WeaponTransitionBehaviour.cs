using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class WeaponTransitionBehaviour : StateMachineBehaviour
{
    private Weapon m_Weapon;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_Weapon == null)
        {
            m_Weapon = animator.GetComponentInParent<Weapon>();
        }
        m_Weapon.SetShowMesh(true);
        m_Weapon.InTransition = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("Seath"))
        {
            m_Weapon.SetShowMesh(false);
        }
        m_Weapon.InTransition = false;
        
        //Para empezar playear la animacion idle del arma y que quede justo con el swap
        var ac = animator.runtimeAnimatorController as AnimatorController;
        var idle = ac.layers[0].stateMachine.states[0].state.name;
        animator.Play(idle, 0, 0.0f);
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
