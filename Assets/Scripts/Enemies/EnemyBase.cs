using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : LivingEntity
{
    PlayerStats player;
    Animator behaviorTree;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        behaviorTree = GetComponent<Animator>();
        player = FindObjectOfType<PlayerStats>();
        OnDeath += Death;
    }

    // Update is called once per frame
    void Update()
    {
        if(!(player != null)) {
            player = FindObjectOfType<PlayerStats>();
        }
        UpdateBehaviorTree();
    }

    int distanceToPlayer = Animator.StringToHash("DistanceToPlayer");
    int isDead = Animator.StringToHash("IsDead");
    void UpdateBehaviorTree() {
        if(player != null) {
            behaviorTree.SetFloat(distanceToPlayer, Vector3.Distance(transform.position, player.transform.position));
        } else {
            behaviorTree.SetFloat(distanceToPlayer, 1000f);
        }
    }

    void Death() {
        behaviorTree.SetTrigger(isDead);
    }
}
