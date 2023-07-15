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

    public virtual void GotDamaged(Damage damage) {

        if(!GetComponent<Rigidbody>()) {
            Debug.Log("empujando al enemigo1");
            Rigidbody temporalRb = gameObject.AddComponent<Rigidbody>();
            temporalRb.useGravity = false;

            // Obtener la dirección opuesta a la normal de la colisión

            temporalRb.AddForce(damage.forceImpulse * 2.5f, ForceMode.Impulse);
            Destroy(temporalRb, 0.5f);
        }
        else
        {
            Debug.Log("empujando al enemigo2");
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;

            // Obtener la dirección opuesta a la normal de la colisión
            
            rb.AddForce(damage.forceImpulse, ForceMode.Impulse);
            //Destroy(rb, 0.5f);
        }
    }

    public abstract void UpdateAnimator();
    public abstract void ExecuteBehavior();

    public abstract void StartBehavior();
    public abstract void EndBehavior();
}
