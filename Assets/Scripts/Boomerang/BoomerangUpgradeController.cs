using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoomerangUpgradeController : MonoBehaviour
{
    public float areaDamageRadius = 1.5f;
    public float freezeDuration = 3f;

    public bool areaDamageMode = false;
    public bool freezeMode = false;
    private int currentChargesAreaDamage;
    private int currentChargesFreeze;
    BoomerangController boomerangController;

    private void Start()
    {
        boomerangController = GetComponent<BoomerangController>();
        currentChargesAreaDamage = 1;
        currentChargesFreeze = 1;
    }
    void Update()
    {
        // Activa el modo de daño en área al presionar la tecla Q
        if (Input.GetKeyDown(KeyCode.Q) && currentChargesAreaDamage > 0)
        {
            currentChargesAreaDamage--;
            areaDamageMode = !areaDamageMode;
            Debug.Log("Modo de daño en área: " + areaDamageMode);
        }

        // Activa el modo de congelamiento en área al presionar la tecla Z
        if (Input.GetKeyDown(KeyCode.Z) && currentChargesFreeze > 0)
        {
            currentChargesFreeze--;
            freezeMode = !freezeMode;
            Debug.Log("Modo de congelamiento en área: " + freezeMode);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (areaDamageMode && other.CompareTag("Enemy") && boomerangController.isFlying && !boomerangController.onHand)
        {
            Debug.Log("COLLISION CON DAÑO EN AREA");
            // Realizar daño en área
            Collider[] colliders = Physics.OverlapSphere(transform.position, areaDamageRadius);
            foreach (Collider collider in colliders)
            {
                
                // Aplicar daño al enemigo
                //para evisar el dñao duplicado en el target original, excluimos el target hiteado
                if (collider.CompareTag("Enemy")/* && other.gameObject.name != collider.gameObject.name*/)
                {
                    boomerangController.MakeDamageToEnemyAndPush(collider,boomerangController.damageBoomerang);
                    Debug.Log("collider name = " + collider.gameObject.name);
                    // Realizar el daño
                    //HACEMOS DAÑO AL ENEMIGO MEDIANTE LA INTERFAZ
                    /*IDamageable damageableObject = collider.GetComponent<IDamageable>();
                    if (damageableObject != null)
                    {
                        print("collision con el enemigo");
                        Damage damageObj = new Damage();
                        damageObj.amount = 5;
                        damageObj.source = UnitType.Player;
                        damageObj.targetType = TargetType.Single;
                        //llamamos al metodo de la interfaz
                        //DoDamage(damageableObject, damageObj);
                        damageableObject.ReceiveDamage(damageObj);
                        //damageableObject.ReceiveDamage(damageObj);

                        Renderer hitRenderer = other.GetComponentInChildren<Renderer>();
                        // Cambiar el color del material del renderer
                        if (hitRenderer != null)
                        {
                            hitRenderer.material.color = Color.blue;
                        }
                        //mostramos la UI de daño inflingido
                        //DealDamageToEnemy(damage);
                    }*/
                }
            }
            areaDamageMode = false;
            //evitamos que golpee al mismo enemigo 2 veces seguidas
            boomerangController.lastHitTime = Time.time;
            //Invoke(nameof(DisableMode), 0.5f);
        }

        if (freezeMode && other.CompareTag("Enemy"))
        {
            // Congelar al enemigo
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("enemy" + other.gameObject.name + "freezed");
                other.GetComponent<Enemy>().Freeze(freezeDuration);
                other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
        }
        freezeMode = false;
    }
    private void DisableMode()
    {
        areaDamageMode = false;
        freezeMode = false;
    }
}
