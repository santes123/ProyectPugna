using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanicHandler : BossMechanic
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
        //empezo movimiento.
        bossMechanics[1].ExecuteMechanic();
        yield return new WaitUntil(bossMechanics[1].isDone);
        bossMechanics[2].ExecuteMechanic();
        yield return new WaitUntil(bossMechanics[2].isDone);
        //termino movimiento.
        bossMechanics[3].ExecuteMechanic();
        yield return new WaitUntil(bossMechanics[3].isDone);
        //solto la trampa.
        Debug.Log("End executing the mechanic");
        yield return null;

        //ResetPositions
        Reset();
        
    }

    public override void Reset() {
        bossMechanics[1].Reset();
        bossMechanics[2].Reset();
    }
}
