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
    public int damageOnAreaDamage = 3;
    public int damageOnFreeze = 1;
    private int currentChargesAreaDamage;
    private int currentChargesFreeze;
    BoomerangController boomerangController;
    ManageBoomerangParticles boomerangParticlesManager;

    //Gameobject para manejar la particula cuando se activa un efecto
    public GameObject managerOfEffectGO;

    private bool effectActivated = false;
    private AreaOfEffect aoe;
    private void Start()
    {
        //recuperamos el manager de particulas
        aoe = GetComponent<AreaOfEffect>();
        boomerangParticlesManager = GetComponent<ManageBoomerangParticles>();
        boomerangController = GetComponent<BoomerangController>();
        currentChargesAreaDamage = 2;
        currentChargesFreeze = 2;
    }
    void Update()
    {
        //SE PUEDE CAMBIAR ENTRE UNO Y OTRO, PERO SOLO SE MANTIENE UNO ACTIVO A LA VEZ
        // Activa el modo de daño en área al presionar la tecla Q
        if (Input.GetKeyDown(KeyCode.Q) && currentChargesAreaDamage > 0 && !effectActivated)
        {//si activas el efecto de daño en area
            currentChargesAreaDamage--;
            areaDamageMode = !areaDamageMode;

            //activamos el efecto en el boomerang
            managerOfEffectGO = boomerangParticlesManager.InstantiateExplosionEffect();
            Debug.Log("Modo de daño en área: " + areaDamageMode);
            effectActivated = true;
        }else if (Input.GetKeyDown(KeyCode.Q) && currentChargesAreaDamage > 0 && effectActivated && freezeMode)
        {//si tienes activado el freeze, pero quieres cambiar a daño en area en su lugar
            freezeMode = !freezeMode;
            areaDamageMode = !areaDamageMode;
            currentChargesFreeze++;
            currentChargesAreaDamage--;

            GameObject copy = managerOfEffectGO;
            Destroy(copy, 0.2f);
            managerOfEffectGO = null;
            managerOfEffectGO = boomerangParticlesManager.InstantiateExplosionEffect();
        }

        // Activa el modo de congelamiento en área al presionar la tecla Z
        if (Input.GetKeyDown(KeyCode.Z) && currentChargesFreeze > 0 && !effectActivated)
        {//si activas el modo freeze
            currentChargesFreeze--;
            freezeMode = !freezeMode;

            //activamos el efecto en el boomerang
            managerOfEffectGO = boomerangParticlesManager.InstantiateFreezeEffect();
            Debug.Log("Modo de congelamiento en área: " + freezeMode);
            effectActivated = true;
        }//si tienes activado el daño en area y quieres activar el freeze en su lugar
        else if (Input.GetKeyDown(KeyCode.Z) && currentChargesFreeze > 0 && effectActivated && areaDamageMode)
        {
            freezeMode = !freezeMode;
            areaDamageMode = !areaDamageMode;
            currentChargesAreaDamage++;
            currentChargesFreeze--;

            GameObject copy = managerOfEffectGO;
            Destroy(copy, 0.2f);
            managerOfEffectGO = null;
            managerOfEffectGO = boomerangParticlesManager.InstantiateFreezeEffect();
        }
        //verificamos si el efecto de particulas es diferente de null, y en el caso correcto, lo movemos a la posicion del boomerang
        if (managerOfEffectGO != null)
        {
            managerOfEffectGO.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (areaDamageMode && other.CompareTag("Enemy") && boomerangController.isFlying && !boomerangController.onHand ||
            areaDamageMode && other.CompareTag("Enemy") && !boomerangController.isFlying && !boomerangController.onHand &&
            boomerangController.bouncing)
        {
            boomerangParticlesManager.InstantiateExplosionParticle();
            List<LivingEntity> damageablesFound = aoe.GetTargets<LivingEntity>();
            Debug.Log("lenght list = " + damageablesFound.Count);
            /*for (int i = 0; i < damageablesFound.Count; i++)
            {
                Debug.Log(damageablesFound[i]);
            }*/
            foreach (LivingEntity damageable in damageablesFound)
            {
                Debug.Log("enemy hited = " + damageable.name);
                IDamageable iDamageable = damageable.GetComponent<IDamageable>();
                if (iDamageable != null)
                {
                    if (boomerangController.specialThrow)
                    {
                        ApplyDamageToEnemy(damageable, damageOnAreaDamage);
                    }
                    else
                    {
                        ApplyDamageToEnemy(damageable, damageOnAreaDamage);
                        if (boomerangController.bouncing)
                        {
                            boomerangController.bouncing = false;
                        }
                        boomerangController.Return();
                    }
                    //ApplyDamageToEnemy(damageable);
                }
            }
            //boomerangParticlesManager.InstantiateExplosionParticle();
            //Debug.Log("COLLISION CON DAÑO EN AREA");
            // Realizar daño en área
            //Collider[] colliders = Physics.OverlapSphere(transform.position, areaDamageRadius);
            /*foreach (Collider collider in colliders)
            {*/

            // Aplicar daño al enemigo
            //para evisar el dñao duplicado en el target original, excluimos el target hiteado
            //if (collider.CompareTag("Enemy")/* && other.gameObject.name != collider.gameObject.name*/)
            //{
            //boomerangController.MakeDamageToEnemyAndPush(collider,boomerangController.damageBoomerang);
            //Debug.Log("collider name = " + collider.gameObject.name);
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
            //}
            //}
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
            //Congelar al enemigo unitarget
            /*if (other.CompareTag("Enemy"))
            {
                Debug.Log("enemy" + other.gameObject.name + "freezed");
                other.GetComponent<Enemy>().Freeze(freezeDuration);
                other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }*/
            //multitarget
            List<LivingEntity> damageablesFound = aoe.GetTargets<LivingEntity>();
            foreach (LivingEntity damageable in damageablesFound)
            {
                if (other.CompareTag("Enemy"))
                {
                    Debug.Log("enemy" + damageable.gameObject.name + "freezed");
                    //damageable.gameObject.GetComponent<Enemy>().Freeze(freezeDuration);
                    damageable.gameObject.GetComponent<EnemyBase>().Freeze(freezeDuration);
                    //preguntarle a mauricio como hacer para detener el chasing del enemigo temporalmente
                    //damageable.gameObject.GetComponent<NavMeshAgent>().enabled = false;

                    if (damageable != null)
                    {
                        ApplyDamageToEnemy(damageable, damageOnFreeze);
                    }
                }
            }
            //hacemos una copia y el original lo ponemos a null
            GameObject copy = managerOfEffectGO;
            Destroy(copy, 0.2f);
            managerOfEffectGO = null;
            effectActivated = false;
            freezeMode = false;
        }
        
    }
    private void ApplyDamageToEnemy(LivingEntity damageable, int damage)
    {
        Damage damageObj = new Damage();
        damageObj.amount = damage;
        damageObj.source = UnitType.Player;
        damageObj.targetType = TargetType.Area;
        // Obtener la direccion opuesta a la normal de la colisi�n
        Vector3 normal = damageable.gameObject.transform.position - transform.position;
        normal.y = 0;
        normal.Normalize();
        damageObj.forceImpulse = normal * boomerangController.impulseForceWhenHit;
        boomerangController.DoDamage(damageable, damageObj);
        boomerangController.DealDamageToEnemy(damageObj.amount);
    }
    private void DisableMode()
    {
        areaDamageMode = false;
        freezeMode = false;
    }
}
