using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObtainableItem : MonoBehaviour
{
    public UnityEvent onItemObtained;
    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player")) {
            GameData.AddValue(name,"1");
            ItemObtained();
        }
    }

    public void ItemObtained() {
        if(onItemObtained != null) {
            onItemObtained.Invoke();
        }
        Destroy(gameObject);
    }
}
