using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//LANZAMIENTO BOOMERANG FUNCIONAL CON AUMENTO DE FUERZA Y LINERENDERER ADAPTATIVO
public class BoomerangController : MonoBehaviour, IDamager
{
    // Variables públicas
    //public float damage;
    public float damageBoomerang;
    public float lastHitTime;
    public float coldownHit;
    public float manaCostOfAttractingBoomerang;

    // Variables privadas
    private Vector3 initialPosition;
    private Vector3 returnPosition;
    [HideInInspector]
    public bool isFlying = false;
    [HideInInspector]
    public bool isReturning = false;
    private Rigidbody rb;
    private LineRenderer lr;

    [HideInInspector]
    public bool onHand = true;
    public Transform handPlace;
    [HideInInspector]
    public bool rotation = false;

    [HideInInspector]
    public bool specialThrow = false;

    [HideInInspector]
    public bool bouncing = false;

    [HideInInspector]
    public bool onGround = false;

    //UI damage dealt
    public GameObject floatingDamageTextPrefab;
    [HideInInspector]
    public bool attracting;
    PlayerStats playerStats;
    [HideInInspector]
    public bool onColdown = false;

    //interpolacion velocidad boomerang
    //public float slowdownFactor = 0.5f; // Factor de desaceleración en el medio
    //private float currentLerpTime = 0.0f; // Tiempo de interpolación actual


    public float impulseForceWhenHit;
    public float rotationSpeed;

    UseBoomerang boomerangPlayer;
    BoomerangUpgradeController boomerangUpgradeController;
    //prefab efecto boomerang
    public GameObject prefabBoomerangRotation;
    private GameObject boomerangEffectInstantiated;

    // Inicialización
    private void Awake()
    {
        floatingDamageTextPrefab = FindObjectOfType<FloatingDamageText>().gameObject;
    }
    void Start()
    {
        boomerangUpgradeController = GetComponent<BoomerangUpgradeController>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();

        onHand = true;
        gameObject.transform.localRotation = Quaternion.identity;
        //playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerStats = FindObjectOfType<PlayerStats>();
        //boomerangPlayer = GameObject.Find("Player").GetComponent<UseBoomerang>();
        boomerangPlayer = FindObjectOfType<UseBoomerang>();
        damageBoomerang = boomerangPlayer.damage;
    }

