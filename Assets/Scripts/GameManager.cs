using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    
    private PlayerStats player;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI pointsText;
    public List<string> enemiesKilled = new List<string>();

    GameState saveInfo;
    GameState playerData;
    public string saveFileName = "savegame.bin";

    private void Awake()
    {
        enemiesKilled = new List<string>();
        _LoadData();
        if (File.Exists("savegame.bin"))
        {
            //LoadData();
        }
        else
        {
            Debug.Log("No se encontró el archivo de guardado");
            //player = GameObject.Find("Player").GetComponent<PlayerStats>();
            player = FindObjectOfType<PlayerStats>();
        }
    }
    void Start()
    {
        //GetComponent<GameManager>().enabled = true;
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player.currentHealth <= 0)
        {
            print("YOU ARE DEAD");
            SceneManager.LoadScene("GameOverMenu");
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
        playerData = GameData.Data.PlayerData;
        Debug.Log("CURRENT HEALTH = " + playerData.currentPlayerHealth);
        //asignar valores
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player.currentHealth = playerData.currentPlayerHealth;
        player.currentMana = playerData.currentPlayerMana;
        Vector3 LastPosition = new Vector3(playerData.playerPositionX, playerData.playerPositionY, playerData.playerPositionZ);
        player.transform.position = LastPosition;
        enemiesKilled = playerData.enemiesEliminated;
        player.selectedMode = playerData.lastSelectedMode;

        Debug.Log("enemies killed data -> " + playerData.enemiesEliminated);
        Debug.Log("mana cargado = " + playerData.currentPlayerMana);
        Debug.Log("selected mode cargado = " + playerData.lastSelectedMode);

        if (enemiesKilled != null)
        {
            EliminateEnemiesKilledBefore(enemiesKilled);
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

}
