using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int maxPool = 100;
    [SerializeField] int currentAmount;

    public delegate void resourceEvent();
    public event resourceEvent OncurrentAmountChanged, OnMaxPoolChanged, OnCurrentAmountIsZero;

    private void Start() {
        currentAmount = maxPool;
    }

    public void ModifyResourcePool(int amount) {
        maxPool += amount;
        OnMaxPoolChanged?.Invoke();
    }

    public void ModifyResource(int amount) {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxPool);
        OncurrentAmountChanged?.Invoke();
        
        if(currentAmount <= 0){
            OnCurrentAmountIsZero?.Invoke();
        }

    }
}
