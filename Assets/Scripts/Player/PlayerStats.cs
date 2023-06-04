using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : LivingEntity, IInteractor
{
    PlayerController controller;
    public Text mode;
    public KeyCode boomerangMode = KeyCode.Alpha1;
    public KeyCode attractThrowMode = KeyCode.Alpha2;
    public KeyCode PsyquicShot = KeyCode.Alpha3;

    public GameObject boomerangGO;
    UseAttractThrowSkill skillAttractThrow;
    BoomerangController boomerang;
    UseBoomerang playerBoomerang;
    [HideInInspector] public GameMode selectedMode;
    //[HideInInspector] public GameObject targetObject;
    public enum GameMode
    {
        Boomerang,
        AttractThrow,
        PyshicShot
    }

    protected override void Start()
    {
        boomerangGO = FindObjectOfType<BoomerangController>().gameObject;
        controller = GetComponent<PlayerController>();
        skillAttractThrow = GetComponent<UseAttractThrowSkill>();
        //boomerang = GameObject.Find("Boomer").GetComponent<BoomerangController>();
        boomerang = FindObjectOfType<BoomerangController>();
        //playerBoomerang = GameObject.Find("Player").GetComponent<UseBoomerang>();
        playerBoomerang = FindObjectOfType<UseBoomerang>();
        base.Start();

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
            selectedMode = GameMode.Boomerang;

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
        Debug.Log("onhand specialobject = " + skillAttractThrow.onHand);
        if (Input.GetKeyDown(boomerangMode))
        {
            UseBoomerang();
        }
        else if (Input.GetKeyDown(attractThrowMode))
        {
            UseAttractThrow();
        }
        else if (Input.GetKeyDown(PsyquicShot))
        {
            UsePyshicShot();
        }
    }
    private void UseBoomerang()
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
        if (skillAttractThrow.onHand)
        {
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
    private void UseAttractThrow()
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
            playerBoomerang.expectedColdownTime = 0f;
        }
        mode.text = "ATTRACT-THROW MODE";
        selectedMode = GameMode.AttractThrow;
    }
    private void UsePyshicShot()
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
}
