using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanicHandler2 : BossMechanic
{
    public BossMechanic[] bossMechanics;

    [ContextMenu("Ejecutar")]
    public override void ExecuteMechanic() {
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        Debug.Log("Start executing the mechanic");
        //Spawn trap.
        bossMechanics[0].ExecuteMechanic();
        yield return new WaitUntil(bossMechanics[0].isDone);
        
        Debug.Log("End executing the mechanic");
        yield return null;
    }

    public override void Reset() {
        throw new System.NotImplementedException();
    }
}
