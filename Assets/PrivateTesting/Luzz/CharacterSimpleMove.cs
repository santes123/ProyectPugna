using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class CharacterSimpleMove : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector2 displacement = Vector2.zero;

    BaseInput input;
    CharacterController cc;
    Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        input = new BaseInput();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        displacement = Vector2.Lerp(displacement, new Vector2(input.Player.Move.ReadValue<Vector2>().x, input.Player.Move.ReadValue<Vector2>().y), 10f * Time.deltaTime);
        displacement = Vector2.ClampMagnitude(displacement, 1.0f);
        Vector3 processedDirection = new Vector3(displacement.x, 0, displacement.y);
        Vector3 processedRotation = (
            Vector3.RotateTowards(
                transform.forward,
                new Vector3(displacement.x, 0, displacement.y)
            ,10f * Time.deltaTime
            ,0.0f)
        );
        transform.rotation = Quaternion.LookRotation(processedRotation, Vector3.up);
        cc.SimpleMove(processedDirection * speed);
        animator.SetFloat("walkSpeed", processedDirection.magnitude);

        if(processedDirection.magnitude > 0.1f)
            animator.SetBool("walking", true);
        else
            animator.SetBool("walking", false);
    }
}