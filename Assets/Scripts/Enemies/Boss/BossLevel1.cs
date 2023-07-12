using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel1 : BossMechanic
{
    public BossMechanic[] BossMechanics;

    [ContextMenu("Start Boss Fight")]
    public override void ExecuteMechanic() {
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        bool sw = true;
        while(true) {
            if(sw) {
                BossMechanics[0].ExecuteMechanic();
            } else {
                BossMechanics[1].ExecuteMechanic();
            }
            sw = !sw;
            yield return new WaitForSeconds(3f);
        }
        yield return null;        
    }



    public override void Reset() {
        
    }

}
