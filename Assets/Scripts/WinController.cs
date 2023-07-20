using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public GameObject victoryMenu;
    public GameObject fade;

    public bool gameFreezed = false;

    public void ShowVictoryMenu()
    {
        victoryMenu.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
        gameFreezed = true;
        Cursor.visible = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShowVictoryMenu();
            fade.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowVictoryMenu();
            fade.SetActive(true);
        }
    }
}
