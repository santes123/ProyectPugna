using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangControllerTest : MonoBehaviour
{
    // Variables públicas
    public float followSpeed;
    public float maxDistance;
    public float minDistance;
    public float damage;
    private float lastHitTime;
    public float coldownHit;

    // Variables privadas
    private Vector3 initialPosition;
    private Vector3 targetPosition;
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
    }

    // Actualización por fotograma
    void Update()
    {
        if (Input.GetMouseButton(0) && !isFlying && onHand)
        {
            lr.positionCount = 2;
            lr.enabled = true;
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 targetPosition = transform.position;


            lr.SetPosition(0, mousePosition);
            lr.SetPosition(1, targetPosition);
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
        }
        // Seguimiento de la línea
        if (isFlying && !isReturning)
        {
            if (targetPosition != Vector3.zero)
            {
                //targetPosition = targetPosition + Vector3.up;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
            }
        }

        // Comprobar la distancia del Boomerang
        if (Vector3.Distance(transform.position, initialPosition) > maxDistance && !isReturning && !specialThrow || 
            Vector3.Distance(transform.position, targetPosition) < minDistance && !specialThrow)
        {
            Debug.Log("returning");
            Return();
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
            return hit.point + new Vector3(0f, 0.1f, 0f); // adjust height of point to be above terrain
        }
        else
        {
            Plane plane = new Plane(transform.up, transform.position);
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
    // Actualizar la posición del Boomerang mientras está en vuelo
    void FixedUpdate()
    {
        //APAÑO PORQUE NO DETECTA BIEN LA COLISION ENTRE EL CHARACTER CONTROLLER Y EL BOX COLLIDER
        if (Vector3.Distance(transform.position, handPlace.position) <= minDistance && !isFlying && !specialThrow || 
            Vector3.Distance(transform.position, handPlace.position) <= minDistance && isFlying && isReturning && !specialThrow)
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
            transform.position = Vector3.Lerp(transform.position, returnPosition,followSpeed * Time.fixedDeltaTime);
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
            rotation = false;
            rb.isKinematic = true;
            transform.position = handPlace.position;
            //transform.rotation = Quaternion.identity;
            //targetPosition = Vector3.zero;
        }
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
        if (other.CompareTag("Enemy") && !onHand && isFlying)
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
                    //vuelve el boomerang
                    Return();

                }
                //Debug.Log("enemy");
                
            }

        }
        if (other.CompareTag("Obstacle") && !isReturning && isFlying || other.CompareTag("Obstacle") && specialThrow)
        {
            if (specialThrow)
            {
                specialThrow = false;
                lr.positionCount = 0;
                GetComponent<FollowLine>().waypoints.Clear();
            }
            Debug.Log("Obstacle");
            isFlying = false;
            //rb.velocity = Vector3.zero;
            rb.useGravity = true;
            rotation = false;
            GetComponent<Collider>().isTrigger = false;

            //REBOTE EN V
            //USAR UN BOOLEANDO "REBOANDO" Y HACER UN LERP COMO CON EL LANZAMIENTO Y HACER QUE "REBOTE" LA LINEA DE PREDICCION Y SE HAGA UN "PREDICT"
            rb.isKinematic = false;
            // Calcular la dirección del rebote
            Vector3 direccionRebote = Vector3.Reflect(transform.forward, (other.transform.position - transform.position).normalized);

            // Calcular la dirección en forma de "V" (hacia arriba)
            Vector3 direccionFinal = Quaternion.AngleAxis(45f, Vector3.up) * direccionRebote;

            // Aplicar la fuerza al objeto A en la dirección del rebote
            rb.AddForce(direccionFinal * 5f, ForceMode.Impulse);
        }
    }
    void MakeDamageToEnemyAndPush(Collider other, float damage)
    {
        Debug.Log("dhsajdas");
        //HACEMOS DAÑO AL ENEMIGO MEDIANTE LA INTERFAZ
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            print("collision con el enemigo");
            Damage damageObj = new Damage();
            damageObj.amount = (int) damage;
            damageObj.source = UnitType.Player;
            damageObj.targetType = TargetType.Single;
            damageableObject.ReceiveDamage(damageObj);
            Renderer hitRenderer = other.GetComponentInChildren<Renderer>();
            // Cambiar el color del material del renderer
            if (hitRenderer != null)
            {
                hitRenderer.material.color = Color.blue;
            }
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
    public float BoomerangHeight()
    {
        return handPlace.position.y;
    }
}
