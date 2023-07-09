using System.Collections;
using UnityEngine;

public class AuxiliarCamera : MonoBehaviour
{
    public Transform player = null;  // Referencia al transform del jugador
    public Vector3 offset;  // Desplazamiento de la cámara desde la posición del jugador
    //public float sensitivity = 2f;  // Sensibilidad del mouse para rotar la cámara
    //private float mouseX, mouseY;  // Movimiento del mouse para rotar la cámara
    private Quaternion originalRotation;

    private void Awake()
    {
        StartCoroutine(FindPlayer());
        originalRotation = transform.rotation;
        Debug.Log("rotacion original = " + originalRotation.x);
    }
    void Update()
    {
        if (player != null)
        {
           /* // Obtener el movimiento del mouse para la rotación de la cámara
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, -90f, 90f);  // Limitar el ángulo de rotación vertical

            // Rotar la cámara según el movimiento del mouse
            transform.LookAt(player);
            player.rotation = Quaternion.Euler(0, mouseX, 0);  // Rotar el jugador

            // Posicionar la cámara detrás del jugador con el desplazamiento especificado
            transform.position = player.position - player.forward * offset.z + player.up * offset.y;
            transform.position = Quaternion.Euler(0, mouseX, 0) * transform.position;

            // Aplicar la rotación vertical a la cámara
            transform.rotation = Quaternion.Euler(mouseY, transform.eulerAngles.y, 0);*/
        }

    }
    void LateUpdate()
    {
        if (player != null)
        {
            // Obtener la posición y rotación del jugador
            Vector3 targetPosition = player.position + offset;
            Quaternion targetRotation = player.rotation;

            // Actualizar la posición y rotación de la cámara
            transform.position = targetPosition;
            //transform.rotation = new Vector3(originalRotation.x, targetRotation.y, targetRotation.z);
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        }

    }
    //buscamos al jugador con una corutina
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
