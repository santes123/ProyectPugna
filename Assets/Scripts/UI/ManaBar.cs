using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image manaBar;
    private float currentMana;
    private float maxMana;
    private PlayerStats player;

    private void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player = FindObjectOfType<PlayerStats>();
        maxMana = player.startingMana;
        currentMana = player.currentMana;
    }
    void Update()
    {
        currentMana = player.currentMana;
        manaBar.fillAmount = currentMana / maxMana;
    }
}
