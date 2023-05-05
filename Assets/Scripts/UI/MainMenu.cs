using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject chargeButton;
    private void Start()
    {
        /*if (PlayerPrefs.HasKey("SavePointX"))
        {
            chargeButton.gameObject.SetActive(true);
        }*/
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CameraScene");
    }
    public void Settings()
    {

    }

    public void CloseGame()
    {
        Application.Quit();
    }

}
//rezise de los componentes de UI cuando se modifica el tamaño de la ventana
public class CanvasController : MonoBehaviour
{
    public RectTransform playButton;
    public RectTransform exitButton;

    void Start()
    {
        ResizeButtons();
    }

    void Update()
    {
        ResizeButtons();
    }

    void ResizeButtons()
    {
        float width = Screen.width;
        float height = Screen.height;

        float buttonWidth = width / 5f;
        float buttonHeight = height / 10f;

        playButton.sizeDelta = new Vector2(buttonWidth, buttonHeight);
        playButton.position = new Vector2(width / 2f - buttonWidth / 2f, height / 2f);

        exitButton.sizeDelta = new Vector2(buttonWidth, buttonHeight);
        exitButton.position = new Vector2(width / 2f - buttonWidth / 2f, height / 2f - buttonHeight * 1.5f);
    }
}
