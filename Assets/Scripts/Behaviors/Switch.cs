using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public InteractableState estado = InteractableState.Open;

    public void InteraccionConSwitch() {
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
            Debug.Log("Estado es abierta. se cierra");
            estado = InteractableState.Closed;
            break;
            case InteractableState.Closed:
            Debug.Log("Estado es cerrado. se abre");
            estado = InteractableState.Open;
            break;
        }
    }
}
