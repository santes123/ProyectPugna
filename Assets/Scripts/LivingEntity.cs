using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    public float currentHealth;
    protected float health;
    protected bool dead;
    GameManager gameManager;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
        currentHealth = health;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.hpText.text = currentHealth.ToString();
        }
        
    }
    public void ReceiveDamage(Damage damage)
    {
        health -= damage.amount;
        currentHealth = health;
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.hpText.text = currentHealth.ToString();
        }
        if (health <= 0 && !dead)
        {
            Die();
            //añadir enemigos a la lista de muertos
            if (this.GetComponent<Enemy>())
            {
                string name = this.GetComponent<Enemy>().name;
                gameManager.enemiesKilled.Add(name);

            }
        }
    }
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }


}
