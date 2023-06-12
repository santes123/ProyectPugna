using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBoomerangParticles : MonoBehaviour
{
    private BoomerangController boomerangController;
    public GameObject particlesPrefab;
    private ParticleSystem particlesSystem;
    private GameObject particlesObject;

    //prefabs explosion y freeze effect del boomerang
    public GameObject particlesFreezePrefab;
    public GameObject particlesExplosionPrefab;

    //prefabs mejora activada en boomerang
    public GameObject particlesFreezeActivatedPrefab;
    public GameObject particlesExplosionActivatedPrefab;
    void Start()
    {
        boomerangController = GetComponent<BoomerangController>();
        particlesObject = Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        particlesSystem = particlesObject.GetComponent<ParticleSystem>();
        particlesSystem.Stop(); // Detiene las partículas inicialmente
    }


    //revisamos si el booleando esta true o false para reproducir las particulas
    void Update()
    {
        particlesObject.transform.position = transform.position;
        if (boomerangController.onColdown)
        {
            particlesSystem.Play(); // Activa las partículas
        }
        else
        {
            particlesSystem.Stop(); // Detiene las partículas
        }
    }
    public void InstantiateFreezeParticle()
    {
        Instantiate(particlesFreezePrefab, transform.position, Quaternion.identity);
    }
    public void InstantiateExplosionParticle()
    {
        Instantiate(particlesExplosionPrefab, transform.position, Quaternion.identity);
    }
    public GameObject InstantiateFreezeEffect()
    {
         GameObject GO = Instantiate(particlesFreezeActivatedPrefab, transform.position, Quaternion.identity);
        return GO;
    }
    public GameObject InstantiateExplosionEffect()
    {
        GameObject GO = Instantiate(particlesExplosionActivatedPrefab, transform.position, Quaternion.identity);
        return GO;
    }
}
