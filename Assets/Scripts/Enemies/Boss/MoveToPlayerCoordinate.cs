using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerCoordinate : BossMechanic
{
    Vector3 startingPosition;

    public float speed = 1f;
    public float accuracy = 0.1f;
    public Coordinate coordinate;
    Vector3 targetPosition;
    public Transform hitPosition;

    private void OnEnable() {
        startingPosition = transform.position;
    }

    public override void ExecuteMechanic() {
        mechanicDone = false;
        SetTarget(hitPosition.position);
        StartCoroutine(Execution());
    }

    void SetTarget(Vector3 position) {
        targetPosition = position;
    }

    public override IEnumerator Execution() {
        mechanicDone = false;
        while(!ArrivedToPosition()) {
            Move();
            yield return null;
        }
        yield return null;
        mechanicDone = true;
    }

    public bool ArrivedToPosition() {
        bool arrived = false;
        float distanceRemaining = accuracy + 1;
        switch(coordinate) {
            case Coordinate.x:
            distanceRemaining = Mathf.Abs(transform.position.x - targetPosition.x);
            break;

            case Coordinate.y:
            distanceRemaining = Mathf.Abs(transform.position.y - targetPosition.y);
            break;

            case Coordinate.z:
            distanceRemaining = Mathf.Abs(transform.position.z - targetPosition.z);
            break;

        }
        arrived = distanceRemaining <= accuracy;
        return arrived;
    }
    void Move() {
        switch(coordinate) {
            case Coordinate.x:
            if(transform.position.x < targetPosition.x) {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            } else {
                transform.Translate(Vector3.right * -speed * Time.deltaTime);
            }
            
            break;
            case Coordinate.y:
            if(transform.position.y < targetPosition.y) {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            } else {
                transform.Translate(Vector3.up * -speed * Time.deltaTime);
            }
            break;
            case Coordinate.z:
            if(transform.position.z < targetPosition.z) {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            } else {
                transform.Translate(Vector3.forward * -speed * Time.deltaTime);
            }
            break;
        }
    }

    public override void Reset() {
        SetTarget(startingPosition);
        StartCoroutine(Execution());
    }
}
public enum Coordinate
{
    x, y, z
}