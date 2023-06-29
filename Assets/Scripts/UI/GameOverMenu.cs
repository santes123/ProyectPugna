using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    //AÑADIR PARA PODER CARGAR PARTIDA EN GAMEOVER MENU
    public GameObject chargeButton;
    private void Start()
    {
        /*if (PlayerPrefs.HasKey("SavePointX"))
        {
            chargeButton.gameObject.SetActive(true);
        }*/
    }
    public void ResetGame()
    {
        //SceneManager.LoadScene("CameraScene");
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        //usamos el menu manager para cambiar de escena y guardar la escena en una pila
        //FindObjectOfType<MenuManager>().CambiarEscena(SceneManager.GetActiveScene().name);
        MenuManager.CambiarEscena(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(GlobalVars.lastSceneBeforeDeadOrSave);
        //cargar desde el ultimo savepoint
        Debug.Log("cargando en el ultimo savepoint...");
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

}
