using UnityEngine;

public class AttractAndThrowEnemiesSKill : MonoBehaviour
{
    public float attractionForce = 10f;
    public float launchForce = 20f;
    public GameObject detectionObject;
    public LayerMask enemyLayer;

    private bool isAttracting;
    private Vector3 attractDirection;
    private Collider[] attractedEnemies;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAttracting = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAttracting = false;
            LaunchEnemies();
        }

        if (isAttracting)
        {
            attractDirection = transform.forward;

            attractedEnemies = Physics.OverlapSphere(detectionObject.transform.position, detectionObject.transform.localScale.x, enemyLayer);
            foreach (Collider enemy in attractedEnemies)
            {
                Vector3 attractionVector = (transform.position - enemy.transform.position).normalized;
                enemy.GetComponent<Rigidbody>().AddForce(attractionVector * attractionForce);
            }
        }
    }

    private void LaunchEnemies()
    {
        foreach (Collider enemy in attractedEnemies)
        {
            Vector3 launchVector = transform.forward;
            enemy.GetComponent<Rigidbody>().AddForce(launchVector * launchForce, ForceMode.Impulse);
        }
    }
}
