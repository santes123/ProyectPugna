using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{

    public Image healthBar;
    public TextMeshProUGUI healthText;
    private float currentHealth;
    private float maxHealth;
    private Enemy enemy;

    private void Start()
    {
        //enemy = GameObject.Find("Player").GetComponent<PlayerStats>();
        enemy = GetLastParent(this.gameObject).GetComponent<Enemy>();
        maxHealth = enemy.startingHealth;
        currentHealth = enemy.currentHealth;
    }
    void Update()
    {
        healthText.text = enemy.currentHealth.ToString();
        currentHealth = enemy.currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    public GameObject GetLastParent(GameObject go)
    {
        if (go.transform.parent == null)
        {
            return go;
        }
        else
        {
            return GetLastParent(go.transform.parent.gameObject);
        }
    }
}
