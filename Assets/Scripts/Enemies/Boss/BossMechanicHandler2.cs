using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanicHandler2 : BossMechanic
{
    //TORRETA
    public BossMechanic[] bossMechanics;
    public Transform hitPosition;
    public Light light;
    [ContextMenu("Ejecutar")]
    public override void ExecuteMechanic() {
        hitPosition.SetParent(null);
        if(isDone()) {
            StartCoroutine(Execution());
            mechanicDone = false;
        }
    }

    public override IEnumerator Execution() {
        mechanicDone = false;
        Debug.Log("Start executing the mechanic");
        //Select hit Position
        hitPosition.position = target.transform.position;
        light.enabled = true;
        yield return new WaitForSeconds(1f);

        //Spawn trap.
        bossMechanics[0].ExecuteMechanic();
        yield return new WaitUntil(bossMechanics[0].isDone);

        //fireMissile
        bossMechanics[1].ExecuteMechanic();
        Debug.Log("End executing the mechanic");
        yield return null;
    
    }

    public void MissileExploded() {
        mechanicDone = true;
        light.enabled = false;
    }

    public override void Reset() {
    }
}
