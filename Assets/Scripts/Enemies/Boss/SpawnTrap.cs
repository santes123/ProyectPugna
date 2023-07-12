using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrap : BossMechanic
{
    public GameObject trapToSpawn;
    public Transform locationToSpawn;
    public GameObject currentTrap;
    public override void ExecuteMechanic() {
        sw = false;
        StartCoroutine(Execution());
        
    }

    public override IEnumerator Execution() {
        currentTrap = Instantiate(trapToSpawn, locationToSpawn);
        currentTrap.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(1f);
        sw = true;
    }

    public override void Reset() {
        throw new System.NotImplementedException();
    }
}
