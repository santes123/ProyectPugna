using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : LivingEntity
{
    PlayerController controller;
    public Text mode;
    public KeyCode boomerangMode = KeyCode.Alpha1;
    public KeyCode attractThrowMode = KeyCode.Alpha2;
    public KeyCode PsyquicShot = KeyCode.Alpha3;

    public GameObject boomerangGO;
    UseAttractThrowSkill skillAttractThrow;
    BoomerangController boomerang;
    [HideInInspector] public GameMode selectedMode;
    public enum GameMode
    {
        Boomerang,
        AttractThrow,
        PyshicShot
    }

    protected override void Start()
    {
        controller = GetComponent<PlayerController>();
        skillAttractThrow = GetComponent<UseAttractThrowSkill>();
        boomerang = GameObject.Find("Boomer").GetComponent<BoomerangController>();
        base.Start();

        //inicializamos el mana
        mana = startingMana;
        currentMana = mana;
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.manaText.text = currentMana.ToString();

        //POR DEFECTO BOOMERANG MODE
        mode.text = "BOOMERANG MODE";

        selectedMode = GameMode.Boomerang;
    }

    void Update()
    {
        Debug.Log("onhand specialobject = " + skillAttractThrow.onHand);
        if (Input.GetKeyDown(boomerangMode))
        {
            boomerangGO.SetActive(true);
            mode.text = "BOOMERANG MODE";
            selectedMode = GameMode.Boomerang;
            if (skillAttractThrow.onHand) {
                //eliminamos todas las variables que hacen que se vuelva a atraer hacia la mano
                Debug.Log("onhand specialobject = true");
                skillAttractThrow.onHand = false;
                skillAttractThrow.target.GetComponent<SpecialObject>().onHand = false;
                skillAttractThrow.estaSiendoAtraido = false;
                skillAttractThrow.target.GetComponent<SpecialObject>().estaSiendoAtraido = false;
                skillAttractThrow.target.GetComponent<SpecialObject>().rb.useGravity = true;
                skillAttractThrow.target.GetComponent<BoxCollider>().enabled = true;
                skillAttractThrow.onColdown = false;
            }

            //ACTIVAR EL BOOMERANG Y FILTRAR PARA QUE FUNCIONE TODO DENTRO DE BOOMERANG (AÑADIR UN ENUM CON BOOMERANG MODE, SKILL 1 MODE Y SKILL 2 MODE
        }
        else if (Input.GetKeyDown(attractThrowMode))
        {
            if (boomerang.onHand) boomerangGO.SetActive(false);
            mode.text = "ATTRACT-THROW MODE";
            selectedMode = GameMode.AttractThrow;
            
            //DESACTIVAR EL BOOMERANG Y FILTRAR PARA QUE NO FUNCIONE NADA DENTRO DE BOOMERANG (SI ESTA ONHAND DESACTIVARLO, SINO DEJARLO ACTIVO)
            //(AÑADIR UN ENUM CON BOOMERANG MODE, SKILL 1 MODE Y SKILL 2 MODE
        }
        else if (Input.GetKeyDown(PsyquicShot))
        {
            if (boomerang.onHand) boomerangGO.SetActive(false);
            mode.text = "PSYQUIC SHOT MODE";
            selectedMode = GameMode.PyshicShot;
            if (skillAttractThrow.onHand) skillAttractThrow.onHand = false;

            //DESACTIVAR EL BOOMERANG Y FILTRAR PARA QUE NO FUNCIONE NADA DENTRO DE BOOMERANG (SI ESTA ONHAND DESACTIVARLO, SINO DEJARLO ACTIVO)
            //(AÑADIR UN ENUM CON BOOMERANG MODE, SKILL 1 MODE Y SKILL 2 MODE

        }
    }
}
