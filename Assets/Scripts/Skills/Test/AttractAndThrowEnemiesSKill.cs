using UnityEngine;

public class AttractAndThrowEnemiesSkill : MonoBehaviour
{
    public float attractionForce = 10f;
    public float launchForce = 20f;
    public float attractionRadius = 2f;
    public GameObject detectionObject;
    public LayerMask enemyLayer;

    private bool isAttracting;
    private bool isLaunching;
    private Vector3 attractDirection;
    private Collider[] attractedEnemies;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAttracting = true;
            isLaunching = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAttracting = false;
            isLaunching = true;
            LaunchEnemies();
        }

        if (isAttracting)
        {
            attractedEnemies = Physics.OverlapSphere(detectionObject.transform.position, detectionObject.transform.localScale.x, enemyLayer);
            foreach (Collider enemy in attractedEnemies)
            {
                Vector3 attractionVector = (playerTransform.position - enemy.transform.position).normalized;
                enemy.GetComponent<Rigidbody>().AddForce(attractionVector * attractionForce);
            }
        }

        if (isLaunching)
        {
            // Código para el lanzamiento si es necesario
        }
    }

    private void FixedUpdate()
    {
        if (isAttracting)
        {
            attractedEnemies = Physics.OverlapSphere(detectionObject.transform.position, attractionRadius, enemyLayer);
            foreach (Collider enemy in attractedEnemies)
            {
                enemy.GetComponent<Rigidbody>().MovePosition(playerTransform.position);
            }
        }
    }

    private void LaunchEnemies()
    {
        foreach (Collider enemy in attractedEnemies)
        {
            Vector3 launchVector = playerTransform.forward;
            enemy.GetComponent<Rigidbody>().AddForce(launchVector * launchForce, ForceMode.Impulse);
        }
    }
}
