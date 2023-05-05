using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemaningAlive;
    float nextSpawnTime;

    void Start()
    {
        NextWave();
    }
    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn --;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            Enemy spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
            spawnedEnemy.name = "Enemy" + enemiesRemainingToSpawn;
        }
        
    }
    void OnEnemyDeath()
    {
        enemiesRemaningAlive --;

        if (enemiesRemaningAlive == 0)
        {
            NextWave();
        }
    }
    void NextWave()
    {
        currentWaveNumber++;
        print("wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemaningAlive = enemiesRemainingToSpawn;
        }
    }


    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
