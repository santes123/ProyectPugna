using UnityEngine;

public class BoomerangMovement : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float throwForce;

    private float t;
    private bool isMoving;

    public void ThrowBoomerang(Vector3 direction, float force)
    {
        isMoving = true;
        initialPosition = transform.position;
        targetPosition = initialPosition + direction * 10f; // Distancia de lanzamiento
        throwForce = force;
        t = 0f;
    }

    private void Update()
    {
        if (isMoving)
        {
            t += Time.deltaTime / (throwForce * 2f); // Ajusta la velocidad del movimiento
            Debug.Log("t = " + t);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            if (t >= 1f)
            {
                isMoving = false;
                // Aquí puedes agregar cualquier código adicional al finalizar el movimiento del boomerang
            }
        }
    }
}
