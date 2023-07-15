using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplode : MonoBehaviour, IDamager
{
    PolygonArsenal.PolygonProjectileScript projectile;
    public BossMechanicHandler2 bossMechanic;
    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<PolygonArsenal.PolygonProjectileScript>();
        projectile.OnExploded += Attack;
    }

    public Damage damage;

    public void Attack() {
        //first we get the targets.
        List<IDamageable> damageablesFound = GetTargets();

        //then we apply the damage on all the targets found.
        foreach(IDamageable damageable in damageablesFound) {
            DoDamage(damageable, damage);
        }
        bossMechanic.MissileExploded();

        Destroy(gameObject);
    }

    public List<IDamageable> GetTargets() {
        AreaOfEffect aoe = GetComponent<AreaOfEffect>();
        return aoe.GetTargets<IDamageable>();
    }

    public void DoDamage(IDamageable target, Damage damage) {
        target.ReceiveDamage(damage);
    }
}
