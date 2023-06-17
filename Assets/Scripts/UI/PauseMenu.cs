using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        
    }
    //volver a la partida
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameManager.onPause = false;
        pauseMenuUI.SetActive(false);
    }
    //cerrar el juego
    public void ExitGame()
    {
        Time.timeScale = 1f;
        gameManager.onPause = false;
        pauseMenuUI.SetActive(false);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
        //Application.Quit();
    }
}
