using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Puerta : MonoBehaviour
{
    public InteractableState estado;
    public bool cerrada = true;
    Animator animator;
    public bool doubleLock = false;
    public string nombreDeRequerimiento;
    private void Start() {
        animator = GetComponent<Animator>();
    }
    
    public void InteraccionConPuerta() {
        switch(estado) {
            case InteractableState.Locked:
            Debug.Log("Estado es bloqueado.");
            FindObjectOfType<ShowMessageToPlayerText>().SetText(""+name, "Necesitas la "+ nombreDeRequerimiento + " para abrir esta puerta.", Color.green);
            //estado = InteractableState.Unlocked;
            break;
            case InteractableState.Unlocked:
            Debug.Log("Estado es desbloqueado.");
            estado = InteractableState.Open;
            AccionarPuerta();
            break;
            case InteractableState.Open:
            Debug.Log("Estado es abierta. Cierra");
            estado = InteractableState.Closed;
            AccionarPuerta();
            break;
            case InteractableState.Closed:
            Debug.Log("Estado es cerrado. Abre");
            estado = InteractableState.Open;
            AccionarPuerta();
            break;
        }

    }
    void AccionarPuerta() {
        if(cerrada) {
            animator.SetTrigger("Abrir");
        } else {
            animator.SetTrigger("Cerrar");
        }
        cerrada = !cerrada;
    }

    public void UnlockPuerta() {
        if(!doubleLock) {
            estado = InteractableState.Unlocked;
        }
        
        if(doubleLock) {
            doubleLock = false;
        }
    }

    public void CerrarPuertaYLock() {
        InteraccionConPuerta();
        estado = InteractableState.Locked;
    }
}