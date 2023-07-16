using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    //This is currently set for a simple sphere but we could add a line or other shapes.
    public float radius;
    public LayerMask layerMask;
    public bool shouldHitTriggers;

    //the idea beind <T> is to be able to interact with many different posible objects. this could be IDamageable, or IInteractables.
    public virtual List<T> GetTargets<T>() {
        List<T> targetsFound = new List<T>();
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask, (shouldHitTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore));
        
        foreach(Collider hit in hits) {
            T comp = hit.GetComponent<T>();
            if(comp != null) {
                targetsFound.Add(comp);
            }
        }
        return targetsFound;
    }

}
