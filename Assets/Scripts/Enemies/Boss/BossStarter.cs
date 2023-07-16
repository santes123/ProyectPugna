using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStarter : MonoBehaviour
{
    BossLevel1 boss;
   

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            FindObjectOfType<BossLevel1>().ExecuteMechanic();
        }
    }
}
