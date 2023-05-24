using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//LANZAMIENTO BOOMERANG FUNCIONAL CON AUMENTO DE FUERZA Y LINERENDERER ADAPTATIVO
public class BoomerangController : MonoBehaviour
{
    // Variables públicas
    public float followSpeed;
    public float maxDistance;
    public float minDistance;
    public float minDistanceToLaunch;
    public float damage;
    private float lastHitTime;
    public float coldownHit;
    public float manaCostOfAttractingBoomerang;

    // Variables privadas
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 localTargetPosition;
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
    public Transform pointer;
    [HideInInspector]
    public bool rotation = false;

    [HideInInspector]
    public bool specialThrow = false;

    [HideInInspector]
    public bool bouncing = false;
    //float lerpSpeed = 2f;
    float distanceToMove = 5f;
    [HideInInspector]
    public bool onGround = false;
    public Camera mainCamera;
    //UI damage dealt
    public GameObject floatingDamageTextPrefab;
    [HideInInspector]
    bool attracting;
    PlayerStats playerStats;
    [HideInInspector]
    public bool onColdown = false;
    public GameObject chargeBar;
    public float distanceToEnd;

    //mouse movement and collision

    //interpolacion velocidad boomerang
    public float slowdownFactor = 0.5f; // Factor de desaceleración en el medio
    //private float currentLerpTime = 0.0f; // Tiempo de interpolación actual

    //calculate endPoint
    private bool isButtonPressed = false;
    private Vector3 startPoint;
    private Vector3 startPointOriginal;
    private Vector3 endPoint;
    private float pressStartTime;
    //public float launchSpeed = 5f;

    Vector3 direction;  // Dirección en la que te alejarás
    public float distancePerSecond = 5; // Distancia a recorrer por segundo
    //float pressDuration;
    bool clickPressed = false;
    int counterClicks = 0;
    public float impulseForceWhenHit;

    public float expectedColdownTime = 0f;

    public Text timing;
    float startedTimeThrow = 0f;
    public Text timePressedText;
    float startedTimePress= 0f;
    private float timePressed = 0f;

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

        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            if (onColdown)
            {
                timing.text = "go and comeback = " + (Time.time - startedTimeThrow).ToString();
            }
            //MOVER TODO EL CODIGO DE UPDATE/LATE UPDATE Y DEMAS AQUI
            if (Input.GetMouseButtonDown(0) && onHand && !isFlying && !specialThrow && !isReturning)
            {
                //activamos la barra de progreso
                chargeBar.SetActive(true);
                chargeBar.GetComponent<ChargeBar>().target = gameObject;
                chargeBar.GetComponent<ChargeBar>().ResetFilled(0f);
                chargeBar.GetComponent<ChargeBar>().ChangeObjetive(maxDistance);


                isButtonPressed = true;
                startedTimePress = Time.time;
                //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                startPoint = transform.position;
                startPointOriginal = startPoint;
                Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                startPoint = transform.position + direction * minDistanceToLaunch;
                pressStartTime = Time.time;
            }

            if (isButtonPressed)
            {
                endPoint = CalculateEndPoint();
                timePressed = Time.time -  startedTimePress;
                if (timePressed <= 1.5f)
                {
                    timePressedText.text = "TimePressed = " + timePressed.ToString();
                }

            }
            //atraer boomerang con poder mental
            if (Input.GetMouseButton(0) && !isFlying && !onHand && !attracting && playerStats.currentMana >= 5 && !isReturning && !specialThrow)
            {
                //revisar bien esto
                startPoint = transform.position;
                startPointOriginal = startPoint;
                //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                startPoint = transform.position + direction * minDistanceToLaunch;
                isReturning = true;
                attracting = true;
                //asi vuelve en linea recta a la posicion inicial de lanzamiento
                returnPosition = handPlace.transform.position;
            }

