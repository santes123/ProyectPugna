using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillUIControllerChargesDash : SkillUIController
{
    public Text chargesText;
    //BoomerangUpgradeController upgrades;
    DashController dashController;
    int maxCharges;
    int currentCharges;
    public override void Start()
    {
        sprite_skill.sprite = skill.imagen;

        StartCoroutine(FindPlayer());
        AssignValuesOfKey();

    }
    public override void Update()
    {
        //base.Update();
        this.UpdateSpriteColdown();
    }
    protected override void UpdateSpriteColdown()
    {
        if (skill.container != null)
        {
            //Debug.Log("current_coldown = " + skill.container.current_coldown);
            //Debug.Log("maxCooldown = " + maxCooldown);

            //sprite_cooldown.fillAmount = skill.container.current_coldown / maxCooldown;
            //Debug.Log("gameobject "+gameObject.name+" coldown = " + skill.container.current_coldown);
            if (skill.container.current_coldown != 0)
            {
                coldown_text.gameObject.SetActive(true);
                sprite_cooldown.gameObject.SetActive(true);
                coldown_text.text = skill.container.current_coldown.ToString("F1");
                sprite_cooldown.fillAmount = skill.container.current_coldown / maxCooldown;
            }
            else
            {
                coldown_text.gameObject.SetActive(false);
                sprite_cooldown.gameObject.SetActive(false);
            }
        }
        else
        {
            chargesText.gameObject.SetActive(false);
            sprite_cooldown.gameObject.SetActive(false);
        }


    }
    public void UpdateChargesText()
    {
        currentCharges = dashController.GetCurrentCharges();
        maxCharges = dashController.GetMaxCharges();
        chargesText.text = currentCharges + "/ " + maxCharges;
    }
    public void SubscriteToEvents()
    {
        dashController = FindObjectOfType<DashController>();
        currentCharges = dashController.GetCurrentCharges();
        maxCharges = dashController.GetMaxCharges();
        chargesText.text = currentCharges + "/ " + maxCharges;
        dashController.OnCurrentChargesUpdate += UpdateChargesText;
    }

    IEnumerator FindPlayer()
    {
        bool sw = true;
        while (sw)
        {
            yield return null;
            if (FindObjectOfType<PlayerStats>() != null)
            {
                FindObjectOfType<PlayerStats>().OnGameModeChanges += GameModeChange;
                skill.container = FindObjectOfType<PlayerStats>().skillList[buttonPosition];
                maxCooldown = skill.container.coldown;
                SubscriteToEvents();
                UpdateChargesText();
                sw = false;
            }

        }
    }
}
