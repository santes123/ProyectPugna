using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaAim : BossMechanic
{
    Quaternion startRotation;
    public float accuracy = 5f;
    public float rotationSpeed = 180f;
    Vector3 targetPosition;

    public void OnEnable() {
        startRotation = transform.rotation;
    }

    public override void ExecuteMechanic() {
        sw = false;
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        sw = false;
        targetPosition = target.transform.position;
        SetSpeedDirection();
        while(!ArrivedToPosition()) {
            Rotate();
            yield return null;
        }
        yield return null;
        sw = true;
    }

    void SetSpeedDirection() {
        float angle = Vector3.SignedAngle(transform.forward, targetPosition - transform.position, Vector3.up);
        if(angle > 0) {
            rotationSpeed = Mathf.Abs( rotationSpeed ) ;
        } else {
            rotationSpeed = Mathf.Abs( rotationSpeed ) * -1f;
        }
    }

    public bool ArrivedToPosition() {
        float angle = Vector3.Angle(transform.forward, targetPosition - transform.position);
        return Mathf.Abs(angle) <= accuracy;
    }
    void Rotate() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public override void Reset() {
        transform.rotation = startRotation;
    }
}
