using System.Collections;
using UnityEngine;

public class AuxiliarCamera : MonoBehaviour
{
    public Transform player = null;  // Referencia al transform del jugador
    public Vector3 offset;  // Desplazamiento de la c�mara desde la posici�n del jugador
    //public float sensitivity = 2f;  // Sensibilidad del mouse para rotar la c�mara
    //private float mouseX, mouseY;  // Movimiento del mouse para rotar la c�mara
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
           /* // Obtener el movimiento del mouse para la rotaci�n de la c�mara
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, -90f, 90f);  // Limitar el �ngulo de rotaci�n vertical

            // Rotar la c�mara seg�n el movimiento del mouse
            transform.LookAt(player);
            player.rotation = Quaternion.Euler(0, mouseX, 0);  // Rotar el jugador

            // Posicionar la c�mara detr�s del jugador con el desplazamiento especificado
            transform.position = player.position - player.forward * offset.z + player.up * offset.y;
            transform.position = Quaternion.Euler(0, mouseX, 0) * transform.position;

            // Aplicar la rotaci�n vertical a la c�mara
            transform.rotation = Quaternion.Euler(mouseY, transform.eulerAngles.y, 0);*/
        }

    }
    void LateUpdate()
    {
        if (player != null)
        {
            // Obtener la posici�n y rotaci�n del jugador
            Vector3 targetPosition = player.position + offset;
            Quaternion targetRotation = player.rotation;

            // Actualizar la posici�n y rotaci�n de la c�mara
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
