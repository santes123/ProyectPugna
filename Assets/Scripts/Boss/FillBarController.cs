using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FillBarController : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI hpText;
    public float fillDuration = 2f;

    private float currentHealth = 0f;
    private float targetHealth = 100f;

    private void Start()
    {
        //(FillBar());
    }

    public IEnumerator FillBar()
    {
        float timer = 0f;
        float startHealth = currentHealth;

        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            currentHealth = Mathf.Lerp(startHealth, targetHealth, timer / fillDuration);
            fillImage.fillAmount = currentHealth / 100f;
            hpText.text = currentHealth.ToString("F0");
            yield return null;
        }

        // Asegurarse de que la barra de vida esté llena al 100%
        currentHealth = targetHealth;
        fillImage.fillAmount = 1f;
    }
}
