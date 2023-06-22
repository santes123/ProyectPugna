using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyBehavior : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected  Animator animator;
    protected Vector3 currentDestination;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<AnimatorEvents>().GetComponent<Animator>();
    }

    public abstract void UpdateAnimator();
    public abstract void ExecuteBehavior();

    public abstract void StartBehavior();
    public abstract void EndBehavior();
}