    // Actualización por fotograma
    void Update()
    {

        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            Debug.Log("EL BOOMERANG HACE COSAS!");

            // Seguimiento de la línea
            if (isFlying && !isReturning)
            {
                Debug.Log("EL BOOMERANG SE MUEVE");
                if (boomerangPlayer.targetPosition != Vector3.zero)
                {

                    // Calcular el nuevo desplazamiento en cada frame
                    Debug.Log("EL BOOMERANG SE MUEVE");

                    float t = Mathf.Clamp01(boomerangPlayer.followSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, boomerangPlayer.targetPosition, t);
                }
            }

            // Comprobar la distancia del Boomerang
            if (Vector3.Distance(transform.position, initialPosition) > boomerangPlayer.maxDistance && !isReturning && !specialThrow && !bouncing & !onGround ||
                Vector3.Distance(transform.position, boomerangPlayer.targetPosition) < boomerangPlayer.minDistance && !specialThrow && !bouncing && !onGround)
            {
                Debug.Log("returning");
                Return();
            }
            if (bouncing)
            {
                boomerangPlayer.expectedColdownTime = 0f;
                // Desplazar gradualmente hacia la posición de destino usando Lerp
                Vector3 newPosition = Vector3.Lerp(transform.position, boomerangPlayer.targetPosition, boomerangPlayer.followSpeed * Time.deltaTime);
                newPosition.y = transform.position.y; // Mantener la posición actual en el eje Y
                transform.position = newPosition;

                // Aplicar gravedad después de haberse desplazado a la posición de destino
                if (Vector3.Distance(transform.position, boomerangPlayer.targetPosition) <= boomerangPlayer.minDistance)
                {
                    Debug.Log("llegue al destino");
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    bouncing = false;
                    onGround = true;
                    rotation = false;
                    rb.isKinematic = false;
                    GetComponent<Collider>().isTrigger = false;
                }
            }
        }

    }
    private void LateUpdate()
    {
    }

    // Actualizar la posición del Boomerang mientras está en vuelo
    void FixedUpdate()
    {
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            //APAÑO PORQUE NO DETECTA BIEN LA COLISION ENTRE EL CHARACTER CONTROLLER Y EL BOX COLLIDER
            if (Vector3.Distance(transform.position, handPlace.position) <= boomerangPlayer.minDistance + 1 && !isFlying && !specialThrow ||
                Vector3.Distance(transform.position, handPlace.position) <= boomerangPlayer.minDistance + 1 && isFlying && isReturning && !specialThrow)
            {
                rb.useGravity = false;
                GetComponent<Collider>().isTrigger = true;
                transform.SetParent(handPlace.transform);
                onHand = true;
            }
            if (isReturning)
            {
                //endPoint = Vector3.zero;
                //asi vuelve siempre al jugador
                returnPosition = handPlace.position;
                float t = Mathf.Clamp01(boomerangPlayer.followSpeed * Time.fixedDeltaTime);
                transform.position = Vector3.Lerp(transform.position, returnPosition, t);
                //transform.position = Vector3.MoveTowards(transform.position, returnPosition, followSpeed * Time.fixedDeltaTime);
                //VUELVE A LA MANO DEL JUGADOR
                //print(Vector3.Distance(transform.position, returnPosition));
                if (Vector3.Distance(transform.position, returnPosition) <= boomerangPlayer.minDistance)
                {
                    Debug.Log("returned 1");
                    //endPoint = Vector3.zero;
                    isReturning = false;
                    isFlying = false;
                    transform.position = initialPosition;
                    rb.velocity = Vector3.zero;
                    onHand = true;
                    transform.SetParent(handPlace.transform);
                    //si lo atraemos con poder psiquico, gastamos mana y ponemos attracing a false
                    if (attracting)
                    {
                        if (playerStats.currentMana >= manaCostOfAttractingBoomerang)
                        {
                            playerStats.UseSkill(manaCostOfAttractingBoomerang);
                            
                        }
                        attracting = false;
                    }
                    //rb.isKinematic = false;
                }
                //SE QUEDA EN EL SUELO EN LA POSICION INICIAL DE LANZAMIENTO
                /*if (Vector3.Distance(transform.position, returnPosition) <= minDistance)
                {
                    Debug.Log("returned 2");
                    isReturning = false;
                    isFlying = false;
                    rb.velocity = Vector3.zero;
                    rb.useGravity = true;
                    rotation = false;
                    rb.isKinematic = false;
                    //transform.SetParent(handPlace.transform);
                    GetComponent<Collider>().isTrigger = false;
                }*/
            }
            if (rotation)
            {
                Debug.Log("rotando...");
                Vector3 rotacionesActuales = transform.rotation.eulerAngles;
                float rotacionY = rotacionesActuales.y + rotationSpeed * Time.fixedDeltaTime;

                transform.rotation = Quaternion.Euler(rotacionesActuales.x, rotacionY, rotacionesActuales.z);

               /* // Agregar rotaciones adicionales en los ejes Y y Z
                float rotacionY = rotacionesActuales.y + velocidadRotacion * Time.deltaTime;
                float rotacionZ = rotacionesActuales.z + velocidadRotacion * Time.deltaTime;

                // Actualizar las rotaciones del objeto
                transform.rotation = Quaternion.Euler(rotacionesActuales.x, rotacionY, rotacionZ);*/
            }
            if (onHand)
            {
                onColdown = false;
                onGround = false;
                rotation = false;
                bouncing = false;
                rb.isKinematic = true;
                transform.position = handPlace.position;
                //AÑADIDO POSTERIORMENTE CUANDO LO ATRAES CON PODER MENTAL USANDO ISRETURNING
                GetComponent<Rigidbody>().useGravity = true; 
            }
        }
    }
    // Volver a la posición inicial
    void Return()
    {
        isReturning = true;
        //asi vuelve en linea recta a la posicion inicial de lanzamiento
        returnPosition = initialPosition;
    }
    // Detectar colisiones
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFlying || collision.gameObject.CompareTag("Player") && isFlying && isReturning)
        {
            Debug.Log("Player");
            rb.useGravity = false;
            GetComponent<Collider>().isTrigger = true;
            onHand = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && !onHand && isFlying && !boomerangUpgradeController.areaDamageMode 
            || other.CompareTag("Enemy") && !onHand && bouncing && !boomerangUpgradeController.areaDamageMode ||
            other.CompareTag("Enemy") && !onHand && specialThrow)
        {
            //damageBoomerang = boomerangPlayer.damage;
            if (Time.time - lastHitTime > coldownHit)
            {
                lastHitTime = Time.time;
                if (specialThrow)
                {
                    //MakeDamageToEnemyAndPush(other, damage);
                    MakeDamageToEnemyAndPush(other, boomerangPlayer.damage);
                }
                else
                {
                    //MakeDamageToEnemyAndPush(other, damage);
                    MakeDamageToEnemyAndPush(other, boomerangPlayer.damage);
                    //en caso de que lo golpee con un rebote, ponemos a false bouncing
                    if (bouncing)
                    {
                        bouncing = false;
                    }
                    //vuelve el boomerang
                    Return();
                }
            }
        }
        if (other.CompareTag("Obstacle") && !isReturning && isFlying || other.CompareTag("Obstacle") && specialThrow)
        {
            onColdown = false;
            if (specialThrow)
            {
                //AÑADIR CODIGO PARA CUANDO CHOCA CON UN MURO Y QUE NO REBOTE (Y SE QUEDE EN EL SUELO SIN DAR ERRORES
                specialThrow = false;
                lr.positionCount = 0;
                GetComponent<FollowLine>().waypoints.Clear();

                //lo dejamos en el suelo
                Debug.Log("colision obstaculo y queda en el suelo");
                GetComponent<Rigidbody>().useGravity = true;

            }
            Debug.Log("Obstacle");
            isFlying = false;

            if (!bouncing)
            {
                bouncing = true;
                boomerangPlayer.targetPosition = boomerangPlayer.localTargetPosition;

                Debug.Log("target = " + boomerangPlayer.targetPosition);
            }
        }
        //collision con el player
        if (other.CompareTag("Player") && !isFlying || other.CompareTag("Player") && isFlying && isReturning)
        {
            rb.useGravity = false;
            GetComponent<Collider>().isTrigger = true;
            onHand = true;
            transform.SetParent(handPlace.transform);
        }
    }
    public void MakeDamageToEnemyAndPush(Collider other, float damage)
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
            //llamamos al metodo de la interfaz
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

            // Obtener la dirección opuesta a la normal de la colisión
            Vector3 normal = transform.position + other.gameObject.transform.position;
            normal = Vector3.Normalize(normal);
            normal.y = 0;
            temporalRb.AddForce(normal * impulseForceWhenHit, ForceMode.Impulse);
            Destroy(temporalRb, 0.5f);
        }
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
    public float BoomerangHeight()
    {
        return handPlace.position.y;
    }
    //metodo de la interfaz IDamager
    public void DoDamage(IDamageable target, Damage damage)
    {
        target.ReceiveDamage(damage);
    }
}
