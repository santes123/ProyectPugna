using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyBase[] enemy;
    public GameObject ItemDropped;
    public int enemyCount;
    public Transform itemDroppedSpawnPosition;

    private void Start() {
        enemyCount = enemy.Length;
        SubscribeToEnemyEvents();
    }

    void SubscribeToEnemyEvents() {
        foreach(EnemyBase e in enemy) {
            WatchEnemyDead(e);
        }
    }

    void WatchEnemyDead(EnemyBase enemy) {
        enemy.OnDeath += EnemyDied;
    }

    void EnemyDied() {
        enemyCount -= 1;
        if(AllEnemiesAreDead()) {
            Instantiate(ItemDropped, itemDroppedSpawnPosition.position, Quaternion.identity);
        }
    }

    bool AllEnemiesAreDead() {
        return enemyCount == 0;
    }

}
