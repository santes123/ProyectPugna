using UnityEngine;

public class MentalIlusion : MonoBehaviour
{
    public GameObject senueloPrefab; // Prefab del se�uelo
    public float vidaSenuelo = 10f; // Vida del se�uelo
    public float duracionSenuelo = 5f; // Tiempo que durar� el se�uelo
    public float cooldown = 3f; // Tiempo de espera entre habilidades
    private float currentCooldown = 0f; // Variable para controlar el cooldown
    void Update()
    {
        if (currentCooldown <= 0f && Input.GetMouseButtonDown(1) && FindObjectOfType<PlayerStats>().selectedMode == GameMode.PyshicShot)
        {
            CrearSenuelo();
            currentCooldown = cooldown; // Establecer el cooldown
        }

        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime; // Restar el tiempo transcurrido
        }
    }

    void CrearSenuelo()
    {
        // Instanciar el se�uelo en la posici�n adecuada
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        GameObject senuelo1 = Instantiate(senueloPrefab, spawnPosition, Quaternion.identity);
        //GameObject senuelo2 = Instantiate(senueloPrefab, spawnPosition, Quaternion.identity);

        // Asignar la vida y duraci�n al se�uelo
        SenueloBehavior senueloBehavior1 = senuelo1.GetComponent<SenueloBehavior>();
        //SenueloBehavior senueloBehavior2 = senuelo2.GetComponent<SenueloBehavior>();

        senueloBehavior1.AsignarVida(vidaSenuelo);
        //senueloBehavior2.AsignarVida(vidaSenuelo);

        senueloBehavior1.AsignarDuracion(duracionSenuelo);
        //senueloBehavior2.AsignarDuracion(duracionSenuelo);
    }
}
