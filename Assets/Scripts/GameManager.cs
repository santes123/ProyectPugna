using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    private PlayerStats player;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI manaText;
    //public TextMeshProUGUI pointsText;
    public List<string> enemiesKilled = new List<string>();
    public GameObject pauseMenuUI;

    //GameState saveInfo;
    GameState playerData;
    public string saveFileName = "savegame.bin";
    public bool onPause = false;
    public bool playerInstantiated = false;
    public bool playable = false;
    //public Button button;
    public GameObject noManaTextGO;

    //showtextmessagetoplayer
    public GameObject ShowMessageToPlayerTextGO;
    public GameObject chargeBar;

    //public GameObject buffBar;
    private void Awake()
    {
        chargeBar = FindObjectOfType<ChargeBar>().gameObject;
        chargeBar.SetActive(false);
        //asignacion de variables necesarias
        pauseMenuUI = FindObjectOfType<PauseMenu>().gameObject;
        pauseMenuUI.SetActive(false);
        hpText = FindObjectOfType<HealthBar>().GetComponentInChildren<TextMeshProUGUI>();
        manaText = FindObjectOfType<ManaBar>().GetComponentInChildren<TextMeshProUGUI>();
        //pointsText = FindObjectOfType<HealthBar>().GetComponentInChildren<TextMeshProUGUI>();
        noManaTextGO = FindObjectOfType<FloatingText>().gameObject;
        enemiesKilled = new List<string>();
        //ShowMessageToPlayerTextGO = FindAnyObjectByType<ShowMessageToPlayerText>().gameObject;
        //ShowMessageToPlayerTextGO.SetActive(false);
        //buffBar = FindObjectOfType<PowerupUIBar>().gameObject;
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
                GlobalVars.lastSceneBeforeDeadOrSave = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("GameOverMenu");
            }
            //controlamos la tecla Escape para cuando el jugador quiere pausar (menos cuando el menu tutorial esta abierto)
            if (Input.GetKeyDown(KeyCode.Escape) && !onPause && !FindObjectOfType<TutorialController>().tutorialMenuOpened)
            {
                Debug.Log("PAUSE ON");
                Time.timeScale = 0f;
                onPause = true;
                Cursor.visible = true;
                pauseMenuUI.SetActive(true);
                //deshabilitamos los scripts que molestan
                FindObjectOfType<Crosshairs>().enabled = false;
                FindObjectOfType<PlayerController>().enabled = false;
                FindObjectOfType<PlayerStats>().enabled = false;
                FindObjectOfType<UseBoomerang>().enabled = false;
                FindObjectOfType<DashController>().enabled = false;
                FindObjectOfType<PushAwaySkill>().enabled = false;
                FindObjectOfType<ManaRegeneration>().enabled = false;
                if (FindObjectOfType<BoomerangController>())
                {
                    FindObjectOfType<BoomerangController>().enabled = false;
                    FindObjectOfType<DrawLine>().enabled = false;
                    FindObjectOfType<BoomerangUpgradeController>().enabled = false;
                }
                //button.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && onPause && !FindObjectOfType<TutorialController>().tutorialMenuOpened)
            {
                Debug.Log("PAUSE OFF");

                //habilitamos los scripts de nuevo
                FindObjectOfType<Crosshairs>().enabled = true;
                FindObjectOfType<PlayerController>().enabled = true;
                FindObjectOfType<PlayerStats>().enabled = true;
                FindObjectOfType<UseBoomerang>().enabled = true;
                FindObjectOfType<DashController>().enabled = true;
                FindObjectOfType<PushAwaySkill>().enabled = true;
                FindObjectOfType<ManaRegeneration>().enabled = true;
                if (FindObjectOfType<BoomerangController>())
                {
                    FindObjectOfType<BoomerangController>().enabled = true;
                    FindObjectOfType<DrawLine>().enabled = true;
                    FindObjectOfType<BoomerangUpgradeController>().enabled = true;
                }
                Time.timeScale = 1f;
                Cursor.visible = false;
                //DESACTIVAMOS LOS PANELES SETTINGS Y CONTROLS
                GeneralUIFunctions[] elementos = pauseMenuUI.GetComponentsInChildren<GeneralUIFunctions>(true);
                elementos[0].gameObject.SetActive(false);
                elementos[1].gameObject.SetActive(false);
                elementos[1].gameObject.SetActive(false);
                GameObject settingsPanel = FindObjectOfType<SettingsPanel>(true).gameObject;
                settingsPanel.transform.Find("PanelMenuAudio").gameObject.SetActive(false);
                pauseMenuUI.SetActive(false);
                onPause = false;
                //verificar si el mapa esta activado
                if (FindObjectOfType<MapDisplay>().GetIsMapVisible())
                {
                    FindObjectOfType<MapDisplay>().ActiveDisableMapFromMenu();
                }
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
        //Debug.Log("loading data...");
        //GameData.Init();
        //Debug.Log("GAMEDATA = " + GameData.Data.PlayerData.currentPlayerHealth);
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
            //cargamos el booleano para el tutorial y desactivamos el tutorial
            GlobalVars.tutorialCompleted = playerData.tutorialCompleted;
            GameObject.Find("TutorialItems").SetActive(false);

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
                //Debug.Log("player = " + player.name);
                playable = true;
                //buscamos todos los audiosource y les seteamos el volumen /mute
                SetAudioSourcesVolume();
            }
            yield return null;
        }
    }
    //funcion para mostrar texto en pantalla cuando no tienes mana para realizar una accion e intentas hacerla igualmente
    public void ShowNoManaText()
    {
        if (noManaTextGO != null)
        {
            if (!noManaTextGO.GetComponent<Text>().isActiveAndEnabled)
            {
                noManaTextGO.GetComponent<Text>().enabled = true;
            }
            FloatingText floatingDamageText = noManaTextGO.GetComponent<FloatingText>();


            if (noManaTextGO != null)
            {
                floatingDamageText.SetText("NO TIENES SUFICIENTE MANA!", Color.red);
            }
        }
    }
    public void SetAudioSourcesVolume()
    {
        // Obtener todos los componentes AudioSource en la escena
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Iterar sobre cada componente AudioSource encontrado
        foreach (AudioSource audioSource in audioSources)
        {
            // Hacer algo con cada AudioSource encontrado
            Debug.Log("AudioSource encontrado: " + audioSource.gameObject.name);
            audioSource.volume = GlobalVars.generalVolume;
            audioSource.mute = GlobalVars.muteOn;

        }
    }

}
