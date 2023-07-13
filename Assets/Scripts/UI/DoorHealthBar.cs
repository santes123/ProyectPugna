using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorHealthBar : MonoBehaviour
{

    public Image healthBar;
    public TextMeshProUGUI healthText;
    private float currentHealth;
    private float maxHealth;
    private BossStatsController boss;

    private void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        boss = FindObjectOfType<BossStatsController>();
        maxHealth = boss.startingHealth;
        currentHealth = boss.currentHealth;
    }
    void Update()
    {
        if (boss != null)
        {
            healthText.text = boss.currentHealth.ToString();
            currentHealth = boss.currentHealth;
            healthBar.fillAmount = currentHealth / maxHealth;
        }

    }
}
