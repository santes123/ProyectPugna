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
            //para que no vuelva cuando acabe el specialThrow
            boomerangGO.GetComponent<BoomerangController>().isReturning = false;
            if (!boomerang.specialThrow)
            {
                boomerangGO.GetComponent<FollowLine>().ClearWayPoints();
                boomerangGO.GetComponent<LineRenderer>().enabled = false;
            }

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
        }
        else if (Input.GetKeyDown(attractThrowMode))
        {
            if (boomerang.onHand) 
            {
                boomerangGO.SetActive(false);
            }
            else
            {
                boomerang.specialThrow = false;
                boomerang.bouncing = false;
                boomerangGO.GetComponent<Rigidbody>().useGravity = true;
                boomerangGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
                boomerang.onGround = true;
                boomerang.rotation = false;
                boomerangGO.GetComponent<Rigidbody>().isKinematic = false;
                boomerangGO.GetComponent<Collider>().isTrigger = false;
                boomerang.isFlying = false;
                boomerang.isReturning = false;
                boomerang.expectedColdownTime = 0f;
            }
            mode.text = "ATTRACT-THROW MODE";
            selectedMode = GameMode.AttractThrow;
        }
        else if (Input.GetKeyDown(PsyquicShot))
        {
            if (boomerang.onHand)
            {
                boomerangGO.SetActive(false);
            }
            else
            {
                boomerangGO.GetComponent<BoomerangController>().specialThrow = false;
                boomerangGO.GetComponent<BoomerangController>().bouncing = false;
                boomerangGO.GetComponent<Rigidbody>().useGravity = true;
                boomerangGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
                boomerangGO.GetComponent<BoomerangController>().onGround = true;
                boomerangGO.GetComponent<BoomerangController>().rotation = false;
                boomerangGO.GetComponent<Rigidbody>().isKinematic = false;
                boomerangGO.GetComponent<Collider>().isTrigger = false;
                boomerangGO.GetComponent<BoomerangController>().isFlying = false;
                boomerangGO.GetComponent<BoomerangController>().isReturning = false;
            }
            mode.text = "PSYQUIC SHOT MODE";
            selectedMode = GameMode.PyshicShot;
            if (skillAttractThrow.onHand) skillAttractThrow.onHand = false;

        }
    }
}
