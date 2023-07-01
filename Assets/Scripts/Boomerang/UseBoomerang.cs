using UnityEngine;
using UnityEngine.UI;

//LANZAMIENTO BOOMERANG FUNCIONAL CON AUMENTO DE FUERZA Y LINERENDERER ADAPTATIVO
public class UseBoomerang : MonoBehaviour
{
    // Variables p�blicas
    public float followSpeed;
    public float maxDistance;
    public float minDistance;
    public float minDistanceToLaunch;
    public float damage;


    // Variables privadas
    private Vector3 initialPosition;
    public Vector3 targetPosition;
    public Vector3 localTargetPosition;
    public Vector3 localTargetPosition2;
    private Vector3 returnPosition;
    private LineRenderer lr;

    public Transform handPlace;
    //[HideInInspector]
    //public bool rotation = false;
    public float distanceToDisplacementOnBouncing = 5f;

    public Camera mainCamera;

    //UI damage dealt

    PlayerStats playerStats;
    [HideInInspector]
    public GameObject chargeBar;
    public float distanceToEnd;

    //interpolacion velocidad boomerang
    //public float slowdownFactor = 0.5f; // Factor de desaceleraci�n en el medio
    //private float currentLerpTime = 0.0f; // Tiempo de interpolaci�n actual

    //calculate endPoint
    private bool isButtonPressed = false;
    private Vector3 startPoint;
    private Vector3 startPointOriginal;
    private Vector3 endPoint;
    private float pressStartTime;

    public float distancePerSecond = 5; // Distancia a recorrer por segundo
    bool clickPressed = false;
    int counterClicks = 0;

    public float expectedColdownTime = 0f;

    public Text timing;
    [HideInInspector]
    public float startedTimeThrow = 0f;
    public Text timePressedText;
    float startedTimePress = 0f;
    private float timePressed = 0f;

