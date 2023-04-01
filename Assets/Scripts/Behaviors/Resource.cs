using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int maxPool;
    int currentAmount;

    private void Start() {
        currentAmount = maxPool;
    }

    public void ModifyResourcePool(int amount) {
        maxPool += amount;
    }
    public void ModifyResource(int amount) {
        currentAmount += amount;
    }
}
