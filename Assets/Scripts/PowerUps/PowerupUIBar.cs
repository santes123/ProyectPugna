using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIBar : MonoBehaviour
{
    public GameObject buffBar;
    public GameObject buffPrefab;
    public List<Sprite> buffSpriteList;

    private void Awake()
    {
        buffBar = gameObject;
    }
    //funcion que instancia el gameobject en el BuffBar y lo devuelve al script Powerup, donde se borrara cuando sea necesario
    public GameObject AddBuffToBar(string nameOfBuff)
    {
        GameObject newBuff;
        switch (nameOfBuff)
        {
            case "DamageBuff":
                newBuff = Instantiate(buffPrefab, buffBar.transform.position, Quaternion.identity);
                newBuff.GetComponentInChildren<Image>().sprite = buffSpriteList[0];
                newBuff.transform.SetParent(buffBar.transform, false);
                return newBuff;
                //break;
            case "SpeedBuff":
                newBuff = Instantiate(buffPrefab, buffBar.transform.position, Quaternion.identity);
                newBuff.GetComponentInChildren<Image>().sprite = buffSpriteList[1];
                newBuff.transform.SetParent(buffBar.transform, false);
                return newBuff;
                //break;
            case "InvencibilityBuff":
                newBuff = Instantiate(buffPrefab, buffBar.transform.position, Quaternion.identity);
                newBuff.GetComponentInChildren<Image>().sprite = buffSpriteList[2];
                newBuff.transform.SetParent(buffBar.transform, false);
                return newBuff;
                //break;
            default:
                Debug.Log("by defualt");
                return null;
                //break;
        }
    }
}
