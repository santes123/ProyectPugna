using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(GunController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    Camera viewCamera;
    CharacterController player;
    GunController gunController;
    public InputConfig move;
    public InputConfig look;

    void Awake() 
    {
        player = GetComponent<CharacterController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    void Update()
    {
        Move(move.GetVector3());
        Point(look.GetVector2());

        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }

    void Move(Vector3 movement)
    {
        player.SimpleMove(movement * speed);
    }

    void Point(Vector2 mousePosition)
    {
        Ray ray = viewCamera.ScreenPointToRay(new Vector3(mousePosition.x,mousePosition.y,0));
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
            LookAt(point);
        }
    }
    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

}