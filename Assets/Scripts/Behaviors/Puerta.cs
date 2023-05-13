using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Puerta : MonoBehaviour
{
    public InteractableState estado;

    public void InteraccionConPuerta() {
        switch(estado) {
            case InteractableState.Locked:
            Debug.Log("Estado es bloqueado. Desbloquea");
            estado = InteractableState.Unlocked;
            break;
            case InteractableState.Unlocked:
            Debug.Log("Estado es desbloqueado. Abre");
            estado = InteractableState.Open;
            break;
            case InteractableState.Open:
            Debug.Log("Estado es abierta. Cierra");
            estado = InteractableState.Closed;
            break;
            case InteractableState.Closed:
            Debug.Log("Estado es cerrado. Abre");
            estado = InteractableState.Open;
            break;
        }

    }
}