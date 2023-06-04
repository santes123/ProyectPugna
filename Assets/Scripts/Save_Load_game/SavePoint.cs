using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization;

public class SavePoint : MonoBehaviour
{
    GameState saveInfo;
    GameManager gameManager;
    public GameObject floatingText;

    private void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager = FindObjectOfType<GameManager>();
        floatingText.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingText.SetActive(true);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //floatingText.transform.LookAt(other.gameObject.transform);
            if (Input.GetKeyDown(KeyCode.F))
            {
                _SaveData(other);
                //metodo antiguo guardado
                //SaveData(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingText.SetActive(false);
        }
    }
    /*public void SaveData(Collider other)
    {
        Debug.Log("saving info");
        //asignar valores
        PlayerStats playerstats = other.GetComponent<PlayerStats>();
        GameObject player = GameObject.Find("Player");

        saveInfo = new GameState();
        saveInfo.currentPlayerHealth = playerstats.currentHealth;
        saveInfo.playerPositionX = player.transform.position.x;
        saveInfo.playerPositionY = player.transform.position.y;
        saveInfo.playerPositionZ = player.transform.position.z;
        saveInfo.enemiesEliminated = gameManager.enemiesKilled;

        try
        {
            //serializar
            var formatter = new BinaryFormatter();
            var stream = new FileStream("savegame.bin", FileMode.Create);
            formatter.Serialize(stream, saveInfo);
            stream.Close();
        }
        catch (IOException e)
        {
            Debug.LogError("Error al crear el archivo: " + e.Message);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Error al serializar los datos: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error desconocido: " + e.Message);
        }
    }*/
    public void _SaveData(Collider other)
    {
        Debug.Log("saving data...");
        //asignar valores
        PlayerStats playerstats = other.GetComponent<PlayerStats>();
        //GameObject player = GameObject.Find("Player");
        GameObject player = playerstats.gameObject;
        GameData.Data.PlayerData.currentPlayerHealth = playerstats.currentHealth;
        GameData.Data.PlayerData.currentPlayerMana = playerstats.currentMana;
        GameData.Data.PlayerData.playerPositionX = player.transform.position.x;
        GameData.Data.PlayerData.playerPositionY = player.transform.position.y;
        GameData.Data.PlayerData.playerPositionZ = player.transform.position.z;
        GameData.Data.PlayerData.enemiesEliminated = gameManager.enemiesKilled;
        GameData.Data.PlayerData.lastSelectedMode = playerstats.selectedMode;

        Debug.Log("mana guardado = " + GameData.Data.PlayerData.currentPlayerMana);
        Debug.Log("selected mode guardado = " + GameData.Data.PlayerData.lastSelectedMode);

        GameData.Save();

    }

}
