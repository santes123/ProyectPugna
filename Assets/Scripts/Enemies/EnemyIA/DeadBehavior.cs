using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBehavior : EnemyBehavior
{

    public GameObject DeathVFX;
    
    public override void EndBehavior() {
    }

    public override void ExecuteBehavior() {
    }

    int deadAnimation = Animator.StringToHash("Death");
    public override void StartBehavior() {
        Debug.Log("I died "+name);
        currentDestination = transform.position;
        agent.isStopped = true;
        agent.speed = 0f;
        animator.SetTrigger(deadAnimation);
        Instantiate(DeathVFX,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    public override void UpdateAnimator() {
    }

}
