using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Resource))]
public class DamageReceiverExample : MonoBehaviour, IDamageable
{
    //
    //Clase ejemplo para explicar como se usaria el IDamageable.
    //
    //El objeto atacante usara un metodo como y enviara la informacion del daño dentro de la variable "damage" que es de tipo "Damage":
    // target.getComponent<IDamageable>().ReceiveDamage(damage);
    //


    void IDamageable.ReceiveDamage(Damage damage) {
        //como la "vida" es un recurso podemos usar etnonces el script de "Resource".
        Resource vida = GetComponent<Resource>();
        vida.ModifyResource(-damage.amount);
    }
}
