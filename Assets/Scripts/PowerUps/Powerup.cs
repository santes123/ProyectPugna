using UnityEngine;
using UnityEngine.UI;

public enum PowerupType
{
    Damage,
    Speed,
    Invincibility
}

public class Powerup : MonoBehaviour
{
    public PowerupType powerupType;
    public float powerupDuration = 5f;
    private float originalDamage;
    private float originalSpeed;
    private GameObject playerReference;

    private PowerupUIBar powerupTimer;
    GameObject buffUI;
    float coldownTime;
    bool onDuration = false;
    public GameObject particlesPrefab;
    private ParticleSystem particlesSystem;
    private GameObject particlesObject;
    private void Start()
    {
        powerupTimer = FindObjectOfType<PowerupUIBar>();
        //instanciamos lel prafab de particles
        particlesObject = Instantiate(particlesPrefab, transform.position, particlesPrefab.transform.rotation);
        particlesObject.transform.SetParent(gameObject.transform);
        particlesSystem = particlesObject.GetComponent<ParticleSystem>();
        particlesSystem.Stop(); // Detiene las partículas inicialmente
    }
    private void Update()
    {
        if (onDuration)
        {
            coldownTime -= Time.deltaTime;
            buffUI.GetComponentInChildren<Text>().text = coldownTime.ToString("F1");
            if (coldownTime <= 0f)
            {
                onDuration = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerReference = other.gameObject;
            //aplicamos el buffo
            ApplyPowerup(other.gameObject);
            //aplicamos el area al jugador
            playerReference.GetComponent<BuffManager>().ApplyBuff(powerupType, powerupDuration);

            Collider collider = GetComponent<Collider>();
            Renderer render = GetComponentInChildren<Renderer>();
            //Destroy(gameObject);
            Destroy(collider);
            Destroy(render);
        }
    }

    private void ApplyPowerup(GameObject player)
    {
        switch (powerupType)
        {
            case PowerupType.Damage:
                // Aplica el power-up de daño al jugador
                // Aquí puedes poner tu código específico para el power-up de daño
                Debug.Log("Power-up de daño activado");
                originalDamage = player.GetComponent<UseBoomerang>().damage;
                player.GetComponent<UseBoomerang>().damage = player.GetComponent<UseBoomerang>().damage * 2;
                buffUI = powerupTimer.AddBuffToBar("DamageBuff");
                coldownTime = powerupDuration;
                break;
            case PowerupType.Speed:
                // Aplica el power-up de velocidad de movimiento al jugador
                // Aquí puedes poner tu código específico para el power-up de velocidad
                Debug.Log("Power-up de velocidad activado");
                originalSpeed = player.GetComponent<PlayerController>().speed;
                player.GetComponent<PlayerController>().speed = player.GetComponent<PlayerController>().speed * 1.5f;
                buffUI = powerupTimer.AddBuffToBar("SpeedBuff");
                coldownTime = powerupDuration;
                break;
            case PowerupType.Invincibility:
                // Aplica el power-up de inmunidad al daño al jugador
                // Aquí puedes poner tu código específico para el power-up de inmunidad
                Debug.Log("Power-up de inmunidad activado");
                player.GetComponent<PlayerController>().invincible = true;
                buffUI = powerupTimer.AddBuffToBar("InvencibilityBuff");
                coldownTime = powerupDuration;
                break;
        }
        particlesSystem.Play(); // Activa las partículas
        onDuration = true;
        // Desactiva el poder después de cierto tiempo
        Invoke(nameof(DeactivatePowerup), powerupDuration);
    }

    private void DeactivatePowerup()
    {
        Debug.Log("desactivando powerup...");
        // Desactiva el efecto del power-up
        switch (powerupType)
        {
            case PowerupType.Damage:
                // Desactiva el power-up de daño
                // Aquí puedes poner tu código específico para desactivar el power-up de daño
                Debug.Log("Power-up de daño desactivado");
                playerReference.GetComponent<UseBoomerang>().damage = originalDamage;
                break;
            case PowerupType.Speed:
                // Desactiva el power-up de velocidad
                // Aquí puedes poner tu código específico para desactivar el power-up de velocidad
                Debug.Log("Power-up de velocidad desactivado");
                playerReference.GetComponent<PlayerController>().speed = originalSpeed;
                break;
            case PowerupType.Invincibility:
                // Desactiva el power-up de inmunidad
                // Aquí puedes poner tu código específico para desactivar el power-up de inmunidad
                Debug.Log("Power-up de inmunidad desactivado");
                playerReference.GetComponent<PlayerController>().invincible = false;
                break;
        }
        Destroy(gameObject);
        Destroy(buffUI);
    }
}
