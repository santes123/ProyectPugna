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

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        maxHealth = player.startingHealth;
        currentHealth = player.currentHealth;
    }
    void Update()
    {
        currentHealth = player.currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
