using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaAim : BossMechanic
{
    Vector3 targetPosition;
    public Transform hitPosition;

    public float aimDuration = 2f;
    float currentAimDuration;
    Vector3 lastPosition;
    Vector3 currentHitPosition;

    public void OnEnable() {
        lastPosition = transform.position + transform.forward;
        currentHitPosition = lastPosition;
    }

    public override void ExecuteMechanic() {
        mechanicDone = false;
        currentAimDuration = 0;
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        mechanicDone = false;
        targetPosition = hitPosition.position;
        while(currentAimDuration < aimDuration) {
            currentAimDuration += (Time.deltaTime / aimDuration);
            currentHitPosition = Vector3.Lerp(lastPosition, targetPosition, currentAimDuration);
            transform.LookAt(currentHitPosition);
            yield return null;
        }
        lastPosition = currentHitPosition;
        mechanicDone = true;
    }

    public override void Reset() {
    //    transform.rotation = startRotation;
    }
    
}
