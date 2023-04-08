using UnityEngine;
using UnityEngine.InputSystem;

public class InputConfig : MonoBehaviour
{
    BaseInput input;
    [SerializeField] InputAction action;

    void Awake()
    {
        input = new BaseInput();
        action.Enable();
    }

    public Vector2 GetVector2()
    {
        return new Vector2(action.ReadValue<Vector2>().x, action.ReadValue<Vector2>().y);
    }

    public Vector3 GetVector3()
    {
        return new Vector3(action.ReadValue<Vector2>().x, 0, action.ReadValue<Vector2>().y);
    }


    public bool GetBoolean()
    {
        return action.ReadValue<bool>();
    }
}
