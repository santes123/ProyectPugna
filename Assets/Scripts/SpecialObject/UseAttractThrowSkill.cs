using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAttractThrowSkill : MonoBehaviour
{
    //variables locales
    [HideInInspector]
    public bool onColdown = false;
    [HideInInspector]
    public float remainingTime;
    public float coldownTime = 2f;
    [HideInInspector]
    public GameObject target = null;
    SpecialObject selectedObjectScript;
    PlayerStats player;

    //////////////////////////////////
    [HideInInspector]
    public bool estaSiendoAtraido = false;
    [HideInInspector]
    public bool haSidoLanzado = false;
    public float velocidadAtraccion;
    //float velocidadAtraccionOriginal;
    public float fuerzaBase;
    [HideInInspector]
    public float fuerzaLanzamiento;
    [HideInInspector]   
    public float fuerzaLanzamientoAnterior;
    [HideInInspector]
    public Vector3 direccionLanzamientoAnterior;
    public float fuerzaMaxima;
    private float tiempoPulsado = 0f;
    public float incrementoDeFuerzaPorSegundo;
    //float distanceToTakeOnHand = 2f;
    //public Transform player;
    public Transform handPlace;
    public Transform pointer;
    //Rigidbody rb;
    public bool onHand = false;
    public LayerMask collisionMask;

    public float velocidadRotacion;

    public float fuerzaRebote;
    public float fuerzaAleatoria;

    //public float damage;
    bool botonPresionado = false;
    [HideInInspector]
    public GameObject chargeBar;
    [HideInInspector]
    public float maxDistanceFromTargetToPlayer = 0f;
    public float currentDistanceFromTargetToPlayer = 0f;

    public float manaCost;
    public float incrementoDeCosteManaPorSegundo;
    public float maxManaCost;
    public float finalManaCost;

    //progresivamente pasar la parte de controles del SpecialObject aqui, tiene mas sentido
    void Start()
    {
        //velocidadAtraccionOriginal = velocidadAtraccion;
        fuerzaLanzamiento = fuerzaBase;
        player = GetComponent<PlayerStats>();
        //chargeBar = GameObject.FindGameObjectWithTag("ChargeBar");
        //Debug.Log("name = " + chargeBar.name);
        //chargeBar = GameObject.Find("ChargeBar");
        chargeBar = FindObjectOfType<ChargeBar>().gameObject;
    }

    void Update()
    {
        if (onColdown)
        {
            remainingTime -= Time.deltaTime;
            //Debug.Log("remaining time = " + remainingTime);
            if (remainingTime <= 0f)
            {
                onColdown = false;
                remainingTime = 0f;
                //reiniciamos el target para poder seleccionar otro objetivo
                target = null;
            }
        }
        if (player.selectedMode == PlayerStats.GameMode.AttractThrow)
        {

            Debug.Log("MODO ATTRACT AND THROW ACTIVADO...");
            if (Input.GetMouseButtonDown(0) && CheckIfRayHitObject() && !onColdown && player.currentMana >= manaCost)
            {
                Debug.Log("ATRAYENDO...");
                estaSiendoAtraido = true;
                //asi solo atraemos al objetivo que acabamos de marcar
                selectedObjectScript.estaSiendoAtraido = true;
                //activamos la barra de progreso
                chargeBar.SetActive(true);
                chargeBar.GetComponent<ChargeBar>().target = target;
                chargeBar.GetComponent<ChargeBar>().ResetFilled(0f);
                chargeBar.GetComponent<ChargeBar>().ChangeObjetive(maxDistanceFromTargetToPlayer);
            }
            if (Input.GetMouseButton(0) && estaSiendoAtraido)
            {
                velocidadAtraccion += 2;
                finalManaCost += incrementoDeCosteManaPorSegundo;
                currentDistanceFromTargetToPlayer = Vector3.Distance(target.transform.position, transform.position);
            }
            /*if (onHand)
            {
                target.transform.position = handPlace.position;
                //reiniciamos la velocidad de atraccion al valor base
                velocidadAtraccion = velocidadAtraccionOriginal;
                // Obtener las rotaciones actuales en los tres ejes
                Vector3 rotacionesActuales = transform.rotation.eulerAngles;

                // Agregar rotaciones adicionales en los ejes Y y Z
                float rotacionY = rotacionesActuales.y + velocidadRotacion * Time.deltaTime;
                float rotacionZ = rotacionesActuales.z + velocidadRotacion * Time.deltaTime;

                // Actualizar las rotaciones del objeto
                target.transform.rotation = Quaternion.Euler(rotacionesActuales.x, rotacionY, rotacionZ);
            }*/
            //Control del tiempo pulsado al lanzar, para luego calcular la fuerza
            if (Input.GetMouseButtonDown(1) && target.GetComponent<SpecialObject>().onHand)
            {
                botonPresionado = true;
                chargeBar.SetActive(true);
                chargeBar.GetComponent<ChargeBar>().ResetFilled(0f);
            }
            if (botonPresionado)
            {
                tiempoPulsado += Time.deltaTime;
                fuerzaLanzamiento = fuerzaLanzamiento + tiempoPulsado * incrementoDeFuerzaPorSegundo;
                if (fuerzaLanzamiento >= fuerzaMaxima)
                {
                    fuerzaLanzamiento = fuerzaMaxima;
                }
                //currentDistanceFromTargetToPlayer = Vector3.Distance(target.transform.position, transform.position);

            }
        }

    }
    private void FixedUpdate()
    {
        if (player.selectedMode == PlayerStats.GameMode.AttractThrow)
        {
            //LANZAMIENTO
            if (Input.GetMouseButtonUp(1)/* && estaSiendoAtraido */&& selectedObjectScript.estaSiendoAtraido == true && player.currentMana >= manaCost)
            {
                
                //target.GetComponent<Rigidbody>().Sleep();
                chargeBar.SetActive(false);
                onColdown = true;
                remainingTime = coldownTime;
                botonPresionado = false;
                print(tiempoPulsado);
                if (tiempoPulsado <= 0.5f)
                {
                    fuerzaLanzamiento = fuerzaBase;
                    finalManaCost = manaCost;
                }
                else
                {
                    //fuerzaLanzamiento = fuerzaLanzamiento + tiempoPulsado * incrementoDeFuerzaPorSegundo;
                    //incrementamos el coste de mana, segun el tiempo pulsado
                    finalManaCost = manaCost * tiempoPulsado * incrementoDeCosteManaPorSegundo;
                    Debug.Log("fuerza lanzamiento calculada = " + fuerzaLanzamiento);
                    /*if (fuerzaLanzamiento >= fuerzaMaxima)
                    {
                        fuerzaLanzamiento = fuerzaMaxima;
                    }*/
                    if (finalManaCost > maxManaCost)
                    {
                        finalManaCost = maxManaCost;
                    }
                }
                Debug.Log("fuerza pre lanzamiento = " + fuerzaLanzamiento);
                target.transform.LookAt(pointer);

                target.GetComponent<Collider>().enabled = true;

                //rb.useGravity = true;
                target.GetComponent<Rigidbody>().useGravity = true;
                //haSidoLanzado = true;
                selectedObjectScript.haSidoLanzado = true;

                estaSiendoAtraido = false;
                selectedObjectScript.estaSiendoAtraido = false;
                onHand = false;
                selectedObjectScript.onHand = false;
                //Vector3 direccionLanzamiento = (pointer.position - target.transform.position).normalized;
                //FIXEAMOS LA DIRECCION DE LANZAMIENTO USANDO GETMOUSEWORLDPOSITION() -> METODO CREADO EN BOOMERANGCONTROLLER
                Vector3 direccionLanzamiento = Vector3.Normalize(GetMouseWorldPosition() - transform.position);

                //target.GetComponent<Rigidbody>().WakeUp();
                //target.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode.Impulse);
                direccionLanzamientoAnterior = direccionLanzamiento;
                fuerzaLanzamientoAnterior = fuerzaLanzamiento;
                fuerzaLanzamiento = fuerzaBase;
                Debug.Log("fuerza post lanzamiento = " + fuerzaLanzamiento);
                tiempoPulsado = 0f;

                //consumimos mana fijo por ahora
                if (finalManaCost <= player.currentMana)
                {
                    player.UseSkill(finalManaCost);
                    target.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode.Impulse);
                }
                finalManaCost = 0f;
            }
        }

    }
    //FUNCIONES UTILIDAD
    private bool CheckIfRayHitObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(player.position, player.forward, Color.red, Mathf.Infinity);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask, QueryTriggerInteraction.Collide))
        {

            if (target == null)
            {

                target = hit.collider.gameObject;
                Debug.Log("TARGET ENCONTRADO!");
                Debug.Log("target = " + target.name);
                maxDistanceFromTargetToPlayer = Vector3.Distance(target.transform.position, gameObject.transform.position);
                selectedObjectScript = target.GetComponent<SpecialObject>();
                print(target.name);
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
                print("collision con un objeto atraible");
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);
            return false;
        }
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
}
