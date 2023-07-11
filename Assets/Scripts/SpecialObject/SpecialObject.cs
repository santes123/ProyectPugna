using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObject : MonoBehaviour, IDamager
{
    public bool estaSiendoAtraido = false;
    public bool haSidoLanzado = false;
    //public float velocidadAtraccion;
    float velocidadAtraccionOriginal;
    //public float fuerzaBase;
    //float fuerzaLanzamiento;
    //float fuerzaLanzamientoAnterior;
    //Vector3 direccionLanzamientoAnterior;
    //public float fuerzaMaxima;
    //private float tiempoPulsado = 0f;
    //public float incrementoDeFuerzaPorSegundo;
    float distanceToTakeOnHand = 2f;
    public Transform player;
    public Transform handPlace;
    //public Transform pointer;
    public Rigidbody rb;
    public bool onHand = false;
    //public LayerMask collisionMask;

    public float velocidadRotacion;

    public float fuerzaRebote;
    public float fuerzaAleatoria;

    public float damage;
    //bool botonPresionado = false;
    public bool onColdown = false;
    private UseAttractThrowSkill useSkil;

    public List<string> enemiesHited;
    public GameObject floatingDamageTextPrefab;

    //BoomerangController boomerangReference;
    private void Awake()
    {
        floatingDamageTextPrefab = FindObjectOfType<FloatingDamageText>().gameObject;
    }
    void Start()
    {
        //useSkil = GameObject.Find("Player").GetComponent<UseAttractThrowSkill>();
        useSkil = FindObjectOfType<UseAttractThrowSkill>();
        //player = FindObjectOfType<PlayerStats>().gameObject.transform;
        rb = GetComponent<Rigidbody>();
        velocidadAtraccionOriginal = useSkil.velocidadAtraccion;
        enemiesHited = new List<string>();
        //cambiar por un metodo con mejor accesibilidad (usando algun script que tenga guardado la referencia)
        handPlace = FindObjectOfType<UseAttractThrowSkill>().handPlace;
        //handPlace = GameObject.Find("hand_right").transform;
        //fuerzaLanzamiento = fuerzaBase;

        //boomerangReference = GameObject.Find("Boomer").GetComponent<BoomerangController>();
        StartCoroutine(FindPlayer());
    }

    void Update()
    {/*
        //Debug.DrawRay(player.position, player.forward, Color.red, Mathf.Infinity);
        if (Input.GetMouseButtonDown(0) && CheckIfRayHitObject() && !useSkil.onColdown)
        {
            estaSiendoAtraido = true;
            onColdown = true;
            useSkil.onColdown = true;
        }
        if (Input.GetMouseButton(0))
        {
            velocidadAtraccion += 2;
        */
        if (player != null)
        {
            if (onHand)
            {
                transform.position = handPlace.position;
                //reiniciamos la velocidad de atraccion al valor base
                useSkil.velocidadAtraccion = velocidadAtraccionOriginal;
                // Obtener las rotaciones actuales en los tres ejes
                Vector3 rotacionesActuales = transform.rotation.eulerAngles;

                // Agregar rotaciones adicionales en los ejes Y y Z
                float rotacionY = rotacionesActuales.y + velocidadRotacion * Time.deltaTime;
                float rotacionZ = rotacionesActuales.z + velocidadRotacion * Time.deltaTime;

                // Actualizar las rotaciones del objeto
                //transform.rotation = Quaternion.Euler(rotacionesActuales.x, rotacionY, rotacionZ);
                transform.rotation = Quaternion.Euler(rotacionesActuales.x, rotacionY, rotacionesActuales.z);
            }
        }
        /*
        //Control del tiempo pulsado al lanzar, para luego calcular la fuerza
        if (Input.GetMouseButtonDown(1))
        {
            botonPresionado = true;
        }
        if (botonPresionado)
        {
            tiempoPulsado += Time.deltaTime;
        }*/
        
    }
    void FixedUpdate()
    {
        //ATRACCION
        if (player != null)
        {
            if (useSkil.estaSiendoAtraido && !useSkil.haSidoLanzado && estaSiendoAtraido/* && !boomerangReference.onHand*/)
            {
                if (Vector3.Distance(handPlace.position, transform.position) <= distanceToTakeOnHand)
                {
                    //CALCULAR MANA POR SEGUNDO PULSADO Y HACER PARA QUE SI YA TIENES UNO EN LA MANO O ESTAS ATRAYENDO UNO, NO PODER ATRAER OTRO
                    print("on hand");
                    useSkil.estaSiendoAtraido = false;
                    //estaSiendoAtraido = false;
                    useSkil.onHand = true;
                    onHand = true;
                    GetComponent<BoxCollider>().enabled = false;
                    //GetComponent<MeshCollider>().enabled = false;
                    rb.useGravity = false;
                    //consumimos mana fijo por ahora
                    if (useSkil.finalManaCost > useSkil.maxManaCost)
                    {
                        useSkil.finalManaCost = useSkil.manaCost;
                    }
                    player.GetComponent<PlayerStats>().UseSkill(useSkil.finalManaCost);
                    useSkil.finalManaCost = 0f;
                    useSkil.chargeBar.SetActive(false);
                }
                else
                {
                    Vector3 posicionJugador = player.position;
                    Vector3 direccion = (posicionJugador - transform.position).normalized;
                    rb.MovePosition(transform.position + direccion * useSkil.velocidadAtraccion * Time.fixedDeltaTime);
                }
            }
            else
            {
                //Debug.Log("No tienes suficiente mana para seguir!");
            }
        }
 
        //LANZAMIENTO
        /*if (Input.GetMouseButtonUp(1) && estaSiendoAtraido)
        {
            onColdown = false;
            useSkil.onColdown = false;
            botonPresionado = false;
            print(tiempoPulsado);
            if (tiempoPulsado <= 0.5f)
            {
                fuerzaLanzamiento = fuerzaBase;
            }
            else
            {
                fuerzaLanzamiento = fuerzaLanzamiento + tiempoPulsado * incrementoDeFuerzaPorSegundo;
                Debug.Log("fuerza lanzamiento calculada = " + fuerzaLanzamiento);
                if (fuerzaLanzamiento >= fuerzaMaxima)
                {
                    fuerzaLanzamiento = fuerzaMaxima;
                }
            }
            Debug.Log("fuerza pre lanzamiento = " + fuerzaLanzamiento);
            transform.LookAt(pointer);

            GetComponent<Collider>().enabled = true;
            rb.useGravity = true;
            haSidoLanzado = true;
            estaSiendoAtraido = false;
            onHand = false;
            Vector3 direccionLanzamiento = (pointer.position - transform.position).normalized;
            rb.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode.Impulse);
            direccionLanzamientoAnterior = direccionLanzamiento;
            fuerzaLanzamientoAnterior = fuerzaLanzamiento;
            fuerzaLanzamiento = fuerzaBase;
            Debug.Log("fuerza post lanzamiento = " + fuerzaLanzamiento);
            tiempoPulsado = 0f;
        }*/
    }
    /*private bool CheckIfRayHitObject()
    {
        Ray ray = new Ray(player.position, player.forward);
        RaycastHit hit;
        //Debug.DrawRay(player.position, player.forward, Color.red, Mathf.Infinity);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask, QueryTriggerInteraction.Collide))
        {
            
            Debug.DrawRay(player.position, player.forward * hit.distance, Color.green);
            print("collision con un objeto atraible");
            return true;
        }
        else
        {
            Debug.DrawRay(player.position, player.forward * 20f, Color.red);
            return false;
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemiesHited.Contains(other.gameObject.name) && haSidoLanzado)
        {
            //añadimos el array a enemigos hiteados por este gameobject
            enemiesHited.Add(other.gameObject.name);

            //APLICAMOS FUERZA AL ENEMIGO
            //other.gameObject.GetComponent<Rigidbody>().AddForce(rb.velocity, ForceMode.Impulse);
            // Comprobar si el objeto colisionado no tiene un Rigidbody
            if (!other.gameObject.GetComponent<Rigidbody>())
            {
                Rigidbody temporalRb = other.gameObject.AddComponent<Rigidbody>();
                temporalRb.useGravity = false;
                //dividimos la fuerza entre 2 porque no usamos gravedad
                temporalRb.AddForce(useSkil.direccionLanzamientoAnterior * useSkil.fuerzaLanzamientoAnterior / 2, ForceMode.Impulse);
                Destroy(temporalRb, 0.5f);
                DealDamageToEnemy(damage);
            }

            //HACEMOS REBOTAR EL OBJETO EN EL ENEMIGO Y RESETEAMOS LA SKILL
            rb.Sleep();
            Invoke("ResetSkill", 1f);
            print("ENEMY");/*
            //DIRECCION OPUESTA
            // Obtener la normal de la colisión
            Vector3 normal = other.transform.position - transform.position;

            // Obtener la dirección opuesta a la normal de la colisión
            Vector3 direccionRebote = Vector3.Reflect(transform.forward, normal).normalized;

            // Aplicar una fuerza al objeto en la dirección opuesta
            rb.AddForce(direccionRebote * fuerzaRebote, ForceMode.Impulse);
            */
            rb.WakeUp();
            //DIRECCION ALEATORIA
            //direccion random x,y,z
            Vector3 direccionAleatoria = Random.insideUnitSphere.normalized;
            //direccion random x z
            Vector3 direccion = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(direccion * fuerzaAleatoria, ForceMode.Impulse);
            Vector3 normal = other.transform.position - transform.position;
            Vector3 direccionRebote = Vector3.Reflect(direccion, normal).normalized;
            rb.AddForce(direccionRebote * fuerzaRebote, ForceMode.Impulse);
            
            

            //HACEMOS DAÑO AL ENEMIGO MEDIANTE LA INTERFAZ
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                print("collision con el enemigo");
                Damage damageObj = new Damage();
                damageObj.amount = (int)damage;
                damageObj.source = UnitType.Player;
                damageObj.targetType = TargetType.Single;

                //llamamos a la interfaz IDamager
                DoDamage(damageableObject, damageObj);
                //damageableObject.ReceiveDamage(damageObj);
                Renderer hitRenderer = other.GetComponentInChildren<Renderer>();
                // Cambiar el color del material del renderer
                if (hitRenderer != null)
                {
                    hitRenderer.material.color = Color.blue;
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Invoke("ResetSkill", 1f);
        }
    }
    private void ResetSkill()
    {
        haSidoLanzado = false;
        enemiesHited.Clear();
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
                // Configura el texto y el color del daño infligidos
                floatingDamageText.SetDamageText(" - " + damage.ToString(), Color.red);
            }
        }
    }
    //metodo de la interfaz IDamager
    public void DoDamage(IDamageable target, Damage damage)
    {
        target.ReceiveDamage(damage);
    }

    public void Attack() {
    }
    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            if (FindObjectOfType<PlayerStats>())
            {
                player = FindObjectOfType<PlayerStats>().gameObject.transform;
                if (player != null) yield return new WaitForSeconds(0.5f);
            }
 
            yield return null;
        }
    }
}
