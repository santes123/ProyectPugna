using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaRegeneration : MonoBehaviour
{
    public float manaRegenRate = 1f;  // Tasa de regeneración de mana por minuto
    public float manaRegenInterval = 30f;  // Intervalo de tiempo en segundos para la regeneración automática
    public int manaRegenAmount = 1;  // Cantidad de mana a regenerar
    public int manaRegenOnHitAmount = 5;  // Cantidad de mana a regenerar al golpear a un enemigo
    public GameObject manaRegenParticlesPrefab;  // Prefab de las partículas de regeneración de mana
    private Image manaBarImage;  // Imagen de la barra de mana
    private TextMeshProUGUI manaBarText;  // texto de la barra de mana

    private float nextRegenTime;  // Siguiente tiempo de regeneración automática
    private int currentMana;  // Mana actual del jugador
    private int maxMana;  // Mana máximo del jugador
    private PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        currentMana = maxMana = GetMaxMana();  // Obtener el mana máximo inicial
        nextRegenTime = Time.time + (manaRegenInterval);  // Calcular el siguiente tiempo de regeneración automática
        manaBarImage = FindObjectOfType<ManaBar>().manaBar;
        manaBarText = FindObjectOfType<ManaBar>().GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("NextRegenTime = " + nextRegenTime);
    }

    private void Update()
    {
        currentMana = (int)player.currentMana;
        // Regeneración automática de mana
        //Debug.Log("tiempo app = " + Time.time);
        //Debug.Log("NextRegenTime = " + nextRegenTime);
        if (Time.time >= nextRegenTime && currentMana < maxMana)
        {
            Debug.Log("Regenerando mana...");
            Debug.Log("currentMana start  = " + currentMana);
            currentMana += manaRegenAmount;  // Regenerar el mana
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);  // Asegurarse de que el mana no supere el máximo
            Debug.Log("currentMana end = " + currentMana);
            //actualizamos el current mana de playerStats
            player.currentMana = currentMana;
            // Instanciar las partículas de regeneración de mana
            if (manaRegenParticlesPrefab != null)
            {
                Instantiate(manaRegenParticlesPrefab, transform.position, Quaternion.identity);
            }

            // Actualizar la barra de mana
            UpdateManaBar();

            nextRegenTime = Time.time + (manaRegenInterval);  // Calcular el siguiente tiempo de regeneración automática
        }
    }

    // Método para regenerar mana al golpear a un enemigo
    public void RegenerateManaOnHit()
    {
        currentMana += manaRegenOnHitAmount;  // Regenerar el mana al golpear a un enemigo
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);  // Asegurarse de que el mana no supere el máximo
        //actualizamos el current mana de playerStats
        player.RegenerateMana(manaRegenOnHitAmount);
        // Instanciar las partículas de regeneración de mana
        if (manaRegenParticlesPrefab != null)
        {
            Instantiate(manaRegenParticlesPrefab, transform.position, manaRegenParticlesPrefab.transform.rotation, transform);
        }

        // Actualizar la barra de mana
        UpdateManaBar();
    }

    // Método para actualizar la barra de mana
    private void UpdateManaBar()
    {
        if (manaBarImage != null)
        {
            float fillAmount = (float)currentMana / maxMana;
            Debug.Log("fillAmount  = " + fillAmount);
            manaBarImage.fillAmount = fillAmount;
        }
        if (manaBarText != null)
        {
            manaBarText.text = currentMana.ToString();
        }
    }

    // Método para obtener el mana máximo del jugador (implementación personalizada)
    private int GetMaxMana()
    {
        // Implementa tu propia lógica para obtener el mana máximo del jugador
        // Puede ser un valor constante, una variable, una referencia a otro componente, etc.
        // Aquí, se utiliza un valor constante de ejemplo de 100
        int maxMana = (int)player.startingMana;
        return maxMana;
    }
}
