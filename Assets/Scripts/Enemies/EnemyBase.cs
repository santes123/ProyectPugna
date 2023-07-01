using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private GameObject debuffColdown;
    public GameObject damageVFX;
    protected override void Start()
    {
        base.Start();
        behaviorTree = GetComponent<Animator>();
        player = FindObjectOfType<PlayerStats>();
        OnDeath += Death;
        //buscamos el icono de debuff en la UI del enemigo
        debuffColdown = FindChildObjectWithImageComponent(gameObject.transform, "Coldown");
        if (debuffColdown != null)
        {
            //Debug.Log("OBJETO COLDOWN ENCONTRADO!");
            //buff.gameObject.transform.parent.gameObject;
            debuffColdown.gameObject.transform.parent.gameObject.SetActive(false);
            //Debug.Log("icono coldown = " + debuffColdown.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze)
        {
            freezeDuration -= Time.deltaTime;
            debuffColdown.GetComponent<TextMeshPro>().text = freezeDuration.ToString("F1");
            //GetComponent<ChaseBehavior>().EndBehavior();
            if (freezeDuration <= 0f)
            {
                freeze = false;
                debuffColdown.gameObject.transform.parent.gameObject.SetActive(false);
                GetComponent<NavMeshAgent>().enabled = true;
                //GetComponent<ChaseBehavior>().StartBehavior();
            }
        }
        //else
        //{
            if (!(player != null) && GetComponent<NavMeshAgent>().isActiveAndEnabled)
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
            Instantiate(damageVFX, transform.position + ((damage.point-transform.position) * 0.5f) ,Quaternion.identity);
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
        debuffColdown.gameObject.transform.parent.gameObject.SetActive(true);
        GetComponent<NavMeshAgent>().enabled = false;
        //GetComponent<ChaseBehavior>().EndBehavior();
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
}
