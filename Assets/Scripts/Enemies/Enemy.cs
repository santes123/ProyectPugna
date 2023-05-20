using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State {Idle, Chasing, Attacking };
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    Material skinMaterial;

    Color originalColour;

    float attackDistanceThreshhold = .5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float enemyCollisionRadius;
    float targetCollisionRadius;

    public float damage;

    //UI damage (añadirlo de forma dinamica posteriormente
    public GameObject floatingDamageTextPrefab;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        currentState = State.Chasing;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        skinMaterial = GetComponentInChildren<Renderer>().material;
        //skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;

        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CharacterController>().radius;

        StartCoroutine(UpdatePath());

        //floatingDamageTextPrefab = transform.GetChild(1).transform.GetChild(1).gameObject;
    }

    void Update()
    {
        //no atacaran si el jugador ha muerto
        if (target != null)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrtToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrtToTarget < Mathf.Pow(attackDistanceThreshhold + enemyCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        } 
    }
    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (enemyCollisionRadius);


        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }
        skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true;


    }
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (target != null)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (enemyCollisionRadius + targetCollisionRadius + attackDistanceThreshhold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            
            yield return new WaitForSeconds(refreshRate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //arreglar los errores al matar al jugador
        if (other.CompareTag("Player"))
        {
            print("hit jugador");
            IDamageable damageableObject = other.GetComponent<IDamageable>();

            if (damageableObject != null)
            {
                Damage damage = new Damage();
                damage.amount = 2;
                damage.source = UnitType.Player;
                damage.targetType = TargetType.Single;

                damageableObject.ReceiveDamage(damage);
                /*if (!floatingDamageTextPrefab.GetComponent<Text>().isActiveAndEnabled)
                {
                    floatingDamageTextPrefab.GetComponent<Text>().enabled = true;
                }
                FloatingDamageText floatingDamageText = floatingDamageTextPrefab.GetComponent<FloatingDamageText>();


                if (floatingDamageText != null)
                {
                    // Configura el texto y el color del daño infligido
                    floatingDamageText.SetDamageText(" - " + damage.amount.ToString(), Color.red);
                }*/
            }
        }
        //hacemos daño al jugador
       /* Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2, -1, QueryTriggerInteraction.Collide))
        {
            print("hit jugador");
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();

            if (damageableObject != null)
            {
                damageableObject.TakeHit2(damage);
            }
        }*/
    }
}