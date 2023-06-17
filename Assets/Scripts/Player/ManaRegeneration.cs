using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaRegeneration : MonoBehaviour
{
    public float manaRegenRate = 1f;  // Tasa de regeneraci�n de mana por minuto
    public float manaRegenInterval = 30f;  // Intervalo de tiempo en segundos para la regeneraci�n autom�tica
    public int manaRegenAmount = 1;  // Cantidad de mana a regenerar
    public int manaRegenOnHitAmount = 5;  // Cantidad de mana a regenerar al golpear a un enemigo
    public GameObject manaRegenParticlesPrefab;  // Prefab de las part�culas de regeneraci�n de mana
    private Image manaBarImage;  // Imagen de la barra de mana
    private TextMeshProUGUI manaBarText;  // texto de la barra de mana

    private float nextRegenTime;  // Siguiente tiempo de regeneraci�n autom�tica
    private int currentMana;  // Mana actual del jugador
    private int maxMana;  // Mana m�ximo del jugador
    private PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        currentMana = maxMana = GetMaxMana();  // Obtener el mana m�ximo inicial
        nextRegenTime = Time.time + (manaRegenInterval);  // Calcular el siguiente tiempo de regeneraci�n autom�tica
        manaBarImage = FindObjectOfType<ManaBar>().manaBar;
        manaBarText = FindObjectOfType<ManaBar>().GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("NextRegenTime = " + nextRegenTime);
    }

    private void Update()
    {
        currentMana = (int)player.currentMana;
        // Regeneraci�n autom�tica de mana
        //Debug.Log("tiempo app = " + Time.time);
        //Debug.Log("NextRegenTime = " + nextRegenTime);
        if (Time.time >= nextRegenTime && currentMana < maxMana)
        {
            Debug.Log("Regenerando mana...");
            Debug.Log("currentMana start  = " + currentMana);
            currentMana += manaRegenAmount;  // Regenerar el mana
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);  // Asegurarse de que el mana no supere el m�ximo
            Debug.Log("currentMana end = " + currentMana);
            //actualizamos el current mana de playerStats
            player.currentMana = currentMana;
            // Instanciar las part�culas de regeneraci�n de mana
            if (manaRegenParticlesPrefab != null)
            {
                Instantiate(manaRegenParticlesPrefab, transform.position, Quaternion.identity);
            }

            // Actualizar la barra de mana
            UpdateManaBar();

            nextRegenTime = Time.time + (manaRegenInterval);  // Calcular el siguiente tiempo de regeneraci�n autom�tica
        }
    }

    // M�todo para regenerar mana al golpear a un enemigo
    public void RegenerateManaOnHit()
    {
        currentMana += manaRegenOnHitAmount;  // Regenerar el mana al golpear a un enemigo
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);  // Asegurarse de que el mana no supere el m�ximo
        //actualizamos el current mana de playerStats
        player.RegenerateMana(manaRegenOnHitAmount);
        // Instanciar las part�culas de regeneraci�n de mana
        if (manaRegenParticlesPrefab != null)
        {
            Instantiate(manaRegenParticlesPrefab, transform.position, manaRegenParticlesPrefab.transform.rotation, transform);
        }

        // Actualizar la barra de mana
        UpdateManaBar();
    }

    // M�todo para actualizar la barra de mana
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

    // M�todo para obtener el mana m�ximo del jugador (implementaci�n personalizada)
    private int GetMaxMana()
    {
        // Implementa tu propia l�gica para obtener el mana m�ximo del jugador
        // Puede ser un valor constante, una variable, una referencia a otro componente, etc.
        // Aqu�, se utiliza un valor constante de ejemplo de 100
        int maxMana = (int)player.startingMana;
        return maxMana;
    }
}
