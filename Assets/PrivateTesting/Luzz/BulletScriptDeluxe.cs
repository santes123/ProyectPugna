using UnityEngine;

public class BulletScriptDeluxe : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float pushSpeed = 100;

    public void Constructor(float pushSpeed) {
        this.pushSpeed = pushSpeed;
    }

    void Awake() {
        Destroy(gameObject, 2f);
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.AddForce(transform.forward * pushSpeed, ForceMode.Impulse);
    }
}
