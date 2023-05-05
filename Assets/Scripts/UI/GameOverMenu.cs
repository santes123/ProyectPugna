using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
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
    }

    public void CloseGame()
    {
        Application.Quit();
    }

}
