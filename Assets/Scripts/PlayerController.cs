using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    CharacterController player;
    public InputConfig move;
    public InputConfig jump;

    void Awake() 
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move(move.GetVector3());
    }

    void Move(Vector3 movement)
    {
        player.SimpleMove(movement * speed);
    }
}