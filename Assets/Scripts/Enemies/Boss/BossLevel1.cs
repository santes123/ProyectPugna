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
        yield return StartCoroutine(bossIntro());
        
        StartCoroutine(Mecanica(BossMechanics[0], 10f));//Bomba
        StartCoroutine(Mecanica(BossMechanics[1], 2f));//Missil 1
        StartCoroutine(Mecanica(BossMechanics[2], 3f));//Missil 2
    }
   
    IEnumerator Mecanica(BossMechanic bossMechanic, float timeBetweenAttacks) {
        yield return new WaitForSeconds(timeBetweenAttacks);
        while(true) {
            if(LaunchMechanic(bossMechanic)) {
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            yield return null;
        }
    }

    IEnumerator bossIntro() {
        door.CerrarPuertaYLock();
        BossTrigger.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().enabled = false;
        FindObjectOfType<PlayerController>().Cinematic();
        bossCamera.gameObject.SetActive(true);
        bossCamera.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(3f);

        StartCoroutine(FindObjectOfType<FillBarController>().FillBar());
        //yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(2f);

        FindObjectOfType<ShowMessageToPlayerText>().SetText("LUZZIANO", "<INSERTE MENSAJE DE BOSS AQUI.>", Color.red);

        yield return new WaitForSeconds(5f);
        bossCamera.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().enabled = true;
    }

    bool LaunchMechanic(BossMechanic mech) {
        if(mech.isDone()) {
            mech.ExecuteMechanic();
            return true;
        }
        return false;
    }

    public override void Reset() {
        
    }

}
