using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject player = Instantiate(playerPrefab, transform.position + new Vector3(2.5f, 0f, 2.5f), Quaternion.identity);
        player.name = "Player";
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
