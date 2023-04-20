using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShotgunDeluxe : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] int numberOfBullets = 10;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] AnimationCurve spreadCurve;
    [SerializeField] float spreadAngle = 30f;
    [SerializeField] Animator muzzleAnim;
    [SerializeField] Animator playerAnim;

    AudioSource audioSource;
    BaseInput input;

    void Awake(){
        input = new BaseInput();
        input.Enable();
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if (input.Player.Fire.WasPressedThisFrame()){
            Fire();
            playerAnim.SetTrigger("knockback");
            audioSource.Play();
        }
    }

    public void Fire(){
        muzzleAnim.SetTrigger("fire");
        for (int i = 0; i < numberOfBullets; i++)
        {
            float t = (float)i / (numberOfBullets - 1);
            float adjust = spreadCurve.Evaluate(t) * Random.Range(-spreadAngle, spreadAngle);
            Quaternion bulletRotation = Quaternion.Euler(0, adjust, 0) * bulletSpawnPoint.rotation;
            GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
            bulletInstance.GetComponent<BulletScriptDeluxe>().Constructor(bulletSpeed * Random.Range(0.9f, 1.1f));
        }
    }
}