using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseTrap : BossMechanic
{
    public GameObject visuals;
    public GameObject trapToRelease;
    public override void ExecuteMechanic() {
        mechanicDone = false;
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        trapToRelease = GetComponent<SpawnTrap>().currentTrap;
        trapToRelease.transform.parent = null;
        Rigidbody rb = trapToRelease.GetComponent<Rigidbody>();
        rb.useGravity = true;
        BossMechanic t = trapToRelease.GetComponent<BossMechanic>();
        t.ExecuteMechanic();
        yield return new WaitForSeconds(2f);
        mechanicDone = true;
    }

    public override void Reset() {
        
    }
}
