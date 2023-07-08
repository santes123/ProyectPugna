using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public CameraController camera;
    public GameManager gameManager;
    private void Awake()
    {
        /*camera = FindObjectOfType<CameraController>();
        camera.enabled = false;
        gameManager = FindObjectOfType<GameManager>();
        gameManager.enabled = false;*/
        GameData.Init();
        /*Debug.Log("last position = " + GameData.Data.PlayerData.playerPositionX);
        Debug.Log("last position = " + GameData.Data.PlayerData.playerPositionY);
        Debug.Log("last position = " + GameData.Data.PlayerData.playerPositionZ);*/
        //verificamos si tiene una posicion de savepoint guardada, sino lo spawneamos en el spawnpoint
        if (GameData.Data.PlayerData.playerPositionX != 0)
        {
            GlobalVars.lastSceneBeforeDeadOrSave = SceneManager.GetActiveScene().name;
            GameObject player = Instantiate(playerPrefab, new Vector3(GameData.Data.PlayerData.playerPositionX,
                GameData.Data.PlayerData.playerPositionY, GameData.Data.PlayerData.playerPositionZ), Quaternion.identity);
            player.name = "Player";
        }
        else
        {
            GlobalVars.lastSceneBeforeDeadOrSave = SceneManager.GetActiveScene().name;
            GameObject player = Instantiate(playerPrefab, transform.position + new Vector3(2.5f, 0f, 2.5f), Quaternion.identity);
            player.name = "Player";
        }

        gameManager = FindObjectOfType<GameManager>();
        gameManager.playerInstantiated = true;
        /*camera.enabled = true;
        gameManager.enabled = true;*/

    }
    void Start()
    {
        //Instantiate(playerPrefab, transform.position + new Vector3( 2.5f, 0f, 2.5f ), Quaternion.identity);
    }

   
}
