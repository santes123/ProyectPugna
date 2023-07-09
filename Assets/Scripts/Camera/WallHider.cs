using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHider : MonoBehaviour
{
    public Transform player = null;
    public Camera mainCamera;
    public LayerMask wallLayer;

    private GameObject lastObjectHide = null;
    private GameObject lastBeforeLastObjectHide = null;
    private List<GameObject> LastHits;
    private void Awake()
    {
        StartCoroutine(FindPlayer());
        LastHits = new List<GameObject>();
    }
    private void Update()
    {
        if (player != null)
        {
            //SingleRaycast();
            MultiRaycast();
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
                player = FindObjectOfType<PlayerStats>().gameObject.transform;
                sw = false;
            }

        }
    }
    public void SingleRaycast()
    {
        // Realiza un raycast desde el jugador hacia la cámara
        Ray ray = new Ray(player.position, mainCamera.transform.position - player.position);
        RaycastHit hit;

        // Realiza el raycast y verifica si hay colisiones con otros objetos
        if (Physics.Raycast(ray, out hit, 10f, wallLayer))
        {
            //Debug.Log("hay coliciones");
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);
            // Si el objeto golpeado por el raycast no es el objeto que se debe verificar, ocúltalo
            //Debug.Log("gameobject = " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Obstacle"))
            {

                if (lastObjectHide != null)
                {
                    //lastObjectHide.SetActive(true);
                    lastObjectHide.GetComponent<MeshRenderer>().enabled = true;
                    lastObjectHide = null;
                }
                lastObjectHide = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

        }
        else
        {
            //Debug.Log("no hay coliciones");
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
            // No hay objetos entre el jugador y la cámara, así que muestra el objeto
            if (lastObjectHide != null)
            {
                //lastObjectHide.SetActive(true);
                lastObjectHide.GetComponent<MeshRenderer>().enabled = true;
                lastObjectHide = null;
            }

        }
    }
    public void MultiRaycast()
    {
        Ray ray = new Ray(player.position, mainCamera.transform.position - player.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 10f, wallLayer);
        Debug.Log("hits wall = " + hits.Length);
        if (hits.Length > 0)
        {
            // Itera a través de los resultados del raycast
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                GameObject hitObject = hit.collider.gameObject;
                if (lastObjectHide == null)
                {
                    lastObjectHide = hitObject;
                }
                else
                {
                    lastBeforeLastObjectHide = hitObject;
                }
                LastHits.Add(hitObject);
                lastObjectHide = hitObject;
                hitObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            // Itera a través de los antiguos objetos golpeados
            for (int i = 0; i < LastHits.Count; i++)
            {
                if (lastObjectHide != null)
                {
                    lastObjectHide = null;
                }
                if (lastBeforeLastObjectHide != null)
                {
                lastBeforeLastObjectHide = null;
                }
                LastHits[i].GetComponent<MeshRenderer>().enabled = true;
                LastHits.Remove(LastHits[i]);

            }
        }
    }
}
