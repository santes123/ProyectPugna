using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

[Serializable]
public class GameState
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public float currentPlayerHealth;
    public float currentPlayerMana;
    public int score;
    public List<string> enemiesEliminated;
    public GameMode lastSelectedMode;
    public string lastScenePlayed;
    //public List<string> collectedItems;
}
