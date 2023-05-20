using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    public UnitType posibleInteractors;

    public void Interact(Interaction interaction) {
        
        if(posibleInteractors.Equals(interaction.source)) {
            //Si el interactuador es el adecuado para interactuar con este objeto, entonces
            //ejecutamos la accion de interaccion.
            IEfector efector = GetComponent<IEfector>();
            if(efector != null) {
                efector.ExecuteEffect();
            }
        }
    }
}
