using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargeBar : MonoBehaviour
{

    public Image chargeBar;
    public TextMeshProUGUI textBar;
    private float currentBarCharge;
    private float maxCharge;
    private PsychicPunchController psychicPusnch;
    //private BoomerangController boomerang;
    private UseBoomerang playerBoomerang;
    private UseAttractThrowSkill attractThrowSkill;
    public GameObject target = null;

    private void Start()
    {
        //luego hacer una funcion para cambiar los valores y poder usarlo con el boomerang o con lo que quiera
        //psychicPusnch = GameObject.Find("Player").GetComponent<PsychicPunchController>();
        psychicPusnch = FindObjectOfType<PsychicPunchController>();
        //boomerang = GameObject.Find("Boomer").GetComponent<BoomerangController>();
        //playerBoomerang = GameObject.Find("Player").GetComponent<UseBoomerang>();
        playerBoomerang = FindObjectOfType<UseBoomerang>();
        //attractThrowSkill = GameObject.Find("Player").GetComponent<UseAttractThrowSkill>();
        attractThrowSkill = FindObjectOfType<UseAttractThrowSkill>();
        /*maxCharge = psychicPusnch.maxDamage;
        currentBarCharge = psychicPusnch.currentDamage;
        textBar.text = psychicPusnch.currentDamage.ToString();*/
    }
    void Update()
    {
        if (target != null)
        {
            if (target.GetComponent<PsychicPunchController>())
            {
                maxCharge = psychicPusnch.maxDamage;
                currentBarCharge = psychicPusnch.currentDamage;
                //con 1 decimal
                string text = EliminateDecimalsOfAFloat(currentBarCharge);
                textBar.text = text;
                //sin decimales (mas fluido)
                //textBar.text = Mathf.Floor(currentBarCharge).ToString();
                chargeBar.fillAmount = currentBarCharge / maxCharge;
            }
            else if(target.GetComponent<BoomerangController>())
            {
                maxCharge = playerBoomerang.maxDistance;  //distancia real
                //maxCharge = boomerang.maxDistance - boomerang.minDistanceToLaunch;  //distancia ficticia, pero realista en la barra
                currentBarCharge = playerBoomerang.distanceToEnd;
                //con 1 decimal
                string text = EliminateDecimalsOfAFloat(currentBarCharge);
                textBar.text = text;
                //sin decimales (mas fluido)
                //textBar.text = Mathf.Floor(currentBarCharge).ToString();
                chargeBar.fillAmount = currentBarCharge / maxCharge;
            }//LANZAMIENTO OBJETO
            else if (target.GetComponent<SpecialObject>() && target.GetComponent<SpecialObject>().onHand)
            {
                maxCharge = attractThrowSkill.fuerzaMaxima;  //distancia real
                //maxCharge = boomerang.maxDistance - boomerang.minDistanceToLaunch;  //distancia ficticia, pero realista en la barra
                currentBarCharge = attractThrowSkill.fuerzaLanzamiento;
                Debug.Log("currentCharge = " + currentBarCharge);
                //con 1 decimal
                string text = EliminateDecimalsOfAFloat(currentBarCharge);
                textBar.text = text;
                //sin decimales (mas fluido)
                //textBar.text = Mathf.Floor(currentBarCharge).ToString();
                chargeBar.fillAmount = currentBarCharge / maxCharge;
            }//ATRAER OBJETO (meter algun parametro mas para filtrar el if)
            else if(target.GetComponent<SpecialObject>())
            {
                maxCharge = attractThrowSkill.maxDistanceFromTargetToPlayer;  //distancia real
                //maxCharge = boomerang.maxDistance - boomerang.minDistanceToLaunch;  //distancia ficticia, pero realista en la barra
                currentBarCharge = attractThrowSkill.currentDistanceFromTargetToPlayer;
                Debug.Log("currentCharge = " + currentBarCharge);
                //con 1 decimal
                string text = EliminateDecimalsOfAFloat(currentBarCharge);
                textBar.text = text;
                //sin decimales (mas fluido)
                //textBar.text = Mathf.Floor(currentBarCharge).ToString();
                chargeBar.fillAmount = currentBarCharge / maxCharge;
            }
            else
            {
                Debug.Log("NO HAY TARGET");
            }
            /*Debug.Log("bar amount = " + chargeBar.fillAmount);
            Debug.Log("current charge = " + currentBarCharge);
            Debug.Log("max charge = " + maxCharge);*/
        }


    }

    public void ChangeObjetive(float _maxCharge)
    {
        maxCharge = _maxCharge;
    }
    public void ResetFilled(float _fillAmount)
    {
        chargeBar.fillAmount = _fillAmount;
    }
    private string EliminateDecimalsOfAFloat(float numberWithDecimals )
    {
        int parteEntera = Mathf.FloorToInt(numberWithDecimals);
        float parteDecimal = numberWithDecimals - parteEntera;
        int primerDecimal = (int)(parteDecimal * 10f);
        float primerDecimalFloat = primerDecimal / 10f;
        string resultado = (parteEntera + primerDecimalFloat).ToString();
        return resultado;
    }
}
