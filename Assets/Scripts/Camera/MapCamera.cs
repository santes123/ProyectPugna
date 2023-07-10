using System.Collections;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Transform target = null;  // Referencia al transform del objetivo
    public float cameraHeight = 10f;  // Altura deseada de la c�mara
    private void Awake()
    {
        StartCoroutine(FindPlayer());
    }

    void LateUpdate()
    {
        if (target != null && FindObjectOfType<MapDisplay>().GetIsMapVisible())
        {
            // Mantener la posici�n x, y, z del objetivo
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);

            // Mantener la altura de la c�mara
            targetPosition.y = cameraHeight;

            // Establecer la posici�n de la c�mara
            transform.position = targetPosition;
        }

    }
    IEnumerator FindPlayer()
    {
        bool sw = true;
        while (sw)
        {
            yield return null;
            if (FindObjectOfType<PlayerStats>() != null)
            {
                target = FindObjectOfType<PlayerStats>().gameObject.transform;
                sw = false;
            }

        }
    }
}
