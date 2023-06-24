using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : EnemyBehavior, IDamager
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
        agent.SetDestination(currentDestination);
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

    public Damage damage;

    public void Attack() {
        //first we get the targets.
        List<IDamageable> damageablesFound = GetTargets();

        //then we apply the damage on all the targets found.
        foreach(IDamageable damageable in damageablesFound) {
            DoDamage(damageable, damage);
        }
    }

    public List<IDamageable> GetTargets() {
        AreaOfEffect aoe = GetComponent<AreaOfEffect>();
        return aoe.GetTargets<IDamageable>();
    }

    public void DoDamage(IDamageable target, Damage damage) {
        target.ReceiveDamage(damage);
    }

    public override void GotDamaged(Damage damage) {
        nextAttack = Time.time + timeBetweenAttacks;
        base.GotDamaged(damage);
    }
}
