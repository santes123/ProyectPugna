using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushAwaySkill : MonoBehaviour, IDamager
{
    //public float radius = 2.5f; // Radio de b�squeda de enemigos
    public AreaOfEffect aoe;
    public float damage = 10f; // Da�o infligido a los enemigos
    public float impulseForceWhenHit = 5f; // Fuerza de empuje hacia atr�s
    public float cooldownTime = 3f; // Tiempo de enfriamiento de la habilidad

    private bool isCooldown = false;
    private float cooldownTimer = 0f;
    public GameObject floatingDamageTextPrefab;
    public GameObject prefabAreaExplosion;
    private GameObject areaInstantiated;

    // Actualiza el estado de la habilidad
    private void Awake()
    {
        floatingDamageTextPrefab = FindObjectOfType<FloatingDamageText>().gameObject;
    }
    private void UpdateCooldown()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                GameObject auxiliar = areaInstantiated;
                Destroy(auxiliar);
                areaInstantiated = null;
                isCooldown = false;
            }
        }
    }

    // Busca enemigos en el radio y les inflige da�o y empuje hacia atr�s
    private void UseAbility()
    {
        areaInstantiated = Instantiate(prefabAreaExplosion, transform.position + Vector3.up, Quaternion.identity);
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<LivingEntity> damageablesFound = aoe.GetTargets<LivingEntity>();
        Debug.Log("lenght list = " + damageablesFound.Count);
        for (int i = 0; i < damageablesFound.Count; i++)
        {
            Debug.Log(damageablesFound[i]);
        }
        foreach (LivingEntity damageable in damageablesFound)
        {
            Debug.Log("enemy hited = " + damageable.name);
            IDamageable iDamageable = damageable.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                Damage damageObj = new Damage();
                damageObj.amount = (int)damage;
                damageObj.source = UnitType.Player;
                damageObj.targetType = TargetType.Area;
                // Obtener la direcci�n opuesta a la normal de la colisi�n
                Vector3 normal = damageable.gameObject.transform.position - transform.position;
                normal.y = 0;
                normal.Normalize();
                damageObj.forceImpulse = normal * impulseForceWhenHit;
                DoDamage(iDamageable, damageObj);
            }
        }
        //then we apply the damage on all the targets found.
        /*foreach (IDamageable damageable in damageablesFound)
        {
            DoDamage(damageable, damage);
        }*/
        /*foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                //HACEMOS DA�O AL ENEMIGO MEDIANTE LA INTERFAZ
                IDamageable damageableObject = collider.gameObject.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    print("collision con el enemigo");
                    Damage damageObj = new Damage();
                    damageObj.amount = (int)damage;
                    damageObj.source = UnitType.Player;
                    damageObj.targetType = TargetType.Single;
                    //llamamos al metodo de la interfaz
                    DoDamage(damageableObject, damageObj);
                    //damageableObject.ReceiveDamage(damageObj);

                    Renderer hitRenderer = collider.gameObject.GetComponentInChildren<Renderer>();
                    // Cambiar el color del material del renderer
                    if (hitRenderer != null)
                    {
                        hitRenderer.material.color = Color.blue;
                    }
                    //mostramos la UI de da�o inflingido
                    DealDamageToEnemy(damage);
                }
                if (!collider.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody temporalRb = collider.gameObject.AddComponent<Rigidbody>();
                    temporalRb.useGravity = false;

                    // Obtener la direcci�n opuesta a la normal de la colisi�n
                    Vector3 normal = transform.position + collider.gameObject.transform.position;
                    normal = Vector3.Normalize(normal);
                    normal.y = 0;
                    temporalRb.AddForce(normal * impulseForceWhenHit, ForceMode.Impulse);
                    Destroy(temporalRb, 0.5f);
                }
            }
        }*/

        // Inicia el tiempo de enfriamiento
        isCooldown = true;
        cooldownTimer = cooldownTime;
    }

    private void Update()
    {
        UpdateCooldown();

        // Verifica si se presion� la tecla R y si no est� en enfriamiento
        if (Input.GetKeyDown(KeyCode.R) && !isCooldown)
        {
            Debug.Log("use la habilidad");
            UseAbility();
        }
    }

    public void DoDamage(IDamageable target, Damage damage)
    {
        target.ReceiveDamage(damage);
        DealDamageToEnemy(damage.amount);
    }

    void DealDamageToEnemy(float damage)
    {
        // Calcula el daño infligido al enemigo y realiza las acciones necesarias
        if (floatingDamageTextPrefab != null)
        {
            if (!floatingDamageTextPrefab.GetComponent<Text>().isActiveAndEnabled)
            {
                floatingDamageTextPrefab.GetComponent<Text>().enabled = true;
            }
            FloatingDamageText floatingDamageText = floatingDamageTextPrefab.GetComponent<FloatingDamageText>();


            if (floatingDamageText != null)
            {
                // Configura el texto y el color del da�o infligidos
                floatingDamageText.SetDamageText(" - " + damage.ToString(), Color.red);
            }
        }
    }

    public void Attack() {
        
    }
}
