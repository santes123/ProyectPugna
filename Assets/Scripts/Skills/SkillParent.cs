using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillParent : MonoBehaviour
{
    public float current_coldown;
    public float coldown;

    public abstract void Activate();
    public abstract void Disable();
}
