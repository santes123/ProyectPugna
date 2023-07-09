using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : BossMechanic
{
    public float timeToExplode;
    public override void ExecuteMechanic() {
        sw = false;
        Invoke("Explode", timeToExplode);    
    }

    public override IEnumerator Execution() {
        sw = false;
        yield return null;
        sw = true;
    }

    public override void Reset() {
        throw new System.NotImplementedException();
    }

    void Explode() {
        Debug.Log("Exploto");
    }

}