            // Lanzamiento
            if (Input.GetMouseButtonUp(0) && !isFlying && onHand && !specialThrow && !isReturning)
            {
                distanceToEnd = 0f;
                chargeBar.SetActive(false);
                //
                clickPressed = false;
                //reiniciamos el counter para empezar a generar el lineRenderer si el jugaodr tiene ya pulsado el click izquierdo
                counterClicks = 0;

                lr.positionCount = 0;
                onHand = false;
                transform.SetParent(null);
                initialPosition = transform.position;
                rotation = true;
                Launch();
                lr.enabled = false;
                onColdown = true;
                isButtonPressed = false;

                //calculamos el coldown estimado (FORMA CORRECTA, PERO EL T NO ES LINEAL)
                Debug.Log("distancia entre inicio y fin = " + Vector3.Distance(transform.position, endPoint));
                /*float distance = Vector3.Distance(transform.position, endPoint);
                expectedColdownTime = (distance / followSpeed * Time.deltaTime);*/
                if (timePressed > 0 && timePressed < 0.8f)
                {
                    expectedColdownTime = 0.7f;
                }
                else if(timePressed > 0.9 && timePressed < 1.5f)
                {
                    expectedColdownTime = 1.1f;
                }
                else
                {
                    expectedColdownTime = 1.1f;
                }
                Debug.Log("expected coldown = " + expectedColdownTime);
                startedTimeThrow = Time.time;
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
                    // Calcular el nuevo desplazamiento en cada frame
                    Debug.Log("tiempo = " + followSpeed * Time.deltaTime);
                    Debug.Log("tiempo = " + followSpeed * Time.deltaTime);
                    float t = Mathf.Clamp01(followSpeed * Time.deltaTime);
                    Debug.Log("tiempo con clamp = " + t);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, t);
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
                expectedColdownTime = 0f;
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
                    //GameObject.Find("Player").GetComponent<BoxCollider>().enabled = true;
                    rotation = false;
                    rb.isKinematic = false;
                    GetComponent<Collider>().isTrigger = false;
                    //isFlying = false;
                }
            }
        }

    }
    private void LateUpdate()
    {
        //AÑADIDO AL LATEUPDATE PARA UNA MEJOR ACTUALIZACION (AUN POR MEJORAR)
        //Carga
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            if (Input.GetMouseButton(0) && !isFlying && onHand && !specialThrow && !isReturning)
            {
                //cambiamos los valores a los del boomerang
                //CALCULA EL ENDPOINT REALISTA, EN EL COMENTARIO ESTA OTRO QUE USA LA BARRA ENTERA
                distanceToEnd = Vector3.Distance(startPointOriginal, endPoint);  //esto es la distancia real, pero como empieza mas lejos no vale
                //distanceToEnd = Vector3.Distance(startPoint, endPoint);  //esto es la distancia ficticia, contando desde el startPoint (launchMin)

                //counter y booleano para el lineRenderer reset
                clickPressed = true;
                counterClicks++;
                //ARREGLO PARA EL RESET DEL LINERENDERER SI SE MANTIENE PULSADO EL CLICK IZQUIERDO ANTES DE QUE LLEGUE EL BOOMERANG
                if (clickPressed && counterClicks == 1)
                {
                    isButtonPressed = true;
                    startPoint = transform.position;
                    startPointOriginal = startPoint;
                    //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                    Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                    startPoint = transform.position + direction * minDistanceToLaunch;
                    pressStartTime = Time.time;
                    endPoint = transform.position;
                    clickPressed = false;
                    counterClicks++;
                }

                lr.positionCount = 2;
                lr.enabled = true;
                Vector3 mousePosition = GetMouseWorldPosition();
                //mousePosition.y = 1f;
                Vector3 targetPositionLine = transform.position;

                lr.SetPosition(0, targetPositionLine);

                /*if (Vector3.Distance(startPoint, mousePosition) <= maxDistance)
                {*/
                lr.SetPosition(1, new Vector3(endPoint.x, 1f, endPoint.z));
                //Debug.Log("endpoint = " + endPoint);
                /*}
                else
                {
                    lr.SetPosition(1, new Vector3(mousePosition.x, 1f, mousePosition.z));
                }*/


                //si colisiona con un muro el raycast, añadimos un tercer punto
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {

                    if (hit.collider.gameObject.CompareTag("Obstacle"))
                    {
                        Ray rayLr = new Ray(lr.GetPosition(0), lr.GetPosition(1) - lr.GetPosition(0));
                        float maxDistance = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(1));
                        if (Physics.Raycast(rayLr, out RaycastHit hit2, maxDistance))
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
            }
        }
    }

    // Actualizar la posición del Boomerang mientras está en vuelo
    void FixedUpdate()
    {
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            //APAÑO PORQUE NO DETECTA BIEN LA COLISION ENTRE EL CHARACTER CONTROLLER Y EL BOX COLLIDER
            if (Vector3.Distance(transform.position, handPlace.position) <= minDistance + 1 && !isFlying && !specialThrow ||
                Vector3.Distance(transform.position, handPlace.position) <= minDistance + 1 && isFlying && isReturning && !specialThrow)
            {
                //Debug.Log("Player1.1");
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
                float t = Mathf.Clamp01(followSpeed * Time.fixedDeltaTime);
                transform.position = Vector3.Lerp(transform.position, returnPosition, t);
                //transform.position = Vector3.MoveTowards(transform.position, returnPosition, followSpeed * Time.fixedDeltaTime);
                //VUELVE A LA MANO DEL JUGADOR
                //print(Vector3.Distance(transform.position, returnPosition));
                if (Vector3.Distance(transform.position, returnPosition) <= minDistance)
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
        //targetPosition = GetMouseWorldPosition();
        targetPosition = CalculateEndPoint();
        //Vector3 direction = (targetPosition - transform.position).normalized;
        //rb.AddForce(direction * launchForce);
        //rb.isKinematic = true;
        isFlying = true;
    }
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
        //collision con el player
        if (other.CompareTag("Player") && !isFlying || other.CompareTag("Player") && isFlying && isReturning)
        {

            Debug.Log("Player1.1");
            //Debug.Log(Time.deltaTime);
            rb.useGravity = false;
            GetComponent<Collider>().isTrigger = true;
            onHand = true;
            transform.SetParent(handPlace.transform);
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
    //calcular punto final de trayectoria
    private Vector3 CalculateEndPoint()
    {

        Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPointOriginal);
        if (Vector3.Distance(startPointOriginal, endPoint) >= maxDistance)
        {
            direction = direction * (maxDistance + 0.5f);
            Vector3 endPoint2 = startPointOriginal + direction;
            //Debug.Log("endPointOnFunction = " + endPoint2);
            return endPoint2;
        }/*else if (counterClicks == 1)
        {
            counterClicks++;
            return Vector3.zero;
        }*/
        else
        {
            //AUMENTAR DISTANCIA POR SEGUNDO O AÑADIRLE UN NUMERO FIJO QUE SEA EL MINIMO SI EL TIEMPO ES MENOR A 1.5 SEGUNDOS
            Vector3 displacement = direction * distancePerSecond;

            float pressDuration = Time.time - pressStartTime;
            //Debug.Log(pressDuration);

            //float distance = Mathf.Clamp(pressDuration * launchSpeed, 0f, maxDistance);
            Vector3 totalDisplacement = displacement * pressDuration;

            Vector3 finalPoint = startPoint + totalDisplacement;
            //Debug.Log("endPointOnFunction2 = " + finalPoint);
            return finalPoint;
        }


    }
}
