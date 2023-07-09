using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIControllerChargesBoomerang : SkillUIController
{
    public UpgradeName upgradeName;
    public Text chargesText;
    BoomerangUpgradeController upgrades;
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
        
    }
    public void UpdateChargesText()
    {
        currentCharges = upgrades.GetCurrentCharges(upgradeName);
        maxCharges = upgrades.GetMaxCharges(upgradeName);
        chargesText.text = currentCharges + "/ " + maxCharges;
    }
    public void GetBoomerangUpgrade()
    {
        upgrades = FindObjectOfType<BoomerangUpgradeController>();
        currentCharges = upgrades.GetCurrentCharges(upgradeName);
        maxCharges = upgrades.GetMaxCharges(upgradeName);
        upgrades.OnCurrentChargesUpdate += UpdateChargesText;
        upgrades.OnSelected += SelectSkill;
        upgrades.OnDeselected += DeselectSkill;
    }

    private void DeselectSkill(UpgradeName uN)
    {
        if (upgradeName == uN)
        {
            Deselect();
        }
    }

    private void SelectSkill(UpgradeName uN)
    {
        if (upgradeName == uN)
        {
            Select();
        }
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
                //maxCooldown = skill.container.coldown;
                GetBoomerangUpgrade();
                UpdateChargesText();
                sw = false;
            }

        }
    }
}
