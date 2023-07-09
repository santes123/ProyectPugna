using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIController : MonoBehaviour
{
    public Image sprite_skill;
    public Image sprite_cooldown;
    public Image sprite_selector;
    public Text tecla;
    bool isSelected;
    public float maxCooldown;
    public Habilidad skill;
    public GameMode gamemode;
    public int buttonPosition;
    public Image hide_sprite;
    public Text coldown_text;
    public virtual void Start()
    {
        //maxCooldown = skill.cooldown;
        sprite_skill.sprite = skill.imagen;
        
        StartCoroutine(FindPlayer());
        AssignValuesOfKey();
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
                sw = false;
            }

        }
    }
    public virtual void Update()
    {
        UpdateSpriteColdown();
    }
    protected virtual void UpdateSpriteColdown()
    {
        /*if (skill.container != null)
        {*/
        //Debug.Log("sprite_cooldown = " + sprite_cooldown.gameObject.name);
        //Debug.Log("skill = " + skill.container);

            sprite_cooldown.fillAmount = skill.container.current_coldown / maxCooldown;
            if (skill.container.current_coldown != 0)
            {
                coldown_text.gameObject.SetActive(true);
                coldown_text.text = skill.container.current_coldown.ToString("F1");
            }
            else
            {
                coldown_text.gameObject.SetActive(false);
            }
        /*}
        else
        {
            coldown_text.gameObject.SetActive(false);
        }*/


    }
    public void Select()
    {
        sprite_selector.gameObject.SetActive(true);
    }
    public void Deselect()
    {
        sprite_selector.gameObject.SetActive(false);
    }
    public void HideSKill()
    {
        hide_sprite.gameObject.SetActive(true);
    }
    public void ShowSkill()
    {
        hide_sprite.gameObject.SetActive(false);
    }
    //getters
    public bool IsSelected()
    {
        return isSelected;
    }
    public void GameModeChange()
    {
        GameMode playerMode = FindObjectOfType<PlayerStats>().selectedMode;
        if (gamemode == playerMode)
        {
            if (skill.tecla == KeyCode.Z || skill.tecla == KeyCode.Q)
            {
                ShowSkill();
            }
            else
            {
                Select();
                ShowSkill();
            }

        }else if (gamemode == GameMode.Other)
        {
            ShowSkill();
        }
        else
        {
            Deselect();
            HideSKill();
        }
    }
    protected void AssignValuesOfKey()
    {
        string nombre = skill.tecla.ToString();
        tecla.text = nombre.Substring(nombre.Length - 1, 1);
    }
    //float coldown = habilidades[1].container.GetComponent<UseAttractThrowSkill>().coldownTime;
}
