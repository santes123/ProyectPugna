using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMoveComponent : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    CharacterController cc;
    BaseInput input;

    void Awake() {
        cc = GetComponent<CharacterController>();
        input = new BaseInput();
        input.Player.Enable();
    }

    void Update(){
        Vector3 displacement = new Vector3(input.Player.Move.ReadValue<Vector2>().x, 0, input.Player.Move.ReadValue<Vector2>().y);
        displacement = Vector3.ClampMagnitude(displacement, 1);
        cc.SimpleMove(displacement * speed);
    }
}
