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
        listOfTextsToShowOnPanel[0] = "El boomerang es tu arma principal y tienes acceso a �l haciendo click en la tecla �1�. El boomerang tiene 2 modalidades: \n " +
            "-<b> Ataque normal: <b> Pulsando el click izquierdo lanzaras el boomerang en l�nea recta hacia el puntero. Si mantienes pulsado, podr�s alargar la " +
            "distancia hasta un m�ximo. \n" +
            "-<b> Ataque especial: <b> Si mantienes pulsado el click derecho y mueves el puntero, podr�s dibujar una l�nea en el suelo y cuando sueltes el click, " +
            "el boomerang seguir� esa l�nea y luego volver� a ti. \n" +
            "Adem�s, en las teclas<b> Q y Z<b> tendr�s las 2 mejoras del boomerang.La primera, al activarla y golpear a un enemigo, har� mas da�o y tambi�n golpear� a " +
            "todos los enemigos en un �rea concreta.La segunda har� m�s da�o y tambi�n congelar� a ese enemigo y a los enemigos cercanos durante 2 segundos.";
        listOfTextsToShowOnPanel[1] = "Aqu� te mostraremos c�mo funciona la habilidad �Atraer y lanzar� y �Golpe Ps�quico�. \n" +
            "-<b> Atraer y lanzar: <b> pulsando la tecla �2� activaras este modo, y podr�s atraer objetos y luego lanzarlos(los ver�s marcados con un borde azul para " +
            "diferenciarlos).Primero deber�s posicionar el puntero sobre el objeto y entonces hacer click izquierdo o mantenerlo(variar� el coste de mana y tambi�n la " +
            "velocidad de atracci�n) y el objeto vendr� hacia ti. Cuando lo tengas en la mano, usando el click derecho o manteni�ndolo(igual que antes) podr�s lanzar " +
            "el objeto hacia la posici�n del puntero.Adem�s, estos objetos har�n da�o a los enemigos la golpearlos. \n" +
            "-<b> Golpe Psiquico: <b> Pulsando la tecla �3� activaras este modo, y pulsando o manteniendo el click izquierdo podr�s cargar un ataque ps�quico. " +
            "Al soltar el click, este ataque se lanzar� en la direcci�n el puntero y har� da�o dependiendo de cuando lo hayas cargado, y tambi�n aumentar� su coste de mana.";
        listOfTextsToShowOnPanel[2] = "Aqu� te mostraremos c�mo funciona la habilidad �Empujar� y �Dash�. \n-<b> Empujar: <b> Esta habilidad se activa directamente " +
            "pulsando la tecla �R�. Empujar� a todos los enemigos en un radio de acci�n concreto hacia fuera y les har� una peque�a cantidad de da�o. \n-<b> Dash: <b> Esta " +
            "habilidad se activa directamente pulsando la tecla �E�. Al usarla, te desplazara(si es posible) varias unidades en la direcci�n a la que estas mirando de forma " +
            "casi instant�nea. Adem�s, esta habilidad tiene 3 cargas, que se van regenerando cada 5 segundos cada una despu�s de usarse. Esta esta habilidad no te permite " +
            "traspasar muros.";
        listOfTextsToShowOnPanel[3] = "Los powerups son unos objetos que encontraras en ciertas zonas del mapa donde hay gran cantidad de enemigos. Te proporcionaran una ventaja temporal y hay " +
            "3 tipos:-<b> Da�o: <b> Aumentaran x2 tu da�o durante 5 segundos.\n-<b> Velocidad: <b> Aumentaran x2 tu velocidad durante 5 segundos.\n-<b> " +
            "invencibilidad: <b> Te har�n invencible durante 5 segundos.\n";
        listOfTextsToShowOnPanel[4] = "Hay diferentes objetos con los que puedes interactuar en el juego: \n-<b> Objetos atraibles: <b> Objetos que veras remarcados con un " +
            "borde azul y los cuales podr�s atraer y luego lanzar. \n-<b> Puertas: <b> Puertas distribuidas por todo el mapa, las cuales solo podras abrir si tienes la llave" +
            " o llaves adecuadas, y que te permitir�n ir avanzando por el nivel. Una vez desbloqueadas, quedar�n desbloqueadas permanentemente. \n-<b> Llaves: <b> Objetos " +
            "que encontraras en el suelo y te permitir�n abrir puertas(cada llave est� asignada a una o varias puertas). Una vez que las consigas, las desbloquearas " +
            "permanentemente. \n-<b> Enemigos: <b> Personajes que deber�s derrotar para poder avanzar, aunque luchar no es siempre la mejor opci�n. \n-<b> Powerups: <b> " +
            "Objetos que te dar�n ventajas temporales para ayudarte en las peleas. \n";
        listOfTextsToShowOnPanel[5] = "El objetivo del juego es escapar del laboratorio, para ello tendr�s que recolectar las diferentes llaves para poder desbloquear las puertas" +
            " y avanzar a lo largo del nivel, a la vez que luchas por tu vida contra los diferentes enemigos. \nUna vez que hayas completado este proceso, te encontraras con " +
            "un objetivo mayor, y si lo superas, habr�s conseguido escapado.";
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
