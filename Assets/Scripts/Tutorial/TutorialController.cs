using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject panelShow;
    public TextMeshProUGUI panelShowText;
    public string[] listOfTextsToShowOnPanel;
    public GameObject[] listOfTutorialItems;
    public float intervalToCheckIfTutorialCompleted;
    public GameObject tutorialBlocker;
    public bool tutorialMenuOpened = false;
    public Puerta puertaDeTutorial;
    void Start()
    {
        FulFillTexts();
        listOfTutorialItems = GameObject.FindGameObjectsWithTag("TutorialItem");
        InvokeRepeating("VerifyIfTutorialIsCompleted", 0, intervalToCheckIfTutorialCompleted);
    }

    void Update()
    {
        //VerifyIfTutorialIsCompleted();
    }
    public void OpenTextPanelAndShowText(TutorialSelection item)
    {
        PauseGame();
        OpenPanelText();
        switch (item)
        {
            case TutorialSelection.BoomerangBasics:
                panelShowText.text = listOfTextsToShowOnPanel[0];
                break;
            case TutorialSelection.Skills1:
                panelShowText.text = listOfTextsToShowOnPanel[1];
                break;
            case TutorialSelection.Skills2:
                panelShowText.text = listOfTextsToShowOnPanel[2];
                break;
            case TutorialSelection.Powerups:
                panelShowText.text = listOfTextsToShowOnPanel[3];
                break;
            /*case TutorialSelection.BoomerangUpgrades:
                panelShowText.text = listOfTextsToShowOnPanel[1];
                break;
            case TutorialSelection.SkillAttractAndThrow:
                panelShowText.text = listOfTextsToShowOnPanel[2];
                break;
            case TutorialSelection.SkillPsychicShot:
                panelShowText.text = listOfTextsToShowOnPanel[3];
                break;
            case TutorialSelection.SKillPushAway:
                panelShowText.text = listOfTextsToShowOnPanel[4];
                break;
            case TutorialSelection.SkillDash:
            panelShowText.text = listOfTextsToShowOnPanel[5];
            break;*/
            case TutorialSelection.InteractWithObjects:
                panelShowText.text = listOfTextsToShowOnPanel[4];
                break;
            /*case TutorialSelection.KeysAndDoors:
                panelShowText.text = listOfTextsToShowOnPanel[7];
                break;*/
            case TutorialSelection.GameObjetive:
                panelShowText.text = listOfTextsToShowOnPanel[5];
                break;
            default:
                panelShowText.text = listOfTextsToShowOnPanel[0];
                break;
        }
        Debug.Log("text = " + panelShowText.text);
        //detener la ejecucion del juego
    }
    //funciones botones
    public void OpenPanelText()
    {
        panelShow.SetActive(true);
        tutorialMenuOpened = true;
    }
    public void ClosePanelText()
    {
        panelShow.SetActive(false);
        tutorialMenuOpened = false;
        UnpauseGame();
        //Invoke("UnpauseGame", 1f);
    }
    public void PauseGame()
    {
        Debug.Log("PAUSE ON");
        //Time.timeScale = 0f;
        Cursor.visible = true;
        //deshabilitamos los scripts que molestan
        FindObjectOfType<Crosshairs>().enabled = false;
        FindObjectOfType<PlayerController>().enabled = false;
        FindObjectOfType<PlayerStats>().enabled = false;
        FindObjectOfType<UseBoomerang>().enabled = false;
        FindObjectOfType<DashController>().enabled = false;
        FindObjectOfType<PushAwaySkill>().enabled = false;
        FindObjectOfType<ManaRegeneration>().enabled = false;
        if (FindObjectOfType<BoomerangController>())
        {
            FindObjectOfType<BoomerangController>().enabled = false;
            FindObjectOfType<DrawLine>().enabled = false;
            FindObjectOfType<BoomerangUpgradeController>().enabled = false;
        }
    }
    public void UnpauseGame()
    {
        Debug.Log("PAUSE OFF");
        //Time.timeScale = 1f;
        Cursor.visible = false;
        //deshabilitamos los scripts que molestan
        FindObjectOfType<Crosshairs>().enabled = true;
        FindObjectOfType<PlayerController>().enabled = true;
        FindObjectOfType<PlayerStats>().enabled = true;
        FindObjectOfType<UseBoomerang>().enabled = true;
        FindObjectOfType<DashController>().enabled = true;
        FindObjectOfType<PushAwaySkill>().enabled = true;
        FindObjectOfType<ManaRegeneration>().enabled = true;
        if (FindObjectOfType<BoomerangController>())
        {
            FindObjectOfType<BoomerangController>().enabled = true;
            FindObjectOfType<DrawLine>().enabled = true;
            FindObjectOfType<BoomerangUpgradeController>().enabled = true;
        }
    }
    public bool VerifyIfTutorialIsCompleted()
    {
        int counterTutorialCompleted = 0;
        for (int i = 0; i < listOfTutorialItems.Length; i++)
        {
            if (listOfTutorialItems[i].GetComponent<TutorialItem>().completed)
            {
                counterTutorialCompleted++;
            }
        }
        Debug.Log("lenght tutorialItems = " + listOfTutorialItems.Length);
        Debug.Log("lenght tutorialItems counter = " + counterTutorialCompleted);
        if (counterTutorialCompleted == listOfTutorialItems.Length)
        {
            tutorialBlocker.SetActive(false);
            CancelInvoke("VerifyIfTutorialIsCompleted");
            Debug.Log("TUTORIAL COMPLETED");
            GlobalVars.tutorialCompleted = true;
            TutorialCompleted();
            return true;
        }
        else
        {
            return false;
        }
       
    }
    private void TutorialCompleted()
    {
        FindObjectOfType<ShowMessageToPlayerText>().SetText("Tutorial completado!", "Has compleato el tutorial satisfactoriamente. Si tienes cualquier duda, revisa la seccion controles" +
            "dentro del menu de pausa o de menu principal. Diviertete!", Color.green);
        //ponemos al maximo todo lo que haya podido usar
        FindObjectOfType<PlayerStats>().SetCurrentHeath(FindObjectOfType<PlayerStats>().startingHealth);
        FindObjectOfType<PlayerStats>().SetCurrentMana(FindObjectOfType<PlayerStats>().startingMana);
        FindObjectOfType<DashController>().currentCharges = FindObjectOfType<DashController>().maxCharges;
        FindObjectOfType<DashController>().currentCharges = FindObjectOfType<DashController>().maxCharges;
        puertaDeTutorial.UnlockPuerta();
    }
    public void FulFillTexts()
    {
        listOfTextsToShowOnPanel = new string[6];
        listOfTextsToShowOnPanel[0] = "El boomerang es tu arma principal y tienes acceso a él haciendo click en la tecla ‘1’. El boomerang tiene 2 modalidades: \n " +
            "-<b> Ataque normal: <b> Pulsando el click izquierdo lanzaras el boomerang en línea recta hacia el puntero. Si mantienes pulsado, podrás alargar la " +
            "distancia hasta un máximo. \n" +
            "-<b> Ataque especial: <b> Si mantienes pulsado el click derecho y mueves el puntero, podrás dibujar una línea en el suelo y cuando sueltes el click, " +
            "el boomerang seguirá esa línea y luego volverá a ti. \n" +
            "Además, en las teclas<b> Q y Z<b> tendrás las 2 mejoras del boomerang.La primera, al activarla y golpear a un enemigo, hará mas daño y también golpeará a " +
            "todos los enemigos en un área concreta.La segunda hará más daño y también congelará a ese enemigo y a los enemigos cercanos durante 2 segundos.";
        listOfTextsToShowOnPanel[1] = "Aquí te mostraremos cómo funciona la habilidad “Atraer y lanzar” y “Golpe Psíquico”. \n" +
            "-<b> Atraer y lanzar: <b> pulsando la tecla “2” activaras este modo, y podrás atraer objetos y luego lanzarlos(los verás marcados con un borde azul para " +
            "diferenciarlos).Primero deberás posicionar el puntero sobre el objeto y entonces hacer click izquierdo o mantenerlo(variará el coste de mana y también la " +
            "velocidad de atracción) y el objeto vendrá hacia ti. Cuando lo tengas en la mano, usando el click derecho o manteniéndolo(igual que antes) podrás lanzar " +
            "el objeto hacia la posición del puntero.Además, estos objetos harán daño a los enemigos la golpearlos. \n" +
            "-<b> Golpe Psiquico: <b> Pulsando la tecla “3” activaras este modo, y pulsando o manteniendo el click izquierdo podrás cargar un ataque psíquico. " +
            "Al soltar el click, este ataque se lanzará en la dirección el puntero y hará daño dependiendo de cuando lo hayas cargado, y también aumentará su coste de mana.";
        listOfTextsToShowOnPanel[2] = "Aquí te mostraremos cómo funciona la habilidad “Empujar” y “Dash”. \n-<b> Empujar: <b> Esta habilidad se activa directamente " +
            "pulsando la tecla ‘R’. Empujará a todos los enemigos en un radio de acción concreto hacia fuera y les hará una pequeña cantidad de daño. \n-<b> Dash: <b> Esta " +
            "habilidad se activa directamente pulsando la tecla ‘E’. Al usarla, te desplazara(si es posible) varias unidades en la dirección a la que estas mirando de forma " +
            "casi instantánea. Además, esta habilidad tiene 3 cargas, que se van regenerando cada 5 segundos cada una después de usarse. Esta esta habilidad no te permite " +
            "traspasar muros.";
        listOfTextsToShowOnPanel[3] = "Los powerups son unos objetos que encontraras en ciertas zonas del mapa donde hay gran cantidad de enemigos. Te proporcionaran una ventaja temporal y hay " +
            "3 tipos:-<b> Daño: <b> Aumentaran x2 tu daño durante 5 segundos.\n-<b> Velocidad: <b> Aumentaran x2 tu velocidad durante 5 segundos.\n-<b> " +
            "invencibilidad: <b> Te harán invencible durante 5 segundos.\n";
        listOfTextsToShowOnPanel[4] = "Hay diferentes objetos con los que puedes interactuar en el juego: \n-<b> Objetos atraibles: <b> Objetos que veras remarcados con un " +
            "borde azul y los cuales podrás atraer y luego lanzar. \n-<b> Puertas: <b> Puertas distribuidas por todo el mapa, las cuales solo podras abrir si tienes la llave" +
            " o llaves adecuadas, y que te permitirán ir avanzando por el nivel. Una vez desbloqueadas, quedarán desbloqueadas permanentemente. \n-<b> Llaves: <b> Objetos " +
            "que encontraras en el suelo y te permitirán abrir puertas(cada llave está asignada a una o varias puertas). Una vez que las consigas, las desbloquearas " +
            "permanentemente. \n-<b> Enemigos: <b> Personajes que deberás derrotar para poder avanzar, aunque luchar no es siempre la mejor opción. \n-<b> Powerups: <b> " +
            "Objetos que te darán ventajas temporales para ayudarte en las peleas. \n";
        listOfTextsToShowOnPanel[5] = "El objetivo del juego es escapar del laboratorio, para ello tendrás que recolectar las diferentes llaves para poder desbloquear las puertas" +
            " y avanzar a lo largo del nivel, a la vez que luchas por tu vida contra los diferentes enemigos. \nUna vez que hayas completado este proceso, te encontraras con " +
            "un objetivo mayor, y si lo superas, habrás conseguido escapado.";
    }
}
public enum TutorialSelection
{
    /*BoomerangBasics,
    BoomerangUpgrades,
    SkillAttractAndThrow,
    SkillPsychicShot,
    SKillPushAway,
    SkillDash,
    InteractWithObjects,
    KeysAndDoors*/
    BoomerangBasics, //todo sobre el boomerang
    Skills1,  //skills attract and throw y Psychic shot
    Skills2,  //skills push away y dash
    Powerups,  //todo sobre los tipos de powerups y donde encontrarlos
    InteractWithObjects,  //interaccion con los differentes objetos del mapa
    GameObjetive  //
}
