using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//LANZAMIENTO BOOMERANG FUNCIONAL CON FUERZA ESTANDAR Y LINERENDERER FIJO
public class BoomerangControllerFuncional2 : MonoBehaviour
{
    // Variables públicas
    public float followSpeed;
    public float maxDistance;
    public float minDistance;
    public float damage;
    private float lastHitTime;
    public float coldownHit;
    public float manaCostOfAttractingBoomerang = 5;

    // Variables privadas
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 localTargetPosition;
    private Vector3 returnPosition;
    public bool isFlying = false;
    private bool isReturning = false;
    private Rigidbody rb;
    private LineRenderer lr;

    public bool onHand = true;
    public Transform handPlace;
    public Transform pointer;
    public bool rotation = false;

    public bool specialThrow = false;

    bool bouncing = false;
    //float lerpSpeed = 2f;
    float distanceToMove = 5f;
    bool onGround = false;
    public Camera mainCamera;
    //UI damage dealt
    public GameObject floatingDamageTextPrefab;
    bool attracting;
    PlayerStats playerStats;
    public bool onColdown = false;

    //mouse movement and collision

    //interpolacion velocidad boomerang
    public float slowdownFactor = 0.5f; // Factor de desaceleración en el medio
    //private float currentLerpTime = 0.0f; // Tiempo de interpolación actual

