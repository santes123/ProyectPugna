using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardAreaOfEffect : AreaOfEffect
{
    public Vector3 offsetOrigin;
    public override List<T> GetTargets<T>() {
        List<T> targetsFound = new List<T>();
        RaycastHit[] hits = Physics.RaycastAll(transform.position + offsetOrigin, transform.forward, radius, layerMask, (shouldHitTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore));
        //Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask, (shouldHitTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore));
        foreach(RaycastHit hit in hits) {
            T comp = hit.collider.GetComponent<T>();
            if(comp != null) {
                targetsFound.Add(comp);
            }
        }
        return targetsFound;
    }
}
