using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : EnemyBehavior
{
    Vector3 spawnLocation;
    
    public Vector2 areaOfPatrol;
    public float accuracy = 0.25f;
    public float maxIdleTime = 3f;
    public float nextDestinationTime;
    // Start is called before the first frame update
    void Start() {
        spawnLocation = transform.position;
    }

     void Update() {
        UpdateAnimator();
    }

    public override void StartBehavior() {
        agent.speed = 2f;
        animator.SetBool(inCombat, false);
        SetNewDestination();
    }
    float currentWaitedTime = 0f;
    float currentMaxWaitTime = 0f;
    public override void ExecuteBehavior() {
        
        if(!agent.hasPath) {
            
            SetNewDestination();
            

        } else if(agent.remainingDistance <= accuracy) {

            currentWaitedTime += Time.deltaTime;

            if(currentWaitedTime > currentMaxWaitTime) {

                SetNewDestination();
                currentWaitedTime = 0f;
                currentMaxWaitTime = Random.Range(0, maxIdleTime);

            }

        }
        
    }
    void SetNewDestination() {
        float posX = Random.Range(-areaOfPatrol.x, areaOfPatrol.x);
        float posZ = Random.Range(-areaOfPatrol.y, areaOfPatrol.y);
        currentDestination = spawnLocation + new Vector3(posX, 0f, posZ);
        agent.SetDestination(currentDestination);
    }
    void OnDrawGizmos() {
        //Area of patrol
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(spawnLocation + Vector3.up, new Vector3(areaOfPatrol.x * 2f, 2f, areaOfPatrol.y * 2f));

        //Next destination
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(currentDestination, 0.1f);

    }
    int inCombat = Animator.StringToHash("InCombat");
    int speed = Animator.StringToHash("Speed");
    public override void UpdateAnimator() {
        animator.SetFloat(speed, agent.velocity.magnitude);
    }

    public override void EndBehavior() {
        animator.SetBool(inCombat, true);
    }
}
