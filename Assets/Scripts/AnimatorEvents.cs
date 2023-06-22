using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    void FootR() {
    
    }

    void FootL() {
    
    }

    void Hit() {
        GetComponentInParent<IDamager>().Attack();
    }
}
