using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialObjectsData : MonoBehaviour
{
    public string name;
    public float x;
    public float y;
    public float z;

    public SpecialObjectsData(string name, Vector3 v)
    {
        this.name = name;
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
