using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    Boomerang,
    AttractThrow,
    PyshicShot,
    Other
}
public class PlayerStats : LivingEntity, IInteractor
{
    PlayerController controller;
    public Text mode;
    public KeyCode boomerangMode = KeyCode.Alpha1;
    public KeyCode attractThrowMode = KeyCode.Alpha2;
    public KeyCode PsyquicShot = KeyCode.Alpha3;

    public GameObject boomerangGO;
    public List<SkillParent> skillList;

    [HideInInspector] public GameMode selectedMode;
    //[HideInInspector] public GameObject targetObject;
    public event System.Action OnGameModeChanges;


    protected override void Start()
    {
        base.Start();
        //base.currentMana = currentMana;
        boomerangGO = FindObjectOfType<BoomerangController>().gameObject;
        controller = GetComponent<PlayerController>();
        mode = GameObject.Find("Mode").GetComponent<Text>();
        skillList.Add(FindObjectOfType<BoomerangUpgradeController>());
        skillList.Add(FindObjectOfType<BoomerangUpgradeController>());

        //inicializamos el mana
        if (currentMana <= 0)
        {
            mana = startingMana;
            currentMana = mana;
        }

        //GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.manaText.text = currentMana.ToString();

        //inicializamos el gamemode, dependiendo de si tiene un archivo de guardado o no
        if (selectedMode == default(GameMode))
        {
            //POR DEFECTO BOOMERANG MODE
            mode.text = "BOOMERANG MODE";
            //selectedMode = GameMode.Boomerang;
            UseBoomerang();

        }
        else
        {
            if (selectedMode == GameMode.Boomerang)
            {
                UseBoomerang();
            }else if (selectedMode == GameMode.AttractThrow)
            {
                UseAttractThrow();
            }else if (selectedMode == GameMode.PyshicShot)
            {
                UsePyshicShot();
            }
            else
            {
                Debug.Log("Ningun modo seleccionado");
            }
        }
    }

    void Update()
    {
        //Debug.Log("onhand specialobject = " + skillAttractThrow.onHand);
        if (Input.GetKeyDown(boomerangMode))
        {
            DisableAllSKills();
            UseBoomerang();
        }
        else if (Input.GetKeyDown(attractThrowMode))
        {
            DisableAllSKills();
            UseAttractThrow();
        }
        else if (Input.GetKeyDown(PsyquicShot))
        {
            DisableAllSKills();
            UsePyshicShot();
        }
    }

    private void UseBoomerang()
    {

        selectedMode = GameMode.Boomerang;
        skillList[0].Activate();
        if (OnGameModeChanges != null)
        {
            OnGameModeChanges();
        }
    }
    private void UseAttractThrow()
    {
        mode.text = "ATTRACT-THROW MODE";
        selectedMode = GameMode.AttractThrow;
        if (OnGameModeChanges != null)
        {
            OnGameModeChanges();
        }
    }
    private void UsePyshicShot()
    {

        mode.text = "PSYQUIC SHOT MODE";
        selectedMode = GameMode.PyshicShot;
        if (OnGameModeChanges != null)
        {
            OnGameModeChanges();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Objeto llave
        if (other.CompareTag("Key"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                IInteractable interfaz = other.gameObject.GetComponent<IInteractable>();
                Interaction interfaz2 = new Interaction();
                interfaz2.source = UnitType.Object;
                //targetObject = other.gameObject;
                DoInteraction(interfaz, interfaz2);
            }
        }
        //objetos a recoger

        //powerup
    }
    //Interfaz para GameObject que interactuan con otros gameObject
    public void DoInteraction(IInteractable target, Interaction interaction)
    {
        //targetObject.GetComponent<SwitchController>().ActivateSwitch();
        //targetObject = null;
        target.Interact(interaction);
    }

    public override void UseSkill(float manaCost)
    {

        base.UseSkill(manaCost);
        Debug.Log("currentmanaPlayerStats = " + currentMana);
    }
    private void DisableAllSKills()
    {
        foreach (SkillParent item in skillList)
        {
            item.Disable();
        }
    }
}
