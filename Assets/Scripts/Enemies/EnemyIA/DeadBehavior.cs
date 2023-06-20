using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBehavior : EnemyBehavior
{
    

    
    public override void EndBehavior() {
    }

    public override void ExecuteBehavior() {
    }

    int deadAnimation = Animator.StringToHash("Death");
    public override void StartBehavior() {
        currentDestination = transform.position;
        agent.isStopped = true;
        agent.speed = 0f;
        animator.SetTrigger(deadAnimation);
    }

    public override void UpdateAnimator() {
    }
}
