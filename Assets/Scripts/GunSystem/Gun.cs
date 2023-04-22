using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    //Not in use yet
    /*
    #region Fire mode and type of guns enums
    public enum FireMode { Auto, Burst, Single };
    public enum TypeOfGun { Pistol, Shotgun };
    
    public FireMode fireMode;
    public TypeOfGun typeOfGun;
    #endregion*/

    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;


    float nextShotTime;

    public void Shoot(string gunType)
    {

        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Pistol();
        }
    }

    private void Pistol()
    {
        Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
        newProjectile.SetSpeed(muzzleVelocity);
    }

    //Not in use yet
    /*
    private void Shootgun()
    {
        for (int i = 0; i < 20; i += 5)
        {
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation * Quaternion.Euler(0, i, 0)) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
            //newProjectile.damage = 5;
        }
    }*/
}