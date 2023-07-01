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
            //Debug.Log("actualizando stats...");
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

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager = FindObjectOfType<GameManager>();

        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.hpText.text = currentHealth.ToString();
            //gameManager.manaText.text = currentMana.ToString();
        }
        
    }
    public virtual void ReceiveDamage(Damage damage)
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
            //Debug.Log(gameManager);
            //Debug.Log(gameManager.enemiesKilled);
            if (gameManager.enemiesKilled == null)
            {
                gameManager.enemiesKilled = new List<string>();
            }
            gameManager.enemiesKilled.Add(gameObject.name);
            
        }
        //GameObject.Destroy(gameObject);
    }

    public virtual void UseSkill(float manaCost)
    {
        //verificar si el manacost tiene mas de un decimal
        bool moreThan1Decimal = Mathf.Floor(manaCost) != manaCost;
        if (moreThan1Decimal)
        {
            mana -= Mathf.Floor(manaCost);
        }
        else
        {
            mana -= manaCost;
        }
        //mana -= manaCost;
        currentMana = mana;
        Debug.Log("currentmanaLivingEntity = " + currentMana);
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.manaText.text = currentMana.ToString();
        }
        if (mana <= 0)
        {
            Debug.Log("NO TIENES SUFICIENTE MANA");
        }
    }
    public virtual void RegenerateMana(float manaToRegenerate)
    {
        //verificar si el manacost tiene mas de un decimal
        bool moreThan1Decimal = Mathf.Floor(manaToRegenerate) != manaToRegenerate;
        if (moreThan1Decimal)
        {
            mana += Mathf.Floor(manaToRegenerate);
        }
        else
        {
            mana += manaToRegenerate;
        }
        currentMana = mana;
        Debug.Log("currentmanaLivingEntity = " + currentMana);
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.manaText.text = currentMana.ToString();
        }
    }
    public void SetCurrentMana(float _mana)
    {
        mana = _mana;
        currentMana = mana;
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.manaText.text = currentMana.ToString();
        }
    }
    public void SetCurrentHeath(float _health)
    {
        health = _health;
        currentHealth = health;
        if (this.gameObject.GetComponent<PlayerController>())
        {
            gameManager.hpText.text = currentHealth.ToString();
        }
    }
}
