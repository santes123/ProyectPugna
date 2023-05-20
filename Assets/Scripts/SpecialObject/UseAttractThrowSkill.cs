using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAttractThrowSkill : MonoBehaviour
{
    //variables locales
    public bool onColdown = false;
    public GameObject target;
    SpecialObject selectedObjectScript;
    PlayerStats player;

    /// ///////////////////////////////
    public bool estaSiendoAtraido = false;
    public bool haSidoLanzado = false;
    public float velocidadAtraccion;
    //float velocidadAtraccionOriginal;
    public float fuerzaBase;
    float fuerzaLanzamiento;
    public float fuerzaLanzamientoAnterior;
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

    //progresivamente pasar la parte de controles del SpecialObject aqui, tiene mas sentido
    void Start()
    {
        //velocidadAtraccionOriginal = velocidadAtraccion;
        fuerzaLanzamiento = fuerzaBase;
        player = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player.selectedMode == PlayerStats.GameMode.AttractThrow)
        {
            Debug.Log("MODO ATTRACT AND THROW ACTIVADO...");
            if (Input.GetMouseButtonDown(0) && CheckIfRayHitObject() && !onColdown)
            {
                Debug.Log("ATRAYENDO...");
                estaSiendoAtraido = true;
                //asi solo atraemos al objetivo que acabamos de marcar
                selectedObjectScript.estaSiendoAtraido = true;
                onColdown = true;
            }
            if (Input.GetMouseButton(0))
            {
                velocidadAtraccion += 2;
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
            if (Input.GetMouseButtonDown(1))
            {
                botonPresionado = true;
            }
            if (botonPresionado)
            {
                tiempoPulsado += Time.deltaTime;
            }
        }

    }
    private void FixedUpdate()
    {
        if (player.selectedMode == PlayerStats.GameMode.AttractThrow)
        {
            //LANZAMIENTO
            if (Input.GetMouseButtonUp(1) && estaSiendoAtraido && selectedObjectScript.estaSiendoAtraido == true)
            {
                //target.GetComponent<Rigidbody>().Sleep();
                onColdown = false;
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
                Vector3 direccionLanzamiento = (pointer.position - target.transform.position).normalized;

                //target.GetComponent<Rigidbody>().WakeUp();
                target.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode.Impulse);
                direccionLanzamientoAnterior = direccionLanzamiento;
                fuerzaLanzamientoAnterior = fuerzaLanzamiento;
                fuerzaLanzamiento = fuerzaBase;
                Debug.Log("fuerza post lanzamiento = " + fuerzaLanzamiento);
                tiempoPulsado = 0f;
            }
        }

    }
    private bool CheckIfRayHitObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(player.position, player.forward, Color.red, Mathf.Infinity);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask, QueryTriggerInteraction.Collide))
        {
            target = hit.collider.gameObject;
            selectedObjectScript = target.GetComponent<SpecialObject>();
            print(target.name);
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            print("collision con un objeto atraible");
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);
            return false;
        }
    }
}
