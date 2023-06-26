using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : LivingEntity
{
    PlayerStats player;
    Animator behaviorTree;
    // Start is called before the first frame update

    //nuevas funcionalidades
    private bool freeze = false;
    private float freezeDuration;
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
        if (freeze)
        {
            freezeDuration -= Time.deltaTime;
            //debuffColdown.GetComponent<TextMeshPro>().text = freezeDuration.ToString("F1");
            if (freezeDuration <= 0f)
            {
                freeze = false;
                //debuffColdown.gameObject.transform.parent.gameObject.SetActive(false);
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }
        //else
        //{
            if (!(player != null))
            {
                player = FindObjectOfType<PlayerStats>();
            }
            UpdateBehaviorTree();
        //}

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

    public override void ReceiveDamage(Damage damage) {
        base.ReceiveDamage(damage);
        if(currentHealth > 0) {
            GetComponent<EnemyBehavior>().GotDamaged(damage);
        }
    }

    void Death() {
        behaviorTree.SetTrigger(isDead);
    }

    //nuevas funcionalidades
    public void Freeze(float timeToFreeze)
    {
        freeze = true;
        freezeDuration = timeToFreeze;
        //debuffColdown.gameObject.transform.parent.gameObject.SetActive(true);
    }
}
