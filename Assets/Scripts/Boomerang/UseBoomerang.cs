using UnityEngine;
using UnityEngine.UI;

//LANZAMIENTO BOOMERANG FUNCIONAL CON AUMENTO DE FUERZA Y LINERENDERER ADAPTATIVO
public class UseBoomerang : MonoBehaviour
{
    // Variables públicas
    public float followSpeed;
    public float maxDistance;
    public float minDistance;
    public float minDistanceToLaunch;
    public float damage;


    // Variables privadas
    private Vector3 initialPosition;
    public Vector3 targetPosition;
    public Vector3 localTargetPosition;
    private Vector3 returnPosition;
    private LineRenderer lr;

    public Transform handPlace;
    //[HideInInspector]
    //public bool rotation = false;

    float distanceToMove = 5f;

    public Camera mainCamera;

    //UI damage dealt

    PlayerStats playerStats;
    [HideInInspector]
    public GameObject chargeBar;
    public float distanceToEnd;

    //interpolacion velocidad boomerang
    //public float slowdownFactor = 0.5f; // Factor de desaceleración en el medio
    //private float currentLerpTime = 0.0f; // Tiempo de interpolación actual

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
    float startedTimeThrow = 0f;
    public Text timePressedText;
    float startedTimePress = 0f;
    private float timePressed = 0f;

    [HideInInspector]
    public BoomerangController boomerangController;

    private void Awake()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        //boomerangController = GameObject.Find("Boomer").GetComponent<BoomerangController>();
        boomerangController = FindObjectOfType<BoomerangController>();
    }
    // Inicialización
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
    }
    // Actualización por fotograma
    void Update()
    {
        Debug.Log(playerStats.selectedMode);
        Debug.Log(PlayerStats.GameMode.Boomerang);
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            if (boomerangController.onColdown)
            {
                timing.text = "go and comeback = " + (Time.time - startedTimeThrow).ToString();
            }
            Debug.Log("onHand = " + boomerangController.onHand);
            Debug.Log("isFlying = " + boomerangController.isFlying);
            Debug.Log("specialThrow = " + boomerangController.specialThrow);
            //SE CALCULA MAL EL ISRETURNING Y ESTA A TRUE, POR ESO NO LANZA
            Debug.Log("isReturning = " + boomerangController.isReturning);
            //MOVER TODO EL CODIGO DE UPDATE/LATE UPDATE Y DEMAS AQUI
            if (Input.GetMouseButtonDown(0) && boomerangController.onHand && !boomerangController.isFlying && !boomerangController.specialThrow && 
                !boomerangController.isReturning)
            {
                Debug.Log("BOOM = CLICK IZQUIERDO PULSADO");
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
                endPoint = CalculateEndPoint();
                timePressed = Time.time - startedTimePress;
                if (timePressed <= 1.5f)
                {
                    timePressedText.text = "TimePressed = " + timePressed.ToString();
                }

            }
            //atraer boomerang con poder mental
            if (Input.GetMouseButton(0) && !boomerangController.isFlying && !boomerangController.onHand && !boomerangController.attracting && 
                playerStats.currentMana >= 5 && !boomerangController.isReturning && !boomerangController.specialThrow)
            {
                Debug.Log("BOOM = ATRAER BOOMERANG CON PODER MENTAL");
                //revisar bien esto
                startPoint = boomerangController.gameObject.transform.position;
                startPointOriginal = startPoint;
                //modificamos el startpoint para que empiece a calcularse un poco mas delante y tenga una "fuerza minima"
                Vector3 direction = Vector3.Normalize(GetMouseWorldPosition() - startPoint);
                startPoint = boomerangController.gameObject.transform.position + direction * minDistanceToLaunch;
                boomerangController.isReturning = true;
                boomerangController.attracting = true;
                //asi vuelve en linea recta a la posicion inicial de lanzamiento
                returnPosition = handPlace.transform.position;
            }

            // Lanzamiento
            if (Input.GetMouseButtonUp(0) && !boomerangController.isFlying && boomerangController.onHand && !boomerangController.specialThrow && 
                !boomerangController.isReturning)
            {
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
        //AÑADIDO AL LATEUPDATE PARA UNA MEJOR ACTUALIZACION (AUN POR MEJORAR)
        //Carga
        if (playerStats.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            if (Input.GetMouseButton(0) && !boomerangController.isFlying && boomerangController.onHand && !boomerangController.specialThrow 
                && !boomerangController.isReturning)
            {
                Debug.Log("BOOM = CLICK IZQUIERDO MANTENER PULSADO LR");
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
                Vector3 mousePosition = GetMouseWorldPosition();
                Vector3 targetPositionLine = boomerangController.gameObject.transform.position;

                lr.SetPosition(0, targetPositionLine);
                lr.SetPosition(1, new Vector3(endPoint.x, 1f, endPoint.z));

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
                            boomerangController.gameObject.transform.LookAt(point);

                            Vector3 direccionEntrante = hit.collider.gameObject.transform.position - boomerangController.gameObject.transform.position;
                            //Vector3 direccionRebote = Vector3.Reflect(direccionEntrante, hit.collider.gameObject.transform.forward.normalized);
                            Vector3 direccionRebote = Vector3.Reflect(boomerangController.gameObject.transform.forward, 
                                hit.collider.gameObject.transform.position);

                            float dotProduct = Vector3.Dot(direccionEntrante.normalized, boomerangController.gameObject.transform.right);

                            Vector3 positionA = new Vector3(boomerangController.gameObject.transform.position.x, 0f, 
                                boomerangController.gameObject.transform.position.z);
                            Vector3 positionB = new Vector3(hit.collider.gameObject.transform.position.x, 0f, hit.collider.gameObject.transform.position.z);
                            Vector3 direction = positionB - positionA;

                            float angle = (dotProduct >= 0f) ? 45f : -45f;

                            //HACER QUE EL ANGULO SEA DINAMICO ENTRE 2 VALORES PARA QUE SEA MAS PRECISO
                            //AÑADIR UN AUMENO DE VELOCIDAD AL PRINCIPIO Y FINAL Y REDUCCIR LA VELOCIDAD EN EL MEDIO

                            Quaternion rotacionRebote = Quaternion.Euler(0, angle, 0); // Ángulo de rebote de 45 grados

                            Vector3 nuevaDireccion = rotacionRebote * direccionRebote;
                            nuevaDireccion.y = boomerangController.gameObject.transform.position.y;

                            targetPosition = boomerangController.gameObject.transform.position + nuevaDireccion.normalized * distanceToMove;
                            targetPosition.y = boomerangController.gameObject.transform.position.y;
                            localTargetPosition = targetPosition;
                            lr.SetPosition(1, point);
                            lr.SetPosition(2, targetPosition);
                        }
                    }
                }
            }
        }
    }

    // Actualizar la posición del Boomerang mientras está en vuelo
    void FixedUpdate()
    {
    }
    // Lanzar el Boomerang
    void Launch()
    {
        targetPosition = CalculateEndPoint();
        boomerangController.isFlying = true;
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
            //AUMENTAR DISTANCIA POR SEGUNDO O AÑADIRLE UN NUMERO FIJO QUE SEA EL MINIMO SI EL TIEMPO ES MENOR A 1.5 SEGUNDOS
            Vector3 displacement = direction * distancePerSecond;
            float pressDuration = Time.time - pressStartTime;
            Vector3 totalDisplacement = displacement * pressDuration;

            Vector3 finalPoint = startPoint + totalDisplacement;
            return finalPoint;
        }
    }
}
