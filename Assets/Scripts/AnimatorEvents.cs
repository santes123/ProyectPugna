using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public AudioSource steps;
    public AudioSource gotHit;
    public AudioSource dead;
    void FootR() {
        if(steps != null) {
            steps.Play();
        }
    }

    void FootL() {
        if(steps != null) {
            steps.Play();
        }
    }

    void Hit() {
        GetComponentInParent<IDamager>().Attack();
    }

    void GetHit() {
        if(gotHit != null) {
            gotHit.Play();
        }
    }

    void Death() {
        if(dead != null) {
            dead.Play();
        }
    }
}
