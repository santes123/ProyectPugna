using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    public float startingMana;
    public float currentHealth;
    public float currentMana;
    protected float health;
    protected float mana;
    protected bool dead;
    GameManager gameManager;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        if (currentHealth == 0 && currentMana == 0)
        {
            Debug.Log("actualizando stats...");
            health = startingHealth;
            currentHealth = health;

            //mana = startingMana;
            //currentMana = mana;
        }
        else
        {
            health = currentHealth;
            mana = currentMana;

        }

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.hpText.text = currentHealth.ToString();
            //gameManager.manaText.text = currentMana.ToString();
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
        //verificamos si es un enemigo
        if (gameObject.GetComponent<Enemy>())
        {
            gameManager.enemiesKilled.Add(gameObject.name);
            
        }
        GameObject.Destroy(gameObject);
    }

    public void UseSkill(float manaCost)
    {
        mana -= manaCost;
        currentMana = mana;
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.manaText.text = currentMana.ToString();
        }
        if (mana <= 0)
        {
            Debug.Log("NO TIENES SUFICIENTE MANA");
        }
    }
}
