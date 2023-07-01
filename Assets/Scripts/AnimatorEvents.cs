using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public AudioSource step1, step2;
    public AudioSource gotHit;
    public AudioSource dead;

    bool leftStep;
    float stepSoundInternalCooldown = 0.2f;
    float lastStep; 
    private void Start() {
        lastStep = Time.time;
    }
    void FootR() {
        PlayStep();
    }

    void FootL() {
        PlayStep();
    }

    void PlayStep() {
        if(step1 != null && step2 != null) {
            if(lastStep + stepSoundInternalCooldown < Time.time) {
                Debug.Log("Step");
                if(leftStep) {
                    step1.Play();
                } else {
                    step2.Play();
                }
                lastStep = Time.time;
                leftStep = !leftStep;
            }
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
