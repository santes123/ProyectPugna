using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

public class GameManager : MonoBehaviour
{
    
    private PlayerStats player;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI manaText;
    //public TextMeshProUGUI pointsText;
    public List<string> enemiesKilled = new List<string>();
    public GameObject pauseMenuUI;
    public GameObject pauseMenuUI2;

    //GameState saveInfo;
    GameState playerData;
    public string saveFileName = "savegame.bin";
    public bool onPause = false;
    public bool playerInstantiated = false;
    public bool playable = false;
    private void Awake()
    {
        //asignacion de variables necesarias
        pauseMenuUI = FindObjectOfType<PauseMenu>().gameObject;
        pauseMenuUI.SetActive(false);
        hpText = FindObjectOfType<HealthBar>().GetComponentInChildren<TextMeshProUGUI>();
        manaText = FindObjectOfType<ManaBar>().GetComponentInChildren<TextMeshProUGUI>();
        //pointsText = FindObjectOfType<HealthBar>().GetComponentInChildren<TextMeshProUGUI>();

        enemiesKilled = new List<string>();

        //StartCoroutine(FindPlayer());
        //_LoadData();
        /*if (File.Exists("savegame.bin"))
        {
            //LoadData();
        }
        else
        {
            Debug.Log("No se encontró el archivo de guardado");
            //player = GameObject.Find("Player").GetComponent<PlayerStats>();
            player = FindObjectOfType<PlayerStats>();
        }*/
    }
    void Start()
    {
        //GetComponent<GameManager>().enabled = true;
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        StartCoroutine(FindPlayer());
    }

    void Update()
    {
        if (playable)
        {
            if (player.currentHealth <= 0)
            {
                print("YOU ARE DEAD");
                Cursor.visible = true;
                SceneManager.LoadScene("GameOverMenu");
            }
            //controlamos la tecla Escape para cuando el jugador quiere pausar
            if (Input.GetKeyDown(KeyCode.Escape) && !onPause)
            {
                Debug.Log("PAUSE ON");
                Time.timeScale = 0f;
                onPause = true;
                pauseMenuUI.SetActive(true);
                //pauseMenuUI2.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && onPause)
            {
                Debug.Log("PAUSE OFF");
                Time.timeScale = 1f;
                onPause = false;
                pauseMenuUI.SetActive(false);
                //pauseMenuUI2.SetActive(false);
            }
        }

    }
    //metodo LoadData antiguo
    /*public void LoadData()
    {
        Debug.Log("loading info...");
        // Deserializar el objeto desde un archivo
        var formatter = new BinaryFormatter();
        var stream = new FileStream("savegame.bin", FileMode.Open);
        saveInfo = (GameState)formatter.Deserialize(stream);
        stream.Close();

        //asignar valores
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player.currentHealth = saveInfo.currentPlayerHealth;
        Vector3 LastPosition = new Vector3(saveInfo.playerPositionX, saveInfo.playerPositionY, saveInfo.playerPositionZ);
        player.transform.position = LastPosition;
        enemiesKilled = saveInfo.enemiesEliminated;
        EliminateEnemiesKilledBefore(enemiesKilled);

        //añadir load
    }*/
    public void _LoadData()
    {
        Debug.Log("loading data...");
        GameData.Init();
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

    }
    void EliminateEnemiesKilledBefore(List<string> enemyNames)
    {
        foreach (string enemyName in enemyNames)
        {
            GameObject enemyObject = GameObject.Find(enemyName);

            if (enemyObject != null)
            {
                Destroy(enemyObject);
            }
            else
            {
                Debug.LogWarning($"No se encontró el enemigo {enemyName} en la escena.");
            }
        }
    }
    //buscamos al jugador con una corutina
    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            if (playerInstantiated)
            {
                player = FindObjectOfType<PlayerStats>();
 
                if (player != null) yield return new WaitForSeconds(0.5f);
                _LoadData();
                Debug.Log("player = " + player.name);
                playable = true;
            }
        }
    }
}
