using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorInteractor :MonoBehaviour, IInteractor {
    public Interaction interaction;
    public AreaOfEffect areaOfEffect;

    void Update() {
        if(Input.GetKeyDown(KeyCode.F)) {
            Interact();
        }
    }

    //simple interact
    [ContextMenu("Interact")]
    public void Interact() {
        //first we get the posible interactables.
        List<IInteractable> interactablesFound = GetTargets();

        //then we interact with all interactables found.
        foreach(IInteractable interactable in interactablesFound) {
            DoInteraction(interactable, interaction);
        }
    }

    public List<IInteractable> GetTargets() {
        return areaOfEffect.GetTargets<IInteractable>();

    }


    public void DoInteraction(IInteractable target, Interaction interaction) {
        target.Interact(interaction);
    
    }
}