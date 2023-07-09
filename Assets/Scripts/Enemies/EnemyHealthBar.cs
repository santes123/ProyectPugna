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
    //private Enemy enemy;
    private EnemyBase enemy;
    public Canvas canvas;
    private void Start()
    {
        //enemy = GameObject.Find("Player").GetComponent<PlayerStats>();
        //enemy = GetLastParent(this.gameObject).GetComponent<Enemy>();
        enemy = GetLastParent(gameObject).GetComponent<EnemyBase>();
        //Debug.Log("lastparent = " + GetLastParent(this.gameObject).name);
        //Debug.Log("enemy name = " + enemy.gameObject.name);
        maxHealth = enemy.startingHealth;
        currentHealth = enemy.currentHealth;
    }
    void Update()
    {
        if (enemy != null)
        { 
            //Debug.Log("enemy = " + enemy.gameObject.name);
            healthText.text = enemy.currentHealth.ToString();
            currentHealth = enemy.currentHealth;
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
    private void LateUpdate()
    {
        if (FindObjectOfType<CameraController>())
        {
            canvas.transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
        }

    }
    /*public GameObject GetLastParent(GameObject go)
    {
        if (go.transform.parent == null)
        {
            return go;
        }
        else
        {
            return GetLastParent(go.transform.parent.gameObject);
        }
    }*/
    public GameObject GetLastParent(GameObject go)
    {
        if (go.transform.parent == null || go.transform.parent.name == "_Enemies")
        {
            return go;
        }
        else
        {
            return GetLastParent(go.transform.parent.gameObject);
        }
    }
}
