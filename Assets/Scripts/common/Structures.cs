using UnityEngine;
[System.Serializable]
public struct Damage
{
    public UnitType source;
    public int amount;
    public TargetType targetType;
    public Vector3 point;
    public Vector3 forceImpulse;
}

public enum TargetType
{
    Single, 
    Area
}
public enum InteractableState
{
    Locked, 
    Unlocked, 
    Open, 
    Closed
}

[System.Serializable]
public struct Interaction
{
    public UnitType source;
}
public enum UnitType
{
    Player,
    Enemy,
    Object
}