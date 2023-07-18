using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanelController : MonoBehaviour
{
    //button functions
    public void OpenCreditsScene()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        MenuManager.CambiarEscena("CreditsScene");
    }
    public void SaveAndQuitGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        
        // Aquí puedes agregar tu lógica para guardar la partida

        // Salir del juego
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
