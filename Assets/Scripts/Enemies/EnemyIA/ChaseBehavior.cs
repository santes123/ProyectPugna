using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehavior : EnemyBehavior
{
    PlayerStats target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }
    
    int speed = Animator.StringToHash("Speed");
    public override void StartBehavior() {
        target = FindObjectOfType<PlayerStats>();
        agent.speed = 2.2f;
    }
    public override void ExecuteBehavior() {
        currentDestination = target.transform.position;
        agent.SetDestination(currentDestination);
    }
    public override void EndBehavior() {
        agent.SetDestination(transform.position);
    }


    public override void UpdateAnimator() {
        animator.SetFloat(speed, agent.velocity.magnitude);
    }

}
