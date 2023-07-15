using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel1 : BossMechanic
{
    public BossMechanic[] BossMechanics;

    public Puerta door;
    public GameObject BossTrigger;
    public Camera bossCamera;

    [ContextMenu("Start Boss Fight")]
    public override void ExecuteMechanic() {
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        //Inicio y presentacion.
        //yield return StartCoroutine(bossIntro());

        //Combate
        bool sw = true;
        while(true) {
            if(sw) {
                LaunchMechanic(BossMechanics[0]);
                
            } else {
                LaunchMechanic(BossMechanics[1]);
            }
            sw = !sw;
            yield return new WaitForSeconds(3f);
        }      
    }

    IEnumerator bossIntro() {
        door.CerrarPuertaYLock();
        BossTrigger.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().enabled = false;
        bossCamera.gameObject.SetActive(true);
        bossCamera.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(10f);
        bossCamera.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().enabled = true;
    }

    void LaunchMechanic(BossMechanic mech) {
        if(mech.isDone()) {
            mech.ExecuteMechanic();
        }
    }

    public override void Reset() {
        
    }

}
