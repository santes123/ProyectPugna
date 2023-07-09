using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    GameManager gameManager;
    //public Canvas targetCanvas; // El Canvas en el que se encuentran los elementos
    //public RectTransform targetElement; // El elemento del Canvas que deseas verificar
    public GameObject settingsPanel;
    public GameObject controlsPanel;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            GraphicRaycaster raycaster = targetCanvas.GetComponent<GraphicRaycaster>();
            EventSystem eventSystem = EventSystem.current;

            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = Input.mousePosition;

            // Lista para almacenar los resultados de los raycasts
            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<RectTransform>() == targetElement)
                {
                    Debug.Log("El elemento objetivo del Canvas tiene encima a: " + result.gameObject.name);
                    break;
                }
            }
        }*/
    }
    //FUNCIONES BOTONES DEL MENU DE PAUSE Y SUBMENUS
    //volver a la partida
    public void _ResumeGame()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log("resume game");
        FindObjectOfType<Crosshairs>().enabled = true;
        FindObjectOfType<PlayerController>().enabled = true;
        FindObjectOfType<PlayerStats>().enabled = true;
        FindObjectOfType<UseBoomerang>().enabled = true;
        FindObjectOfType<DashController>().enabled = true;
        FindObjectOfType<PushAwaySkill>().enabled = true;
        FindObjectOfType<ManaRegeneration>().enabled = true;
        FindObjectOfType<BoomerangController>().enabled = true;
        FindObjectOfType<DrawLine>().enabled = true;
        FindObjectOfType<BoomerangUpgradeController>().enabled = true;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        gameManager.onPause = false;
        
    }
    //cerrar el juego
    public void _ExitGame()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log("exit game");
        Time.timeScale = 1f;
        gameManager.onPause = false;
        pauseMenuUI.SetActive(false);
        GlobalVars.lastSceneBeforeDeadOrSave = SceneManager.GetActiveScene().name;
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        MenuManager.CambiarEscena("MainMenu");
        /*
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif*/
        //Application.Quit();
    }
    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
    }
    public void CloseControlsPanel()
    {
        controlsPanel.SetActive(false);
    }
    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    public void OpenMapButton()
    {
        pauseMenuUI.SetActive(false);
        FindObjectOfType<MapDisplay>().ActiveDisableMapFromMenu();
    }
    public void CloseMapButton()
    {
        FindObjectOfType<MapDisplay>().ActiveDisableMapFromMenu();
        pauseMenuUI.SetActive(false);
    }
}
