using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] List<keybind> keybinds;

    public InputConfig GetKeyBind(string keyname) {
        foreach(keybind key in keybinds) {
            if(keyname == key.name) {
                return key.inputKeybind;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct keybind
{
    public string name;
    public InputConfig inputKeybind;
}
