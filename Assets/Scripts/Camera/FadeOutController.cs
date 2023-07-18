using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutController : MonoBehaviour
{
    public float fadeDuration = 2f;  // Duración del fadeout en segundos
    public Image fadeImage;  // Referencia al objeto Image que cubre la pantalla

    public float timer;  // Temporizador para el fadeout
    CameraLayerChanger cameraLayerChanger;
    private void Start()
    {
        timer = fadeDuration;  // Inicia el temporizador en el valor máximo para comenzar opaco
        fadeImage.color = new Color(0f, 0f, 0f, 1f);  // Configura el color inicialmente opaco
        cameraLayerChanger = GetComponent<CameraLayerChanger>();
    }

    private void Update()
    {
        if (cameraLayerChanger.finished)
        {
            // Decrementa el temporizador
            timer -= Time.deltaTime;

            // Calcula el valor de alpha para el fadeout
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            // Actualiza el color del objeto Image
            fadeImage.color = new Color(0f, 0f, 0f, alpha);

            // Si el fadeout ha terminado, carga la escena principal
            if (timer <= 0f)
            {
                //SceneManager.LoadScene(nextScene);
                fadeImage.gameObject.SetActive(false);
            }
        }

    }
}
