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
    ManageBoomerangParticles boomerangParticlesManager;

    //Gameobject para manejar la particula cuando se activa un efecto
    GameObject managerOfEffectGO;

    private bool effectActivated = false;
    private void Start()
    {
        //recuperamos el manager de particulas
        boomerangParticlesManager = GetComponent<ManageBoomerangParticles>();
        boomerangController = GetComponent<BoomerangController>();
        currentChargesAreaDamage = 2;
        currentChargesFreeze = 2;
    }
    void Update()
    {
        //EVITAR QUE SE ACTIVEN LOS 2 A LA VEZ
        // Activa el modo de daño en área al presionar la tecla Q
        if (Input.GetKeyDown(KeyCode.Q) && currentChargesAreaDamage > 0 && !effectActivated)
        {
            currentChargesAreaDamage--;
            areaDamageMode = !areaDamageMode;

            //activamos el efecto en el boomerang
            managerOfEffectGO = boomerangParticlesManager.InstantiateExplosionEffect();
            Debug.Log("Modo de daño en área: " + areaDamageMode);
            effectActivated = true;
        }

        // Activa el modo de congelamiento en área al presionar la tecla Z
        if (Input.GetKeyDown(KeyCode.Z) && currentChargesFreeze > 0 && !effectActivated)
        {
            currentChargesFreeze--;
            freezeMode = !freezeMode;

            //activamos el efecto en el boomerang
            managerOfEffectGO = boomerangParticlesManager.InstantiateFreezeEffect();
            Debug.Log("Modo de congelamiento en área: " + freezeMode);
            effectActivated = true;
        }
        //verificamos si el efecto de particulas es diferente de null, y en el caso correcto, lo movemos a la posicion del boomerang
        if (managerOfEffectGO != null)
        {
            managerOfEffectGO.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (areaDamageMode && other.CompareTag("Enemy") && boomerangController.isFlying && !boomerangController.onHand)
        {
            boomerangParticlesManager.InstantiateExplosionParticle();
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
            //hacemos una copia y el original lo ponemos a null
            GameObject copy = managerOfEffectGO;
            Destroy(copy, 0.2f);
            managerOfEffectGO = null;
            areaDamageMode = false;
            //evitamos que golpee al mismo enemigo 2 veces seguidas
            boomerangController.lastHitTime = Time.time;
            //Invoke(nameof(DisableMode), 0.5f);
            effectActivated = false;
        }

        if (freezeMode && other.CompareTag("Enemy"))
        {
            boomerangParticlesManager.InstantiateFreezeParticle();
            // Congelar al enemigo
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("enemy" + other.gameObject.name + "freezed");
                other.GetComponent<Enemy>().Freeze(freezeDuration);
                other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            //hacemos una copia y el original lo ponemos a null
            GameObject copy = managerOfEffectGO;
            Destroy(copy, 0.2f);
            managerOfEffectGO = null;
            effectActivated = false;
        }
        freezeMode = false;
    }
    private void DisableMode()
    {
        areaDamageMode = false;
        freezeMode = false;
    }
}
