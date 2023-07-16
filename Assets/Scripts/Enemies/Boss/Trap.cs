using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : BossMechanic, IDamager
{

    public float timeToExplode;
    float timeRemaining;
    bool countdownStarted = false;
    public Damage damage;
    public GameObject explodeFX;

    public override void ExecuteMechanic() {
        mechanicDone = false;
        countdownStarted = true;
        timeRemaining = timeToExplode;
    }

    void Update() {
        if(countdownStarted) {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0) {
                Attack();
            }
        }
    }

    public override IEnumerator Execution() {
        mechanicDone = false;
        yield return null;
        mechanicDone = true;
    }

    public override void Reset() {
        throw new System.NotImplementedException();
    }

    public void Attack() {
        List<IDamageable> damageablesFound = GetTargets();

        foreach(IDamageable damageable in damageablesFound) {
            DoDamage(damageable, damage);
        }
        Instantiate(explodeFX,transform.position, Quaternion.identity);
        Debug.Log("Exploded");
        Destroy(gameObject);
    }
    public List<IDamageable> GetTargets() {
        AreaOfEffect aoe = GetComponent<AreaOfEffect>();
        return aoe.GetTargets<IDamageable>();
    }
    public void DoDamage(IDamageable target, Damage damage) {
        target.ReceiveDamage(damage);
    }
    public void OnTriggerEnter(Collider other) {
        if(GetComponent<SpecialObject>().estaSiendoAtraido || GetComponent<SpecialObject>().haSidoLanzado) {
            return;
        }
        if(other.gameObject.CompareTag("Player")) {
            Attack();
        }
    }
}
