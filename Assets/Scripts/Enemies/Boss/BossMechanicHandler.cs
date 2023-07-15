using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanicHandler : BossMechanic
{
    public BossMechanic[] bossMechanics;
    public Transform hitPosition;
    public Light light;

    [ContextMenu("Ejecutar")]
    public override void ExecuteMechanic() {
        hitPosition.SetParent(null);
        mechanicDone = false;
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        Debug.Log("Start executing the mechanic");
        //Spawn trap.
        hitPosition.position = target.transform.position;
        light.enabled = true;

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
        yield return new WaitForSeconds(1f);
        light.enabled = false;
        //ResetPositions
        Reset();
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(bossMechanics[1].isDone);
        yield return new WaitUntil(bossMechanics[2].isDone);
        mechanicDone = true;
    }

    public override void Reset() {
        bossMechanics[1].Reset();
        bossMechanics[2].Reset();
    }
}
