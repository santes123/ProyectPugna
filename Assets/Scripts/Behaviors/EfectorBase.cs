using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EfectorBase : MonoBehaviour, IEfector
{
    public UnityEvent effectToExecute;
    public void ExecuteEffect() {
        effectToExecute.Invoke();
    }
}
