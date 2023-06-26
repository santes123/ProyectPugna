using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity, IDamager
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

    public bool freeze = false;
    private float freezeDuration;

    private GameObject debuffColdown;
    private Image debuggImage;
    [HideInInspector]
    public bool bounceOnEnemies = false;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        currentState = State.Chasing;
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = FindObjectOfType<PlayerStats>().gameObject.transform;
        skinMaterial = GetComponentInChildren<Renderer>().material;
        //skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;

        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CharacterController>().radius;

        StartCoroutine(UpdatePath());

        debuffColdown = FindChildObjectWithImageComponent(this.gameObject.transform, "Coldown");
        if (debuffColdown != null)
        {
            //Debug.Log("OBJETO COLDOWN ENCONTRADO!");
            //buff.gameObject.transform.parent.gameObject;
            debuffColdown.gameObject.transform.parent.gameObject.SetActive(false);
        }
        //floatingDamageTextPrefab = transform.GetChild(1).transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (freeze)
        {
            currentState = State.Idle;
            freezeDuration -= Time.deltaTime;
            debuffColdown.GetComponent<TextMeshPro>().text = freezeDuration.ToString("F1");
            if (freezeDuration <= 0f)
            {
                freeze = false;
                debuffColdown.gameObject.transform.parent.gameObject.SetActive(false);
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }
        else
        {
            
            currentState = State.Chasing;
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
    public void Freeze(float timeToFreeze)
    {
        freeze = true;
        freezeDuration = timeToFreeze;
        debuffColdown.gameObject.transform.parent.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        //arreglar los errores al matar al jugador
        if (other.CompareTag("Player") && !freeze)
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
                other.gameObject.GetComponentInChildren<Animator>().SetTrigger("GetHit");
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
        }else if (other.CompareTag("Enemy") && bounceOnEnemies)
        {
            MakeDamageAndPushEnemy(other);
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
    private GameObject FindChildObjectWithImageComponent(Transform parent, string childObjectName)
    {
        // Buscar en los hijos directos del objeto
        foreach (Transform child in parent)
        {
            //Debug.Log("NOMBRE DEL GAMEOBJECT = " + child.gameObject.name);
            TextMeshPro imageComponent = child.gameObject.GetComponent<TextMeshPro>();
            if (imageComponent != null && child.gameObject.name == childObjectName)
            {
                //Debug.Log("NOMBRE DEL GAMEOBJECT ENCONTRADO = " + child.gameObject.name);
                // Se encontró el objeto hijo con el componente Image y el nombre deseado
                // Devolver el objeto padre en lugar del hijo
                return child.gameObject/*.transform.parent.gameObject*/;
            }

            // Realizar una búsqueda recursiva en los hijos del objeto actual
            GameObject foundObject = FindChildObjectWithImageComponent(child, childObjectName);
            if (foundObject != null)
            {
                // Se encontró el objeto deseado en uno de los hijos
                return foundObject;
            }
        }

        // No se encontró ningún objeto hijo con el nombre y componente Image deseado
        return null;
    }
    //recibir dañor
    public override void ReceiveDamage(Damage damage)
    {
        base.ReceiveDamage(damage);
        RecieveDamageVisual();
        RecieveDamagePhysics(damage);

    }
    public void RecieveDamageVisual()
    {
        
        Renderer hitRenderer = GetComponentInChildren<Renderer>();
        // Cambiar el color del material del renderer
        if (hitRenderer != null)
        {
            hitRenderer.material.color = Color.blue;
        }
    }
    public void RecieveDamagePhysics(Damage damage)
    {
        if (!GetComponent<Rigidbody>())
        {
            Rigidbody temporalRb = gameObject.AddComponent<Rigidbody>();
            temporalRb.useGravity = false;

            // Obtener la dirección opuesta a la normal de la colisión

            temporalRb.AddForce(damage.forceImpulse, ForceMode.Impulse);
            Destroy(temporalRb, 0.5f);
        }
    }
    public void MakeDamageAndPushEnemy(Collider other)
    {
        //HACEMOS DAÑO AL ENEMIGO MEDIANTE LA INTERFAZ
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            print("collision con el enemigo");
            Damage damageObj = new Damage();
            damageObj.amount = (int)damage;
            damageObj.source = UnitType.Player;
            damageObj.targetType = TargetType.Single;

            // Obtener la direccion opuesta a la normal de la colision
            Vector3 normal = other.transform.position - transform.position;
            normal.y = 0;
            normal.Normalize();
            damageObj.forceImpulse = normal * 5f;
            //llamamos al metodo de la interfaz
            DoDamage(damageableObject, damageObj);
            //damageableObject.ReceiveDamage(damageObj);
        }
    }

    public void DoDamage(IDamageable target, Damage damage)
    {
        target.ReceiveDamage(damage);
    }

    void IDamager.Attack() {
    }
}