using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    public bool cerrada = false;
    Animator animator;
    private void Start() {
        animator = GetComponent<Animator>();
    }
    public void AccionarPuerta() {
        
        if(cerrada) {
            animator.SetTrigger("Cerrar");
        } else {
            animator.SetTrigger("Abrir");
        }
        cerrada = !cerrada;
    }
}
