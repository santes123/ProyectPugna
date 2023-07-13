using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsController : LivingEntity, IDamageable
{
    public Material hitMaterial; // Material con la textura de brillo blanco
    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private bool isHit = false;
    protected override void Start()
    {
        base.Start();
        meshRenderer = GetComponentsInChildren<MeshRenderer>()[1];
        Debug.Log("renderer = "+ meshRenderer.gameObject.name);
        originalMaterial = meshRenderer.material;
    }

    void Update()
    {
        if (isHit)
        {
            // Cambiar el material al material de brillo blanco
            meshRenderer.material = hitMaterial;

            // Establecer una duración para el efecto de golpe
            Invoke("ResetHitEffect", 0.15f);
        }
    }
    private void ResetHitEffect()
    {
        // Restaurar el material original del enemigo
        meshRenderer.material = originalMaterial;
        isHit = false;
    }
    public override void ReceiveDamage(Damage damage)
    {
        isHit = true;
        base.ReceiveDamage(damage);
        if (currentHealth <= 0)
        {
            OnDead();
        }
    }
    private void OnDead()
    {
        Debug.Log("BOSS ASESINADO");
        Destroy(gameObject);
        //realizar mas acciones despues de asesinar al boss
    }
}
