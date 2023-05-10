using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public float currentPlayerHealth;
    public int score;
    public List<string> enemiesEliminated;
    //public List<string> collectedItems;
}
