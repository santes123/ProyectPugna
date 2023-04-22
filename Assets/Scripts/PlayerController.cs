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
    private InputConfig move;
    private InputConfig look;
    private InputConfig fire;
    private InputConfig switchGun;
    private InputConfig switchGun2;

    void Awake() 
    {
        player = GetComponent<CharacterController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    private void Start()
    {
        move = FindObjectOfType<InputManager>().GetKeyBind("move");
        look = FindObjectOfType<InputManager>().GetKeyBind("look");
        fire = FindObjectOfType<InputManager>().GetKeyBind("fire");
        switchGun = FindObjectOfType<InputManager>().GetKeyBind("switchGun");
        switchGun2 = FindObjectOfType<InputManager>().GetKeyBind("switchGun2");
    }

    void Update()
    {
        Move(move.GetVector3());
        Point(look.GetVector2());
        Shoot(fire.GetFloat());
        GunSwitcher(switchGun.GetFloat(), switchGun2.GetFloat());
    }

    void GunSwitcher(float switchGun, float switchGun2)
    {
        if (switchGun == 1)
        {
            gunController.SwitchGun(0);
        }else if (switchGun2 == 1)
        {
            gunController.SwitchGun(1);
        }
    }

    void Shoot(float fire)
    {
        if (fire == 1)
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