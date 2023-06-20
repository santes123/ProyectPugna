using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : EnemyBehavior
{
    float timeBetweenAttacks = 3f;
    float nextAttack;

    PlayerStats target;
    int attack = Animator.StringToHash("Attack");
    
    public override void StartBehavior() {
        agent.speed = 0;
        target = FindObjectOfType<PlayerStats>();
    }
    public override void ExecuteBehavior() {
        currentDestination = target.transform.position;
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        if(nextAttack < Time.time) {
            animator.SetTrigger(attack);
            nextAttack = Time.time + timeBetweenAttacks;
        }
    }
    public override void EndBehavior() {
        agent.SetDestination(transform.position);
    }

    public override void UpdateAnimator() {
       
    }
}
