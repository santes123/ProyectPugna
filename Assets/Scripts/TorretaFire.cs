using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaFire : BossMechanic
{

    public GameObject projectile;
    public Transform projectileSpawnPosition;
    public Transform hitPosition;
    public float speed = 500f;
    

    public override void ExecuteMechanic() {
        StartCoroutine(Execution());
    }

    public override IEnumerator Execution() {
        GetComponent<AudioSource>().Play();
        GameObject projectileGO = Instantiate(projectile, projectileSpawnPosition.position, Quaternion.identity) as GameObject;
        projectileGO.transform.LookAt(hitPosition); //Sets the projectiles rotation to look at the point clicked
        projectileGO.GetComponent<Rigidbody>().AddForce(projectileGO.transform.forward * speed); //Set the speed of the projectile by applying force to the rigidbody
        projectileGO.GetComponent<MissileExplode>().bossMechanic = GetComponentInParent<BossMechanicHandler2>();
        yield return null;
    }

    public override void Reset() {
        
    }
}