    // Inicialización
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        initialPosition = transform.position;
        returnPosition = initialPosition;
        lr.positionCount = 2;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.useWorldSpace = true;
        onHand = true;
        gameObject.transform.localRotation = Quaternion.identity;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    // Actualización por fotograma
    void Update()
    {
        if (playerStats.selectedMode == GameMode.Boomerang)
        {
            //MOVER TODO EL CODIGO DE UPDATE/LATE UPDATE Y DEMAS AQUI
            if (Input.GetMouseButtonDown(0))
            {
            }
            //atraer boomerang con poder mental
            if (Input.GetMouseButton(0) && !isFlying && !onHand && !attracting && playerStats.currentMana >= 5)
            {
                isReturning = true;
                attracting = true;
                //asi vuelve en linea recta a la posicion inicial de lanzamiento
                returnPosition = handPlace.transform.position;
            }
            //Carga
            if (Input.GetMouseButton(0) && !isFlying && onHand)
            {
                lr.positionCount = 2;
                lr.enabled = true;
                Vector3 mousePosition = GetMouseWorldPosition();
                //mousePosition.y = 1f;
                Vector3 targetPositionLine = transform.position;

                lr.SetPosition(0, targetPositionLine);
                lr.SetPosition(1, new Vector3(mousePosition.x, 1f, mousePosition.z));

                //si colisiona con un muro el raycast, añadimos un tercer punto
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {

                    if (hit.collider.gameObject.CompareTag("Obstacle"))
                    {
                        lr.positionCount = 3;

                        Vector3 point = hit.point;
                        point.y = 1f;
                        transform.LookAt(point);

                        Vector3 direccionEntrante = hit.collider.gameObject.transform.position - transform.position;
                        //Vector3 direccionRebote = Vector3.Reflect(direccionEntrante, hit.collider.gameObject.transform.forward.normalized);
                        Vector3 direccionRebote = Vector3.Reflect(transform.forward, hit.collider.gameObject.transform.position);

                        float dotProduct = Vector3.Dot(direccionEntrante.normalized, transform.right);

                        //Debug.Log("dotProduc = " + dotProduct);
                        Vector3 positionA = new Vector3(transform.position.x, 0f, transform.position.z);
                        Vector3 positionB = new Vector3(hit.collider.gameObject.transform.position.x, 0f, hit.collider.gameObject.transform.position.z);
                        Vector3 direction = positionB - positionA;

                        float angle = (dotProduct >= 0f) ? 45f : -45f;

                        //float adjustedAngle = Mathf.Lerp(-50, 50, dotProduct);
                        //HACER QUE EL ANGULO SEA DINAMICO ENTRE 2 VALORES PARA QUE SEA MAS PRECISO
                        //AÑADIR UN AUMENO DE VELOCIDAD AL PRINCIPIO Y FINAL Y REDUCCIR LA VELOCIDAD EN EL MEDIO

                        Quaternion rotacionRebote = Quaternion.Euler(0, angle, 0); // Ángulo de rebote de 45 grados

                        Vector3 nuevaDireccion = rotacionRebote * direccionRebote;
                        nuevaDireccion.y = transform.position.y;

                        targetPosition = transform.position + nuevaDireccion.normalized * distanceToMove;
                        targetPosition.y = transform.position.y;
                        localTargetPosition = targetPosition;
                        lr.SetPosition(1, point);
                        lr.SetPosition(2, targetPosition);
                        //Debug.Log("Pretarget = " + targetPosition);

                    }
                }
            }
            // Lanzamiento
            if (Input.GetMouseButtonUp(0) && !isFlying && onHand)
            {
                lr.positionCount = 0;
                onHand = false;
                transform.SetParent(null);
                initialPosition = transform.position;
                rotation = true;
                Launch();
                lr.enabled = false;
                onColdown = true;
            }
            // Seguimiento de la línea
            if (isFlying && !isReturning)
            {
                if (targetPosition != Vector3.zero)
                {
                    /*
                    // Incrementa el tiempo de interpolación
                    currentLerpTime += Time.deltaTime * followSpeed;

                    // Limita el tiempo de interpolación entre 0 y 1
                    currentLerpTime = Mathf.Clamp01(currentLerpTime);

                    // Calcula el factor de desaceleración basado en el tiempo de interpolación
                    float slowdown = Mathf.Lerp(1.0f, slowdownFactor, currentLerpTime);

                    // Interpolación lineal utilizando el factor de desaceleración
                    float lerpValue = Mathf.Lerp(0.0f, 1.0f, Mathf.Pow(currentLerpTime, slowdown));*/
                    //targetPosition = targetPosition + Vector3.up;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
                }
            }

            // Comprobar la distancia del Boomerang
            if (Vector3.Distance(transform.position, initialPosition) > maxDistance && !isReturning && !specialThrow && !bouncing & !onGround ||
                Vector3.Distance(transform.position, targetPosition) < minDistance && !specialThrow && !bouncing && !onGround)
            {
                Debug.Log("returning");
                Return();
            }
            if (bouncing)
            {
                // Desplazar gradualmente hacia la posición de destino usando Lerp
                Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
                newPosition.y = transform.position.y; // Mantener la posición actual en el eje Y
                transform.position = newPosition;

                // Aplicar gravedad después de haberse desplazado a la posición de destino
                if (Vector3.Distance(transform.position, targetPosition) <= minDistance)
                {
                    Debug.Log("llegue al destino");
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    bouncing = false;
                    onGround = true;
                    rotation = false;
                    rb.isKinematic = false;
                    GetComponent<Collider>().isTrigger = false;
                    //isFlying = false;
                }
            }
        }

    }

   
    // Actualizar la posición del Boomerang mientras está en vuelo
    void FixedUpdate()
    {
        if (playerStats.selectedMode == GameMode.Boomerang)
        {
            //APAÑO PORQUE NO DETECTA BIEN LA COLISION ENTRE EL CHARACTER CONTROLLER Y EL BOX COLLIDER
            if (Vector3.Distance(transform.position, handPlace.position) <= minDistance + 1 && !isFlying && !specialThrow ||
                Vector3.Distance(transform.position, handPlace.position) <= minDistance + 1 && isFlying && isReturning && !specialThrow)
            {
                Debug.Log("Player1.1");
                rb.useGravity = false;
                GetComponent<Collider>().isTrigger = true;
                onHand = true;
            }
            if (isReturning)
            {
                //asi vuelve siempre al jugador
                returnPosition = handPlace.position;
                transform.position = Vector3.Lerp(transform.position, returnPosition, followSpeed * Time.fixedDeltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, returnPosition, followSpeed * Time.fixedDeltaTime);
                //VUELVE A LA MANO DEL JUGADOR
                //print(Vector3.Distance(transform.position, returnPosition));
                if (Vector3.Distance(transform.position, returnPosition) <= minDistance)
                {
                    Debug.Log("returned 1");
                    isReturning = false;
                    isFlying = false;
                    transform.position = initialPosition;
                    rb.velocity = Vector3.zero;
                    onHand = true;
                    transform.SetParent(handPlace.transform);
                    //si lo atraemos con poder psiquico, gastamos mana y ponemos attracing a false
                    if (attracting)
                    {
                        playerStats.UseSkill(manaCostOfAttractingBoomerang);
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
                Vector3 rotacionesActuales = transform.rotation.eulerAngles;
                float rotacionY = rotacionesActuales.y + 80f * Time.fixedDeltaTime;

                transform.rotation = Quaternion.Euler(rotacionesActuales.x, rotacionY, rotacionesActuales.z);
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
                //transform.rotation = Quaternion.identity;
                //targetPosition = Vector3.zero;
            }
        }


    }
    // Lanzar el Boomerang
    void Launch()
    {
        targetPosition = GetMouseWorldPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;
        //rb.AddForce(direction * launchForce);
        //rb.isKinematic = true;
        isFlying = true;
    }

    // Calcular la posición del cursor en el mundo
    /*Vector3 GetCursorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }*/
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return hit.point + new Vector3(0f, 1f, 0f); // adjust height of point to be above terrain
        }
        else
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            else
            {
                return Vector3.zero;
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
            //Debug.Log(Time.deltaTime);
            rb.useGravity = false;
            GetComponent<Collider>().isTrigger = true;
            onHand = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !onHand && isFlying || other.CompareTag("Enemy") && !onHand && bouncing ||
            other.CompareTag("Enemy") && !onHand && specialThrow)
        {
            if (Time.time - lastHitTime > coldownHit)
            {
                lastHitTime = Time.time;
                if (specialThrow)
                {
                    MakeDamageToEnemyAndPush(other, damage);
                }
                else
                {
                    MakeDamageToEnemyAndPush(other, damage);
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
            if (specialThrow)
            {
                //AÑADIR CODIGO PARA CUANDO CHOCA CON UN MURO Y QUE NO REBOTE (Y SE QUEDE EN EL SUELO SIN DAR ERRORES
                specialThrow = false;
                lr.positionCount = 0;
                GetComponent<FollowLine>().waypoints.Clear();

                //lo dejamos en el suelo
                Debug.Log("colision obstaculo y queda en el suelo");
                GetComponent<Rigidbody>().useGravity = true;
                /*GetComponent<Rigidbody>().useGravity = true;
                //GetComponent<Rigidbody>().velocity = Vector3.zero;
                onGround = true;
                rotation = false;
                rb.isKinematic = false;
                GetComponent<Collider>().isTrigger = false;*/
            }
            Debug.Log("Obstacle");
            isFlying = false;

            if (!bouncing)
            {
                bouncing = true;

                /*Vector3 direccionEntrante = transform.position - other.transform.position;
                Vector3 direccionRebote = Vector3.Reflect(direccionEntrante, other.transform.forward);

                handPlace.LookAt(targetPosition);
                float dotProduct = Vector3.Dot(direccionEntrante.normalized, handPlace.transform.right);

                float angle = (dotProduct >= 0f) ? 45f : -45f;
                Debug.Log(angle);
                Quaternion rotacionRebote = Quaternion.Euler(0, angle, 0); // Ángulo de rebote de 45 grados

                Vector3 nuevaDireccion = rotacionRebote * direccionRebote;
                nuevaDireccion.y = transform.position.y;

                targetPosition = transform.position + nuevaDireccion.normalized * distanciaConcreta;
                targetPosition.y = transform.position.y;*/
                targetPosition = localTargetPosition;

                Debug.Log("target = " + targetPosition);
            }
        }
    }
    void MakeDamageToEnemyAndPush(Collider other, float damage)
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
            damageableObject.ReceiveDamage(damageObj);

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
            Vector3 normal = transform.position + other.gameObject.transform.position;
            normal = Vector3.Normalize(normal);
            normal.y = 0;
            temporalRb.AddForce(normal, ForceMode.Impulse);
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

}
