using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObtainableItem : MonoBehaviour
{
    public UnityEvent onItemObtained;
    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player")) {
            //lanzamos el sonido
            GetComponentInParent<AudioSource>().Play();
            ShowMessage();
            GameData.AddValue(name,"1");
            ItemObtained();
        }
    }

    void ShowMessage() {
        FindObjectOfType<ShowMessageToPlayerText>().SetText("Item Obtenido", "Llave obtenida " + name, Color.white);
    }

    public void ItemObtained() {
        if(onItemObtained != null) {
            onItemObtained.Invoke();
        }
        Destroy(gameObject);
    }
}
