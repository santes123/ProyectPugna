using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image healthBar;
    private float currentHealth;
    private float maxHealth;
    private PlayerStats player;
    public Image healthBarRed;
    public float speedRed;
    private void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player = FindObjectOfType<PlayerStats>();
        maxHealth = player.startingHealth;
        currentHealth = player.currentHealth;
    }
    void Update()
    {
        currentHealth = player.currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (healthBar.fillAmount < healthBarRed.fillAmount)
        {
            healthBarRed.fillAmount += -speedRed * Time.deltaTime;
        }
        else
        {
            healthBarRed.fillAmount = healthBar.fillAmount;
        }

    }
}
