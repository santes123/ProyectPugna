using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private PlayerStats player;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pointsText;
    public List<string> enemiesKilled = new List<string>();

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player.currentHealth <= 0)
        {
            print("YOU ARE DEAD");
            SceneManager.LoadScene("GameOverMenu");
        }
    }
}
