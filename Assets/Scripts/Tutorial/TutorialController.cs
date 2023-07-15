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
    void Start()
    {
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
            case TutorialSelection.BoomerangUpgrades:
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
                break;
            case TutorialSelection.InteractWithObjects:
                panelShowText.text = listOfTextsToShowOnPanel[6];
                break;
            case TutorialSelection.KeysAndDoors:
                panelShowText.text = listOfTextsToShowOnPanel[7];
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
        Time.timeScale = 0f;
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
        Time.timeScale = 1f;
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
    }
}
public enum TutorialSelection
{
    BoomerangBasics,
    BoomerangUpgrades,
    SkillAttractAndThrow,
    SkillPsychicShot,
    SKillPushAway,
    SkillDash,
    InteractWithObjects,
    KeysAndDoors
}
