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
    [HideInInspector]
    public Vector3 returnPosition;
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
    /*public GameObject prefabBoomerangRotation;
    private GameObject boomerangEffectInstantiated;*/

    //variables para gestionar rebotes multiples
    int counterBouncing = 0;
    public int maxBouncingCounter = 2;
    [HideInInspector]
    public bool updatedTiming = false;

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
        handPlace = GameObject.Find("hand_right").transform;
    }

    // Actualización por fotograma
    void Update()
    {

        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            //Debug.Log("EL BOOMERANG HACE COSAS!");

            // Seguimiento de la línea
            if (isFlying && !isReturning)
            {
                //Debug.Log("EL BOOMERANG SE MUEVE");
                if (boomerangPlayer.targetPosition != Vector3.zero)
                {

                    // Calcular el nuevo desplazamiento en cada frame
                    //Debug.Log("EL BOOMERANG SE MUEVE");

                    float t = Mathf.Clamp01(boomerangPlayer.followSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, boomerangPlayer.targetPosition, t);
                }
            }

            // Comprobar la distancia del Boomerang
            if (Vector3.Distance(transform.position, initialPosition) > boomerangPlayer.maxDistance && !isReturning && !specialThrow && !bouncing & !onGround ||
                Vector3.Distance(transform.position, boomerangPlayer.targetPosition) < boomerangPlayer.minDistance && !specialThrow && !bouncing && !onGround)
            {

 
                //Debug.Log("transform.position = " + transform.position);
                Debug.Log("distancia entre boomerang e initialPosition = " + Vector3.Distance(transform.position, initialPosition));
                //Debug.Log("initialPosition = " + initialPosition);
                Debug.Log("maxDistance = " + boomerangPlayer.maxDistance);
                Debug.Log("distancia entre boomerang y targetPosition = " + Vector3.Distance(transform.position, boomerangPlayer.targetPosition));
                //Debug.Log("targetPosition = " + boomerangPlayer.targetPosition);
                Debug.Log("minDistance = " + boomerangPlayer.minDistance);


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
                    Debug.Log("llegue al destino Bouncing");
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    bouncing = false;
                    onGround = true;
                    rotation = false;
                    rb.isKinematic = false;
                    GetComponent<Collider>().isTrigger = false;
                }
            }
            if (specialThrow & !updatedTiming)
            {
                boomerangPlayer.startedTimeThrow = Time.time;
                updatedTiming = true;
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
                    //Debug.Log("returned 1");
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
                initialPosition = transform.position;
                //AÑADIDO POSTERIORMENTE CUANDO LO ATRAES CON PODER MENTAL USANDO ISRETURNING
                GetComponent<Rigidbody>().useGravity = true; 
            }
        }
    }
    // Volver a la posición inicial
    public void Return()
    {
        Debug.Log("METODO RETURN");
        isReturning = true;
        //asi vuelve en linea recta a la posicion inicial de lanzamiento
        returnPosition = initialPosition;
    }
    // Detectar colisiones
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("EL BOOMERANG COLISIONO CON = " + collision.gameObject.name);
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
        Debug.Log("EL BOOMERANG HIZO TRIGGER CON = " + other.gameObject.name);
        if (other.CompareTag("Enemy") && !onHand && isFlying && !boomerangUpgradeController.areaDamageMode 
            || other.CompareTag("Enemy") && !onHand && bouncing && !boomerangUpgradeController.areaDamageMode ||
            other.CompareTag("Enemy") && !onHand && specialThrow)
        {
            //damageBoomerang = boomerangPlayer.damage;
            if (Time.time - lastHitTime > coldownHit)
            {
                //regeneramos el mana al golpear al enemigo
                if (playerStats.currentMana < playerStats.startingMana)
                {
                    FindObjectOfType<ManaRegeneration>().RegenerateManaOnHit();
                }
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
        if (other.CompareTag("Obstacle") && !isReturning && isFlying || other.CompareTag("Obstacle") && specialThrow ||
            other.CompareTag("Obstacle") && !isReturning && !isFlying && bouncing) // controlamos los rebotes multiples
        {
            Debug.Log("collision con un obstaculo");
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
                Debug.Log("BOUNCING POR PRIMERA VEZ");
                bouncing = true;
                counterBouncing = 0;
                counterBouncing++;
                //transform.LookAt(other.transform);
                //actualizar y crear un metodo que calcule el final point del rebote y asi poder sostener varios rebotes
                if (boomerangPlayer.localTargetPosition != Vector3.zero) boomerangPlayer.targetPosition = boomerangPlayer.localTargetPosition;

                Debug.Log("targetPosition first bounce = " + boomerangPlayer.targetPosition);
                //controlamos el bounce cuando rebote por segunda vez
            }else if (bouncing && counterBouncing < maxBouncingCounter)
            {
                Debug.Log("BOUNCING POR SEGUNDA VEZ");
                counterBouncing++;
                transform.LookAt(other.transform);
                //calculamos el punto final de trayectoria con el rebote
                //CalculateBouncingPoint();
                if(boomerangPlayer.localTargetPosition2 != Vector3.zero) boomerangPlayer.targetPosition = boomerangPlayer.localTargetPosition2;
                Debug.Log("targetPosition second bounce = " + boomerangPlayer.targetPosition);

            }
            else //filtramos para que no atraviese muros, si ha superado el numero maximos de rebotes
            {
                Debug.Log("llegue al destino");
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                bouncing = false;
                onGround = true;
                rotation = false;
                rb.isKinematic = false;
                GetComponent<Collider>().isTrigger = false;
                onColdown = false;
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

            // Obtener la direccion opuesta a la normal de la colision
            Vector3 normal = other.transform.position - transform.position;
            normal.y = 0;
            normal.Normalize();
            damageObj.forceImpulse = normal * impulseForceWhenHit;
            //llamamos al metodo de la interfaz
            DoDamage(damageableObject, damageObj);
            //damageableObject.ReceiveDamage(damageObj);

           /* Renderer hitRenderer = other.GetComponentInChildren<Renderer>();
            // Cambiar el color del material del renderer
            if (hitRenderer != null)
            {
                hitRenderer.material.color = Color.blue;
            }*/
            //mostramos la UI de daño inflingido
            DealDamageToEnemy(damage);
        }
        /*if (!other.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody temporalRb = other.gameObject.AddComponent<Rigidbody>();
            temporalRb.useGravity = false;

            // Obtener la dirección opuesta a la normal de la colisión
            Vector3 normal = transform.position + other.gameObject.transform.position;
            normal = Vector3.Normalize(normal);
            normal.y = 0;
            temporalRb.AddForce(normal * impulseForceWhenHit, ForceMode.Impulse);
            Destroy(temporalRb, 0.5f);
        }*/
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

    public void Attack() {
    }
    //funcion para calcular el punto final de rebote al chocar con un obstaculo y poder calcular multiples rebotes (codigo del LR)
    /*public void CalculateBouncingPoint()
    {
        //si colisiona con un muro el raycast, añadimos un tercer punto
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            Debug.Log("BOUNCING POR SEGUNDA VEZ RAYCAST");
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                //Vector3 point = hit.point;
                //point.y = 1f;
                //transform.LookAt(point);

                Vector3 direccionEntrante = hit.collider.gameObject.transform.position - transform.position;
                //Vector3 direccionRebote = Vector3.Reflect(direccionEntrante, hit.collider.gameObject.transform.forward.normalized);
                Vector3 direccionRebote = Vector3.Reflect(transform.forward, hit.collider.gameObject.transform.position);

                float dotProduct = Vector3.Dot(direccionEntrante.normalized, transform.right);

                //Vector3 positionA = new Vector3(transform.position.x, 0f, transform.position.z);
                //Vector3 positionB = new Vector3(hit.collider.gameObject.transform.position.x, 0f, hit.collider.gameObject.transform.position.z);
                //Vector3 direction = positionB - positionA;

                float angle = (dotProduct >= 0f) ? 45f : -45f;

                //HACER QUE EL ANGULO SEA DINAMICO ENTRE 2 VALORES PARA QUE SEA MAS PRECISO
                //AÑADIR UN AUMENO DE VELOCIDAD AL PRINCIPIO Y FINAL Y REDUCCIR LA VELOCIDAD EN EL MEDIO

                Quaternion rotacionRebote = Quaternion.Euler(0, angle, 0); // Ángulo de rebote de 45 grados

                Vector3 nuevaDireccion = rotacionRebote * direccionRebote;
                nuevaDireccion.y = transform.position.y;

                boomerangPlayer.targetPosition = transform.position + nuevaDireccion.normalized * boomerangPlayer.distanceToDisplacementOnBouncing;
                Debug.Log("target position = " + boomerangPlayer.targetPosition);
                boomerangPlayer.targetPosition.y = transform.position.y;
                boomerangPlayer.localTargetPosition = boomerangPlayer.targetPosition;

                //boomerangPlayer.targetPosition = boomerangPlayer.localTargetPosition;
            }
        }
    }*/
}
