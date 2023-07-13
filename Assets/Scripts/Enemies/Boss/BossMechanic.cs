using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossMechanic : MonoBehaviour
{
    public PlayerStats target;
    protected bool sw = false;
    private void Start() {
        StartCoroutine(GetTarget());    
    }
    IEnumerator GetTarget() {
        while(!target) {
            target = FindObjectOfType<PlayerStats>();
            yield return null;
        }
    }

    public bool isDone() {
        return sw;
    }

    public abstract void ExecuteMechanic();

    public abstract IEnumerator Execution();

    public abstract void Reset();
}
