public struct Damage
{
    UnitType source;
    int amount;
    TargetType target;
}

public enum TargetType
{
    Single, Area
}

public struct Interaction
{
    UnitType source;
}
public enum UnitType
{
    Player,
    Enemy,
    Object
}