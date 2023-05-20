using UnityEngine;

public class DashController : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public KeyCode dashKey = KeyCode.E;
    public int maxCharges = 3;
    public float chargeRegenTime = 5f;

    private bool isDashing = false;
    private Vector3 dashStartPosition;
    private Vector3 dashTargetPosition;
    private float dashStartTime;
    private int currentCharges;
    private float lastDashTime;

    public LayerMask obstacleLayer;
    CharacterController characterController;
    private BoxCollider temporaryCollider;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentCharges = maxCharges;
        lastDashTime = -chargeRegenTime;
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
        Debug.Log("cargas actuales : " + currentCharges);
    }
    private bool CanDash()
    {
        return currentCharges > 0 && Time.time - lastDashTime > dashDuration;
    }   
    private void StartDash()
    {
        Debug.Log("Dashing...");
        isDashing = true;
        dashStartPosition = transform.position;
        dashTargetPosition = transform.position + transform.forward * dashDistance;
        dashStartTime = Time.time;

        // Desactivar los colliders u otras lógicas de colisión aquí.
        transform.GetComponent<CharacterController>().enabled = false;
        transform.GetComponent<PlayerController>().enabled = false;
        currentCharges--;
        lastDashTime = Time.time;

        // Crear el BoxCollider temporal
        temporaryCollider = gameObject.AddComponent<BoxCollider>();
        temporaryCollider.isTrigger = true;
        temporaryCollider.size = characterController.bounds.size;
    }

    private void PerformDash()
    {
        float elapsed = Time.time - dashStartTime;
        float t = Mathf.Clamp01(elapsed / dashDuration);

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


        transform.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, t);
        //characterController.Move((dashTargetPosition - dashStartPosition) * t);

        if (t >= 1f)
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
            // Eliminar el BoxCollider temporal
            Destroy(temporaryCollider);
        }
    }
    private void RegenerateCharges()
    {
        if (currentCharges >= maxCharges)
        {
            return;
        }

        if (Time.time - lastDashTime > chargeRegenTime)
        {
            Debug.Log("carga regenerada...");
            currentCharges++;
            lastDashTime = Time.time;
        }
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
}

