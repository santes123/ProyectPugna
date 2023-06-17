using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadButton;
    GameState saveInfo;
    public string fileName = "/GameData";

    private void Start()
    {
        /*if (File.Exists("savegame.bin"))
        {
            loadButton.gameObject.SetActive(true);
        }*/
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.Log("Archivo de guardado encontrado...");
            loadButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("No se ha encontrado el archivo de guardado...");
        }
    }

    public void PlayGame()
    {
        /*if (File.Exists("savegame.bin"))
        {
            File.Delete("savegame.bin");
            Debug.Log("Archivo eliminado correctamente");
        }
        else
        {
            Debug.Log("No se encontró el archivo a eliminar");
        }*/
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            File.Delete(Application.persistentDataPath + fileName);
            Debug.Log("Archivo eliminado correctamente");
        }
        else
        {
            Debug.Log("No se ha encontrado el archivo de guardado...");
        }
        SceneManager.LoadScene("CameraScene");
    }
    public void Settings()
    {

    }
    public void LoadGame()
    {
        SceneManager.LoadScene("CameraScene");
    }

    public void CloseGame()
    {
        //Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    //CARGADO DE DATOS


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
