using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PsychicBall : MonoBehaviour, IDamager
{
    public int damage;
    public float velocity;
    public float maxDistance = 15f;
    public bool throwed = false;
    public float impulseForce;

    private float lifeTime;
    public GameObject floatingDamageTextPrefab;
    private void Awake()
    {
        floatingDamageTextPrefab = FindObjectOfType<FloatingDamageText>().gameObject;
    }
    void Update()
    {
        if (throwed)
        {
            lifeTime = maxDistance / velocity;
            Debug.Log("lifetime = " + lifeTime);
            Destroy(gameObject, lifeTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy name = " + other.gameObject.name);
            MakeDamageToEnemyAndPush(other, damage);
        }
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("object name = " + other.gameObject.name);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy name = " + collision.gameObject.name);
        }
    }
    void MakeDamageToEnemyAndPush(Collider other, int damage)
    {
        //HACEMOS DAÑO AL ENEMIGO MEDIANTE LA INTERFAZ
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            print("collision con el enemigo");
            Damage damageObj = new Damage();
            damageObj.amount = damage;
            damageObj.source = UnitType.Player;
            damageObj.targetType = TargetType.Single;
            //usamos el metodo DoDamage de IDamager
            DoDamage(damageableObject, damageObj);
            //damageableObject.ReceiveDamage(damageObj);

            Renderer hitRenderer = other.GetComponentInChildren<Renderer>();
            // Cambiar el color del material del renderer
            if (hitRenderer != null)
            {
                hitRenderer.material.color = Color.blue;
            }
            //mostramos la UI de daño inflingido
            DealDamageToEnemy(damage);
        }
        if (!other.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody temporalRb = other.gameObject.AddComponent<Rigidbody>();
            temporalRb.useGravity = false;
            //dividimos la fuerza entre 2 porque no usamos gravedad
            //temporalRb.AddForce(Vector3.forward, ForceMode.Impulse);
            // Obtener la dirección opuesta a la normal de la colisión
            Vector3 normal = (transform.position + other.gameObject.transform.position);
            normal = Vector3.Normalize(normal);
            normal.y = 0;
            temporalRb.AddForce(normal * impulseForce, ForceMode.Impulse);
            Destroy(temporalRb, 0.5f);
            Destroy(this.gameObject);
        }
    }
    //metodo de la interfaz IDamager
    public void DoDamage(IDamageable target, Damage damage)
    {
        target.ReceiveDamage(damage);
    }
    //AÑADIR EL DAÑO INFLINGIDO CON LOS FLOATINGTEXT DE DAÑO
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
               // Configura el texto y el color del daño infligidos
               floatingDamageText.SetDamageText(" - " + damage.ToString(), Color.red);
           }
       }
    }
}
