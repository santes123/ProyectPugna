using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : LivingEntity
{
    PlayerController controller;
    protected override void Start()
    {
        controller = GetComponent<PlayerController>();
        base.Start();

        //inicializamos el mana
        mana = startingMana;
        currentMana = mana;
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.manaText.text = currentMana.ToString();
    }

    void Update()
    {
        
    }
}
