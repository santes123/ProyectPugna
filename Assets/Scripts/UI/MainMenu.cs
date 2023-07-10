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
    private string sceneToLoad;
    GameState playerData;
    private void Start()
    {
        /*if (File.Exists("savegame.bin"))
        {
            loadButton.gameObject.SetActive(true);
        }*/
        //verificar si el archivo existe y tiene informacion de la ubicacion del jugador
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.Log("Archivo de guardado encontrado...");

            _LoadData();
            playerData = GameData.Data.PlayerData;
            if (playerData.currentPlayerHealth > 0)
            {
                Debug.Log("Archivo de guardado encontrado...y hay datos");
                loadButton.gameObject.SetActive(true);
            }
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
        Debug.Log("ruta de guardado = " + Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            File.Delete(Application.persistentDataPath + fileName);
            //Debug.Log("Archivo eliminado correctamente");
            // Verificar si el archivo ha sido eliminado
            
            if (!File.Exists(Application.persistentDataPath + fileName))
            {
                Debug.Log("El archivo se eliminó correctamente.");
            }
            else
            {
                Debug.Log("No se pudo eliminar el archivo.");
            }
        }
        else
        {
            Debug.Log("No se ha encontrado el archivo de guardado...");
        }
        //SceneManager.LoadScene("CameraScene");
        //cargar la informacion del persistanceData para cargar el mapa
        GlobalVars.lastSceneBeforeDeadOrSave = "Level 1 Laboratory";
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        //usamos el menu manager para cambiar de escena y guardar la escena en una pila
        //FindObjectOfType<MenuManager>().CambiarEscena(SceneManager.GetActiveScene().name);
        _LoadData();
        GameData.ResetData();
        MenuManager.CambiarEscena("Level 1 Laboratory");
        //SceneManager.LoadScene("Level 1 Laboratory");
    }
    public void Settings()
    {
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        //usamos el menu manager para cambiar de escena y guardar la escena en una pila
        //FindObjectOfType<MenuManager>().CambiarEscena(SceneManager.GetActiveScene().name);
        MenuManager.CambiarEscena("SettingsMenu");
        //SceneManager.LoadScene("SettingsMenu");
    }
    public void Credits()
    {
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        //usamos el menu manager para cambiar de escena y guardar la escena en una pila
        //FindObjectOfType<MenuManager>().CambiarEscena(SceneManager.GetActiveScene().name);
        MenuManager.CambiarEscena("CreditsScene");
        //SceneManager.LoadScene("CreditsScene");
    }
    public void LoadGame()
    {
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;

        _LoadData();
        //SceneManager.LoadScene("CameraScene");
        if (sceneToLoad != "" && sceneToLoad != null)
        {
            //usamos el menu manager para cambiar de escena y guardar la escena en una pila
            MenuManager.CambiarEscena(sceneToLoad);
            //SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            //usamos el menu manager para cambiar de escena y guardar la escena en una pila
            MenuManager.CambiarEscena("Level 1 Laboratory");
            //SceneManager.LoadScene("Level 1 Laboratory");
        }

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
    public void _LoadData()
    {
        GameData.Init();
        sceneToLoad = GameData.Data.PlayerData.lastScenePlayed;
        //luego cargar toda la data de la partida aqui
        /*
        Debug.Log("loading data...");
        //GameData.Init();
        Debug.Log("GAMEDATA = " + GameData.Data.PlayerData.currentPlayerHealth);
        playerData = GameData.Data.PlayerData;
        if (playerData.currentPlayerHealth > 0)
        {
            //Debug.Log("CURRENT HEALTH = " + playerData.currentPlayerHealth);
            //asignar valores
            //player = GameObject.Find("Player").GetComponent<PlayerStats>();
            player.SetCurrentHeath(playerData.currentPlayerHealth);
            //player.currentHealth = playerData.currentPlayerHealth;
            player.SetCurrentMana(playerData.currentPlayerMana);
            //player.currentMana = playerData.currentPlayerMana;
            Vector3 LastPosition = new Vector3(playerData.playerPositionX, playerData.playerPositionY, playerData.playerPositionZ);
            player.transform.position = LastPosition;
            enemiesKilled = playerData.enemiesEliminated;
            player.selectedMode = playerData.lastSelectedMode;

            //Debug.Log("enemies killed data -> " + playerData.enemiesEliminated);
            //Debug.Log("mana cargado = " + playerData.currentPlayerMana);
            //Debug.Log("selected mode cargado = " + playerData.lastSelectedMode);

            if (enemiesKilled != null)
            {
                EliminateEnemiesKilledBefore(enemiesKilled);
            }

        }
        */
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
