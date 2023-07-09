using System.Collections.Generic;
using UnityEngine;

public class DashController : SkillParent
{
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public KeyCode dashKey = KeyCode.E;
    public int maxCharges = 3;


    private bool isDashing = false;
    private Vector3 dashStartPosition;
    private Vector3 dashTargetPosition;
    private float dashStartTime;
    public int currentCharges;
    private float lastDashTime;

    public LayerMask obstacleLayer;
    CharacterController characterController;
    private BoxCollider temporaryCollider;
    public GameObject dashPrefabParticle;
    private GameObject DashInstantiated;

    private List<float> cooldownTimers = new List<float>();
    bool dashCanceled = false;
    public event System.Action OnCurrentChargesUpdate;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentCharges = maxCharges;
        if (OnCurrentChargesUpdate != null)
        {
            OnCurrentChargesUpdate();
        }
        lastDashTime = -coldown;
    }
    private void Update()
    {
        if (CanDash() && Input.GetKeyDown(dashKey))
        {
            StartDash();
        }

        if (isDashing)
        {
            PerformDash();
        }
        RegenerateCharges();
        //Debug.Log("cargas actuales : " + currentCharges);
    }
    private bool CanDash()
    {
        return currentCharges > 0 && Time.time - lastDashTime > dashDuration;
    }   
    private void StartDash()
    {
        /*Debug.Log("Dashing...");
        //isDashing = true;
        dashStartPosition = transform.position;
        dashTargetPosition = transform.position + transform.forward * dashDistance;
        //dashStartTime = Time.time;

        // Desactivar los colliders u otras lógicas de colisión aquí.
        transform.GetComponent<CharacterController>().enabled = false;
        transform.GetComponent<PlayerController>().enabled = false;
        //currentCharges--;
        //lastDashTime = Time.time;
        cooldownTimers.Add(lastDashTime);

        // Crear el BoxCollider temporal
        temporaryCollider = gameObject.AddComponent<BoxCollider>();
        temporaryCollider.isTrigger = true;
        temporaryCollider.size = characterController.bounds.size;*/

        //verificamos si es posible hacer el dash
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, dashDistance, obstacleLayer))
        {
            Debug.Log("collision con obstaculo");
            //isDashing = false;
            //dashCanceled = true;

            //return;
            //dashTargetPosition = transform.position;
            //transform.position = dashStartPosition;
            //dashTargetPosition = hit2.point;
        }
        else
        {
            Debug.Log("Dashing...");
            isDashing = true;
            dashStartPosition = transform.position;
            dashTargetPosition = transform.position + transform.forward * dashDistance;
            //dashStartTime = Time.time;

            // Desactivar los colliders u otras lógicas de colisión aquí.
            transform.GetComponent<CharacterController>().enabled = false;
            transform.GetComponent<PlayerController>().enabled = false;
            //currentCharges--;
            //lastDashTime = Time.time;
            cooldownTimers.Add(lastDashTime);

            // Crear el BoxCollider temporal
            temporaryCollider = gameObject.AddComponent<BoxCollider>();
            temporaryCollider.isTrigger = true;
            temporaryCollider.size = characterController.bounds.size;

            dashStartTime = Time.time;
            currentCharges--;
            if (current_coldown == 0)//modify charges ui
            {//modify charges ui
                lastDashTime = Time.time;
            }//modify charges ui

            //instanciamos el prefab del dash
            DashInstantiated = Instantiate(dashPrefabParticle, transform.position, transform.rotation);
            //llamamos al evento para que actualice las cargas en la UI
            if (OnCurrentChargesUpdate != null)
            {
                OnCurrentChargesUpdate();
            }
        }
        //instanciamos el prefab del dash
        //DashInstantiated = Instantiate(dashPrefabParticle, transform.position, transform.rotation);
    }

    private void PerformDash()
    {
        //modificado coldown
        if (current_coldown == 0)//modify charges ui
        {//modify charges ui
            current_coldown = coldown;
        }//modify charges ui


        float elapsed = Time.time - dashStartTime;
        float t = Mathf.Clamp01(elapsed / dashDuration);

        //raycast para evitar poder pasar muros muy anchos (CAMBIAR EL LAYER EN EL FUTURO)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, dashDistance, obstacleLayer))
        {
            float obstacleDistance = Vector3.Distance(transform.position, hit.point);
            if (obstacleDistance > dashDistance)
            {
                Debug.Log("Demasiado largo");
                isDashing = false;
                return;
            }
        }
        //raycast para evitar que atraveise muros y puertas
        /*RaycastHit hit2;
        if (Physics.Raycast(transform.position, transform.forward, out hit2, dashDistance, obstacleLayer))
        {
            Debug.Log("collision con obstaculo");
            isDashing = false;
            dashCanceled = true;
            //return;
            //dashTargetPosition = transform.position;
            //transform.position = dashStartPosition;
            //dashTargetPosition = hit2.point;
        }*/
        transform.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, t);
        //characterController.Move((dashTargetPosition - dashStartPosition) * t);

        //SUSTITUIR T POR "FLASHFURATION"
        if (t >= 1f || dashCanceled)
        {
            isDashing = false;
            

            // Volver a activar los colliders u otras lógicas de colisión aquí.
            transform.GetComponent<CharacterController>().enabled = true;
            transform.GetComponent<PlayerController>().enabled = true;

            Debug.Log("Dash finished");

            Debug.Log(IsInsideGameObject());

            if (IsInsideGameObject())
            {
                transform.position = dashStartPosition;

                /*isDashing = false;
                return;*/
            }
            if (dashCanceled)
            {
                transform.position = dashStartPosition;
            }
            // Eliminar el BoxCollider temporal
            Destroy(temporaryCollider);
            dashCanceled = false;
        }
    }
    private void RegenerateCharges()
    {
        if (currentCharges >= maxCharges)
        {
            current_coldown = 0;
            return;
        }
        Debug.Log("time.time - lastdashtime" + (Time.time - lastDashTime));
        if (Time.time - lastDashTime > coldown)
        {
            //regeneramos una carga a la vez y en el caso de que haya mas en espera, ponemos a regenerar la siguiente
            //se podria hacer con un array para que se regeneren todas simultaneamente (habrai que cambiaar cosas)
            Debug.Log("carga regenerada...");
            currentCharges++;
            lastDashTime = Time.time;
            if (currentCharges < maxCharges)//modify charges ui
            {//modify charges ui
                current_coldown = coldown;//modify charges ui
            }//modify charges ui
            else//modify charges ui
            {//modify charges ui
                current_coldown = 0;
            }//modify charges ui

            //llamamos al evento para que actualice las cargas en la UI
            if (OnCurrentChargesUpdate != null)
            {
                OnCurrentChargesUpdate();
            }
        }
        //añadir los timers a un list y verificarlos todos en orden
        //current_coldown = Time.time - lastDashTime;
        //modificado coldown
        current_coldown -= Time.deltaTime;
    }
    private bool IsInsideGameObject()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, characterController.bounds.extents, Quaternion.identity, obstacleLayer);
        foreach (Collider collider in colliders)
        {
            if (collider != temporaryCollider && collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public override void Activate()
    {
    }

    public override void Disable()
    {
    }

    //getters y setters
    public int GetCurrentCharges()
    {
        return currentCharges;
    }
    public int GetMaxCharges()
    {
        return maxCharges;
    }
    public void SetCurrentCharges(int newValue)
    {
        currentCharges = newValue;
    }

}

