using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReceiverExample : MonoBehaviour, IInteractable
{
    //Codigo ejemplo:
    //Este codigo se puede usar en un objeto con el que se esta interactuando.
    //La idea es que si es el jugador el que va a interactuar, presione la tecla de "interactuar"
    //y se comunique con la interfaz de esta manera.
    //
    // GetComponent<IInteractable>.Interact(new Interaction(UnitType.player));
    //

    public void Interact(Interaction interaction) {
        //Este metodo recibira la informacion de la interaccion y hara lo necesario segun esta informacion.
        UnitType unitType = interaction.source;
        switch(unitType) {
            case UnitType.Player:
                Debug.Log("El player interactuo con: "+ gameObject.name );
            break;
            case UnitType.Enemy:
                Debug.Log("Un enemigo interactuo con: " + gameObject.name);
            break;
            case UnitType.Object:
                Debug.Log("Un objeto interactuo con: " + gameObject.name);
            break;
        }
    }
}