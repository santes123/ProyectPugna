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
        door.CerrarPuertaYLock();
        BossTrigger.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().enabled = false;
        bossCamera.gameObject.SetActive(true);
        bossCamera.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(10f);
        bossCamera.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().enabled = true;

        //Combate
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
