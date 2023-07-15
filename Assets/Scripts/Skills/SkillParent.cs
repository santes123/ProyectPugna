using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillParent : MonoBehaviour
{
    public float current_coldown;
    public float coldown;

    protected bool GetMouseButton(int value) {
        if(FindObjectOfType<ShowMessageToPlayerText>().mainObject.activeSelf) {
            return false;
        }
        return Input.GetMouseButton(value);
    }
    protected bool GetMouseButtonDown(int value) {
        if(FindObjectOfType<ShowMessageToPlayerText>().mainObject.activeSelf) {
            return false;
        }
        return Input.GetMouseButtonDown(value);
    }
    protected bool GetMouseButtonUp(int value) {
        if(FindObjectOfType<ShowMessageToPlayerText>().mainObject.activeSelf) {
            return false;
        }
        return Input.GetMouseButtonUp(value);
    }

    protected bool GetKeyDown(KeyCode kc) {
        if(FindObjectOfType<ShowMessageToPlayerText>().mainObject.activeSelf) {
            return false;
        }
        return Input.GetKeyDown(kc);
    }

    public abstract void Activate();
    public abstract void Disable();
}
