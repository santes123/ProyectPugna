using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AreaOfEffect))]
public class DamageDealerExample : MonoBehaviour, IDamager
{
    public Damage damage;

    //simple Attack
    [ContextMenu("Attack")]
    public void Attack() {
        //first we get the targets.
        List<IDamageable> damageablesFound = GetTargets();

        //then we apply the damage on all the targets found.
        foreach(IDamageable damageable in damageablesFound) {
            DoDamage(damageable, damage);
        }
    }

    public List<IDamageable> GetTargets() {
        AreaOfEffect aoe = GetComponent<AreaOfEffect>();
        return aoe.getTargets<IDamageable>();
    }

    public void DoDamage(IDamageable target, Damage damage) {
        target.ReceiveDamage(damage);
    }
}
