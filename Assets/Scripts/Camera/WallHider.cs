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
    private void Awake()
    {
        StartCoroutine(FindPlayer());
    }
    private void Update()
    {
        if (player != null)
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
    
}
