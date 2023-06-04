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
        SceneManager.LoadScene("CameraScene");
        //cargar desde el ultimo savepoint
        Debug.Log("cargando en el ultimo savepoint...");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

}
