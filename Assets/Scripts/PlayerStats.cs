using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : LivingEntity
{
    PlayerController controller;
    protected override void Start()
    {
        controller = GetComponent<PlayerController>();
        base.Start();
    }

    void Update()
    {
        
    }
}
