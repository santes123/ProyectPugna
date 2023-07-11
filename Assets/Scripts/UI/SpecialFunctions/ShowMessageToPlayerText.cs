using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessageToPlayerText : MonoBehaviour
{
    public GameObject mainObject;
    //texts
    public TextMeshProUGUI topTitle;
    public TextMeshProUGUI centralText;
    //movement
    public float moveSpeed = 2f;
    public float fadeSpeed = 2f;

    public float lifeTime = 2f;
    private float timer = 0f;

    //
    /*public float minAlpha = 0.0f;
    public float maxAlpha = 1.0f;
    public float flickerInterval = 0.5f;

    private Image image;
    private float originalAlpha;*/

    private void Start()
    {
        //image = GetComponent<Image>();

    }
    private void Update()
    {
        if (mainObject.activeSelf)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            //centralText.color = Color.Lerp(centralText.color, Color.clear, fadeSpeed * Time.deltaTime);
            //fadeSpeed = Mathf.Lerp(1,fadeSpeed,3);
            //centralText.color = Color.Lerp(centralText.color, Color.clear, fadeSpeed);

            timer += Time.deltaTime;
            Debug.Log("timeDuration = " +timer);
            if (timer >= lifeTime)
            {
                //centralText.enabled = false;
                //StopShowing();
                mainObject.SetActive(false);
            }
        }
    }
    public void SetText(string title, string text, Color color)
    {
        timer = 0f;
        if (centralText != null)
        {
            topTitle.text = title;
            centralText.text = text;
            centralText.color = color;
        }
        else
        {
            Debug.LogError("Text component not found on the FloatingDamageText object.");
        }
        //lanzamos la corutina

        // Guardar el valor original de la transparencia
        //originalAlpha = image.color.a;

        // Iniciar la corutina del efecto de parpadeo
        //StartCoroutine(FlickerRoutine());
    }
    /*public void ActivateAndSendMessage(string text, string title, Color color)
    {
        //mainObject.SetActive(true);
        SetText(text, title, color);
        
    }*/
    /*private System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Establecer la transparencia al valor mínimo
            SetAlpha(minAlpha);

            // Esperar el intervalo de parpadeo
            yield return new WaitForSeconds(flickerInterval);

            // Establecer la transparencia al valor máximo
            SetAlpha(maxAlpha);

            // Esperar el intervalo de parpadeo
            yield return new WaitForSeconds(flickerInterval);
        }
    }

    private void SetAlpha(float alpha)
    {
        // Obtener el color actual del elemento
        Color currentColor = image.color;

        // Establecer el nuevo valor de transparencia
        currentColor.a = alpha;

        // Asignar el nuevo color al elemento
        image.color = currentColor;
    }

    private void StopShowing()
    {
        // Restaurar la transparencia original antes de destruir el objeto
        SetAlpha(originalAlpha);
        StopCoroutine(FlickerRoutine());
    }*/
}