    [HideInInspector]
    public BoomerangController boomerangController;
    Animator animator;
    private Vector3 lastEndPoint;
    private bool variant = false;
    Vector3 deflection = Vector3.zero;
    float timeAcumulated = 0f;
    private void Awake()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        //boomerangController = GameObject.Find("Boomer").GetComponent<BoomerangController>();
        boomerangController = FindObjectOfType<BoomerangController>();
        chargeBar = FindObjectOfType<ChargeBar>().gameObject;
        animator = GetComponentInChildren<Animator>();
    }
    // Inicializaci�n
    void Start()
    {
        lr = boomerangController.gameObject.GetComponent<LineRenderer>();
        lr.enabled = false;
        initialPosition = transform.position;
        returnPosition = initialPosition;
        lr.positionCount = 2;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.useWorldSpace = true;

        mainCamera = FindObjectOfType<CameraController>().GetComponent<Camera>();
        timing = GameObject.Find("TimeToComeBack").GetComponent<Text>();
        timePressedText = GameObject.Find("TimePressed").GetComponent<Text>();
    }
    // Actualizaci�n por fotograma
    void Update()
    {
        //Debug.Log(playerStats.selectedMode);
        //Debug.Log(PlayerStats.GameMode.Boomerang);
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            if (boomerangController.onColdown)
            {
                timing.text = "go and comeback = " + (Time.time - startedTimeThrow).ToString();
            }
            /*
            Debug.Log("onHand = " + boomerangController.onHand);
            Debug.Log("isFlying = " + boomerangController.isFlying);
            Debug.Log("specialThrow = " + boomerangController.specialThrow);
            //SE CALCULA MAL EL ISRETURNING Y ESTA A TRUE, POR ESO NO LANZA
            Debug.Log("isReturning = " + boomerangController.isReturning);
            */
            //MOVER TODO EL CODIGO DE UPDATE/LATE UPDATE Y DEMAS AQUI
            if (Input.GetMouseButtonDown(0) && boomerangController.onHand && !boomerangController.isFlying && !boomerangController.specialThrow && 
                !boomerangController.isReturning)
            {
                Debug.Log("BOOM = CLICK IZQUIERDO PULSADO");
                animator.SetTrigger("Charge");
                //activamos la barra de progreso
                chargeBar.SetActive(true);
                chargeBar.GetComponent<ChargeBar>().target = boomerangController.gameObject;
                chargeBar.GetComponent<ChargeBar>().ResetFilled(0f);
                chargeBar.GetComponent<ChargeBar>().ChangeObjetive(maxDistance);


                isButtonPressed = true;
                startedTimePress = Time.time;
                //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                startPoint = boomerangController.gameObject.transform.position;
                startPointOriginal = startPoint;
                Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                startPoint = boomerangController.gameObject.transform.position + direction * minDistanceToLaunch;
                pressStartTime = Time.time;
            }

            if (isButtonPressed)
            {
                Debug.Log("BOOM = BUTTON PRESSED");
                endPoint = CalculateEndPoint()/* + deflection.normalized*/;
                timePressed = Time.time - startedTimePress;
                if (timePressed <= 1.5f)
                {
                    timePressedText.text = "TimePressed = " + timePressed.ToString();
                }

            }
            //atraer boomerang con poder mental
            if (Input.GetMouseButton(0) && !boomerangController.isFlying && !boomerangController.onHand && !boomerangController.attracting && 
                playerStats.currentMana >= 5 && !boomerangController.isReturning && !boomerangController.specialThrow && !boomerangController.bouncing)
            {
                Debug.Log("BOOM = ATRAER BOOMERANG CON PODER MENTAL");
                //revisar bien esto
                startPoint = boomerangController.gameObject.transform.position;
                startPointOriginal = startPoint;
                //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                //Vector3 direction = GetMouseWorldPosition() - startPoint;
                Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                startPoint = boomerangController.gameObject.transform.position + direction/* * minDistanceToLaunch*/;
                boomerangController.isReturning = true;
                boomerangController.attracting = true;
                //asi vuelve en linea recta a la posicion inicial de lanzamiento
                boomerangController.returnPosition = handPlace.transform.position;
            }

            // Lanzamiento
            if (Input.GetMouseButtonUp(0) && !boomerangController.isFlying && boomerangController.onHand && !boomerangController.specialThrow && 
                !boomerangController.isReturning && isButtonPressed)
            {
                animator.SetTrigger("Attack");
                Debug.Log("BOOM = CLICK IZQUIERDO SOLTADO");
                //update del damage del boomerangController, por si pillamos un Powerup
                boomerangController.damageBoomerang = damage;
                Debug.Log("boomerang lanzado");
                distanceToEnd = 0f;
                chargeBar.SetActive(false);

                clickPressed = false;
                //reiniciamos el counter para empezar a generar el lineRenderer si el jugaodr tiene ya pulsado el click izquierdo
                counterClicks = 0;

                lr.positionCount = 0;
                boomerangController.onHand = false;
                //mantenemos su rotacion original
                boomerangController.gameObject.transform.rotation = Quaternion.identity;
                boomerangController.gameObject.transform.SetParent(null);
                initialPosition = boomerangController.gameObject.transform.position;
                boomerangController.rotation = true;
                Launch();
                lr.enabled = false;
                boomerangController.onColdown = true;
                isButtonPressed = false;

                //calculamos el coldown estimado (FORMA CORRECTA, PERO EL T NO ES LINEAL)
                Debug.Log("distancia entre inicio y fin = " + Vector3.Distance(transform.position, endPoint));
                /*float distance = Vector3.Distance(transform.position, endPoint);
                expectedColdownTime = (distance / followSpeed * Time.deltaTime);*/
                if (timePressed > 0 && timePressed < 0.8f)
                {
                    expectedColdownTime = 0.7f;
                }
                else if (timePressed > 0.9 && timePressed < 1.5f)
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
        }

    }
    
    private void LateUpdate()
    {
        //A�ADIDO AL LATEUPDATE PARA UNA MEJOR ACTUALIZACION (AUN POR MEJORAR)
        //Carga
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            if (Input.GetMouseButton(0) && !boomerangController.isFlying && boomerangController.onHand && !boomerangController.specialThrow 
                && !boomerangController.isReturning && isButtonPressed)
            {
                Debug.Log("BOOM = CLICK IZQUIERDO MANTENER PULSADO LR");
                localTargetPosition = Vector3.zero;
                localTargetPosition2 = Vector3.zero;
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
                    startPoint = boomerangController.gameObject.transform.position;
                    startPointOriginal = startPoint;
                    //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                    Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                    startPoint = boomerangController.gameObject.transform.position + direction * minDistanceToLaunch;
                    pressStartTime = Time.time;
                    endPoint = boomerangController.gameObject.transform.position;
                    clickPressed = false;
                    counterClicks++;
                }

                lr.positionCount = 2;
                lr.enabled = true;
                //Vector3 mousePosition = GetMouseWorldPosition();
                Vector3 targetPositionLine = boomerangController.gameObject.transform.position;
                //actualizamos el startPoint para que la linea se dibuje correctamente (por culpa de que empieza a una distancia minima, el
                //calculo se hace mal y hay que recalcular el startPoint)
                if (Vector3.Distance(lr.GetPosition(1), GetMouseWorldPosition()) > 1)
                {
                    Debug.Log("superada la distancia");
                    lr.positionCount = 0;
                    lr.positionCount = 2;
                    Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPointOriginal);
                    startPoint = boomerangController.gameObject.transform.position + direction * minDistanceToLaunch;
                    //endPoint = CalculateEndPoint();
                }
                lr.SetPosition(0, targetPositionLine);
                lr.SetPosition(1, new Vector3(endPoint.x, 1f, endPoint.z));
                Debug.Log("DISTANCIA ENTRE ENDPOINT LR Y ENDPOINT REAL = " + Vector3.Distance(lr.GetPosition(1), GetMouseWorldPosition()));
                Debug.Log("posicion 0 lr = " + lr.GetPosition(0));
                Debug.Log("posicion 1 lr = " + lr.GetPosition(1));

                //si colisiona con un muro el raycast, a�adimos un tercer punto
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                //PRIMER REBOTE
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue);
                    if (hit.collider.gameObject.CompareTag("Obstacle"))
                    {
                        
                        localTargetPosition2 = Vector3.zero;
                        //raycast innecesario
                        /*Ray rayLr = new Ray(lr.GetPosition(0), lr.GetPosition(1) - lr.GetPosition(0));
                        float maxDistance = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(1));
                        if (Physics.Raycast(rayLr, out RaycastHit hit2, maxDistance))
                        {*/
                        lr.positionCount = 3;

                        Vector3 point = hit.point;
                        point.y = 1f;

                        boomerangController.gameObject.transform.LookAt(point);
                        //Debug.DrawRay(transform.forward, Input.mousePosition, Color.cyan);
              

                        Vector3 direccionEntrante = hit.collider.gameObject.transform.position - boomerangController.gameObject.transform.position;
                        Vector3 bounceDirection = Vector3.Reflect(direccionEntrante.normalized, hit.normal);
                        /*Vector3 direccionNormalizada = direccionEntrante.normalized;
                        //Vector3 direccionRebote = Vector3.Reflect(direccionEntrante, hit.collider.gameObject.transform.forward.normalized);
                        //Vector3 direccionRebote = Vector3.Reflect(boomerangController.gameObject.transform.forward,
                            //Vector3.Normalize(boomerangController.gameObject.transform.forward));

                        float dotProduct = Vector3.Dot(direccionNormalizada, hit.normal);
                        Vector3 direccionRebote = direccionNormalizada - 2f * dotProduct * hit.normal;
                        //Vector3 positionA = new Vector3(boomerangController.gameObject.transform.position.x, 0f, 
                            //boomerangController.gameObject.transform.position.z);
                        //Vector3 positionB = new Vector3(hit.collider.gameObject.transform.position.x, 0f, hit.collider.gameObject.transform.position.z);
                        //Vector3 direction = positionB - positionA;

                        float angle = (dotProduct >= 0f) ? 45f : -45f;
                        float angleRadians = angle * Mathf.Deg2Rad;
                        //HACER QUE EL ANGULO SEA DINAMICO ENTRE 2 VALORES PARA QUE SEA MAS PRECISO
                        //A�ADIR UN AUMENO DE VELOCIDAD AL PRINCIPIO Y FINAL Y REDUCCIR LA VELOCIDAD EN EL MEDIO

                        Quaternion rotacionRebote = Quaternion.AngleAxis(angleRadians, Vector3.up); // �ngulo de rebote de 45 grados

                        Vector3 nuevaDireccion = rotacionRebote * direccionRebote;
                        nuevaDireccion.y = 1f;//boomerangController.gameObject.transform.position.y/;
                        */
                        targetPosition = point + bounceDirection * distanceToDisplacementOnBouncing;
                        targetPosition.y = 1f;//boomerangController.gameObject.transform.position.y;
                        localTargetPosition = targetPosition;
                        lr.SetPosition(1, point);
                        lr.SetPosition(2, localTargetPosition);
                        //Debug.DrawRay(hit.collider.gameObject.transform.position, /*Vector3.Reflect(localTargetPosition, point)*/localTargetPosition, Color.blue);
                        //}
                        Ray ray2 = new Ray(point, bounceDirection);
                        //SEGUNDO REBOTE
                        if (Physics.Raycast(ray2, out RaycastHit hit2, distanceToDisplacementOnBouncing))
                        {
                            Vector3 point2 = hit2.point;
                            point2.y = 1f;
                            Debug.DrawRay(ray2.origin, ray2.direction * distanceToDisplacementOnBouncing, Color.green);
                            Debug.Log("RAYCAST 2");

                            if (hit2.collider.gameObject.CompareTag("Obstacle"))
                            {
                                //actualizamos el punto de colision anterior
                                lr.SetPosition(2, point2);
                                localTargetPosition = point2;
                                //lr.SetPosition(2, hit2.collider.gameObject.transform.position);
                                Debug.Log(hit2.collider.gameObject);
                                Debug.Log("CONSILION CON UN SEGUNDO OBSTACULO");
                                lr.positionCount = 4;
                                Vector3 direccionEntrante2 = hit2.collider.gameObject.transform.position - hit.collider.gameObject.transform.position;
                                Vector3 bounceDirection2 = Vector3.Reflect(direccionEntrante2.normalized, hit2.normal);
                                //Vector3 direccionEntrante2 = hit2.collider.gameObject.transform.position - hit.collider.gameObject.transform.position;
                                /*Vector3 direccionNormalizada2 = direccionEntrante2.normalized;
                                //Vector3 direccionRebote = Vector3.Reflect(direccionEntrante, hit.collider.gameObject.transform.forward.normalized);
                                //Vector3 direccionRebote2 = Vector3.Reflect(direccionEntrante2, hit2.normal);


                                float dotProduct2 = Vector3.Dot(direccionNormalizada2, hit2.normal);
                                Vector3 direccionRebote2 = direccionNormalizada2 - 2f * dotProduct2 * hit2.normal;

                                float angle2 = (dotProduct2 >= 0f) ? 45f : -45f;
                                float angleRadians2 = angle2 * Mathf.Deg2Rad;
                                //HACER QUE EL ANGULO SEA DINAMICO ENTRE 2 VALORES PARA QUE SEA MAS PRECISO
                                //A�ADIR UN AUMENO DE VELOCIDAD AL PRINCIPIO Y FINAL Y REDUCCIR LA VELOCIDAD EN EL MEDIO

                                Quaternion rotacionRebote2 = Quaternion.AngleAxis(angleRadians2, Vector3.up); // �ngulo de rebote de 45 grados

                                Vector3 nuevaDireccion2 = rotacionRebote2 * direccionRebote2;
                                nuevaDireccion2.y = 1f;//boomerangController.gameObject.transform.position.y/;
                                */
                                //Vector3 targetPosition2 = point2 + bounceDirection2 * distanceToDisplacementOnBouncing;
                                targetPosition = point2 + bounceDirection2 * distanceToDisplacementOnBouncing;
                                //targetPosition2.y = 1f/* boomerangController.gameObject.transform.position.y*/;
                                targetPosition.y = 1f/* boomerangController.gameObject.transform.position.y*/;
                                //localTargetPosition2 = targetPosition2;
                                localTargetPosition2 = targetPosition;
                                lr.SetPosition(3, localTargetPosition2);
                                //Debug.DrawRay(hit.point, /*Vector3.Reflect(localTargetPosition, point)*/nuevaDireccion2, Color.green);
                                //Debug.DrawRay(localTargetPosition, nuevaDireccion2, Color.grey);
                            }
                        }
                        else
                        {
                            Debug.DrawRay(ray2.origin, ray2.direction * distanceToDisplacementOnBouncing, Color.red);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.direction * distanceToDisplacementOnBouncing, Color.yellow);
                    }
                }

            }
        }
    }

    // Actualizar la posici�n del Boomerang mientras est� en vuelo
    void FixedUpdate()
    {
    }
    // Lanzar el Boomerang
    void Launch()
    {
        targetPosition = endPoint;
        //targetPosition = CalculateEndPoint();
        boomerangController.isFlying = true;
        timeAcumulated = 0f;
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
            Plane plane = new Plane(Vector3.up, boomerangController.gameObject.transform.position);
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
    //calcular punto final de trayectoria
    private Vector3 CalculateEndPoint()
    {
        //Vector3 endPointDirection = Vector3.Normalize(GetMouseWorldPosition() - endPoint);
        /*Vector3 direction;
        if (variant)
        {
            direction = Vector3.Normalize(GetMouseWorldPosition() - startPointOriginal);
        }
        else
        {
            direction = Vector3.Normalize(endPointDirection - startPointOriginal);
        }*/
        Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPointOriginal);
        if (Vector3.Distance(startPointOriginal, endPoint) >= maxDistance)
        {
            direction = direction * (maxDistance + 0.5f);
            Vector3 endPoint2 = startPointOriginal + direction;
            //Debug.Log("endPointOnFunction = " + endPoint2);
            return endPoint2;
        }
        else
        {
            //AUMENTAR DISTANCIA POR SEGUNDO O A�ADIRLE UN NUMERO FIJO QUE SEA EL MINIMO SI EL TIEMPO ES MENOR A 1.5 SEGUNDOS
            Vector3 displacement = direction * distancePerSecond;
            float pressDuration = Time.time - pressStartTime;
            Vector3 totalDisplacement = displacement * pressDuration;
            //timeAcumulated += distancePerSecond;
            Vector3 finalPoint = startPoint + totalDisplacement;
            return finalPoint;
        }
    }
    //funcion para arreglar el lineRenderer
    /*private void RepaintLinerendereOnMaxDistanceSurpass(Vector3 currentEndPoint)
    {
        deflection = GetMouseWorldPosition();
        Vector3 newDirection = Vector3.Normalize(deflection - boomerangController.transform.position);
        Vector3 newDisplacement = newDirection * timeAcumulated;
        lr.positionCount = 0;
        lr.positionCount = 2;
        lr.SetPosition(0, boomerangController.transform.position);
        lr.SetPosition(1, startPointOriginal + newDisplacement);
        lastEndPoint = currentEndPoint;
        //deflection = Vector3.zero;
    }*/
}
