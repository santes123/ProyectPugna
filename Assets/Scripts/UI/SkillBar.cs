using System;
using UnityEngine;
using UnityEngine.UI;

// Estructura para almacenar los datos de cada habilidad
[System.Serializable]
public class Habilidad
{
    public Sprite imagen;
    public KeyCode tecla;
    //public float cooldown;
    public SkillParent container;
}
public class SkillBar : MonoBehaviour
{


    public Habilidad[] habilidades; // Arreglo de habilidades

    // Prefab del cuadrado de habilidad
    public GameObject cuadradoPrefab;
    public GameObject barPrefab;

    // Referencia al contenedor de las habilidades
    public Transform contenedor;
    private Text [] coldowns;
    private Image[] coldownImages;
    private Image[] imageSkillSelector;
    private Image[] hideSkillImage;
    private PlayerStats player;

    //booleanos para los coldowns
    bool coldown1 = false;
    bool coldown2 = false;
    bool coldown3 = false;
    //bool coldown4 = false;

    float remainingColdownTime = 0f;
    float expectedBoomerangColdownTime;
    //public GameObject skillBar;
    private BoomerangUpgradeController upgradeBoomerang;
    private DashController dashController;
    //text que controla las cargas del dash
    private Text dashChargesCounter;
    private Text AreaDamageCounter;
    private Text FreezeCounter;
    private void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player = FindObjectOfType<PlayerStats>();
        /*habilidades[0].container = FindObjectOfType<BoomerangController>().gameObject;
        habilidades[1].container = FindObjectOfType<UseAttractThrowSkill>().gameObject;
        habilidades[2].container = FindObjectOfType<PsychicPunchController>().gameObject;
        habilidades[3].container = FindObjectOfType<DashController>().gameObject;*/
        contenedor = GameObject.FindGameObjectWithTag("SkillBar").transform;
        upgradeBoomerang = FindObjectOfType<BoomerangUpgradeController>();
        dashController = FindObjectOfType<DashController>();

    }
    private void Update()
    {
       
        //controlar las cargas del dash y upgrades del boomerang
        UpdateSkillsChargesUI();
    }
    private void FixedUpdate()
    {

    }
    //metodos de asignacion y update de variables
    private void UpdateSkillsChargesUI()
    {
        /*if (dashController.currentCharges >= 0)
        {
            string chargesUIText = dashController.currentCharges + "/" + dashController.maxCharges;
            dashChargesCounter.text = chargesUIText;
        }
        if (upgradeBoomerang.currentChargesAreaDamage >= 0)
        {
            string chargesUIText = upgradeBoomerang.currentChargesAreaDamage + "/" + upgradeBoomerang.maxChargesAreaDamage;
            AreaDamageCounter.text = chargesUIText;
        }
        if (upgradeBoomerang.currentChargesFreeze >= 0)
        {
            string chargesUIText = upgradeBoomerang.currentChargesFreeze + "/" + upgradeBoomerang.maxChargesFreeze;
            FreezeCounter.text = chargesUIText;
        }*/
    }
    private void AssignChargesTexts(int i, GameObject cuadrado)
    {
        if (i == 3)
        {
            dashChargesCounter = cuadrado.GetComponentsInChildren<Text>(true)[2];
            Debug.Log("nombre del GO counter = " + dashChargesCounter.gameObject.name);
            dashChargesCounter.gameObject.SetActive(true);
        }
        if (i == 5)
        {
            AreaDamageCounter = cuadrado.GetComponentsInChildren<Text>(true)[2];
            Debug.Log("nombre del GO counter = " + AreaDamageCounter.gameObject.name);
            AreaDamageCounter.gameObject.SetActive(true);
        }
        if (i == 6)
        {
            FreezeCounter = cuadrado.GetComponentsInChildren<Text>(true)[2];
            Debug.Log("nombre del GO counter = " + FreezeCounter.gameObject.name);
            FreezeCounter.gameObject.SetActive(true);
        }
    }
  
}
