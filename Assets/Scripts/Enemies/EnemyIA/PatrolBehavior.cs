using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : EnemyBehavior
{
    Vector3 spawnLocation;
    
    public Vector2 areaOfPatrol;
    public float accuracy = 0.25f;
    public float maxIdleTime = 3f;
    float nextDestinationTime;
    // Start is called before the first frame update
    void Start() {
        spawnLocation = transform.position;
    }

    public override void Update() {
        base.Update();
        UpdateAnimator();
    }

    
    bool givenNextDestinationTime = false;
    public override void StartBehavior() {
        agent.speed = 2f;
        givenNextDestinationTime = false;
        animator.SetBool(inCombat, false);
        SetNewDestination();
    }
   
    public override void ExecuteBehavior() {
        if(!agent.hasPath || agent.remainingDistance <= accuracy) {
            if(!givenNextDestinationTime) {
                nextDestinationTime = Time.time + Random.Range(0f, maxIdleTime);
                givenNextDestinationTime = true;
            } else {
                if(Time.time > nextDestinationTime) {
                    SetNewDestination();
                }
            }
        }
    }
    void SetNewDestination() {
        float posX = Random.Range(-areaOfPatrol.x, areaOfPatrol.x);
        float posZ = Random.Range(-areaOfPatrol.y, areaOfPatrol.y);
        currentDestination = spawnLocation + new Vector3(posX, 0f, posZ);
        givenNextDestinationTime = false;
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
