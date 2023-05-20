using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour
{
    InteractableState estado;
    public void InteraccionConBoton() {
        switch(estado) {
            case InteractableState.Locked:
            Debug.Log("Estado es bloqueado. desbloquea");
            estado = InteractableState.Unlocked;
            break;
            case InteractableState.Unlocked:
            Debug.Log("Estado es desbloqueado. Abre");
            estado = InteractableState.Open;
            break;
            case InteractableState.Open:
            Debug.Log("Estado es abierta. cierra");
            estado = InteractableState.Closed;
            break;
            case InteractableState.Closed:
            Debug.Log("Estado es cerrado. abre");
            estado = InteractableState.Open;
            break;
        }
    }
}
