using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 offset;

    public  Vector3 velocity;
    public float smoothTime = 0.5f;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Camera camera;
    private Transform player;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(FindPlayer());
        //targets.Add(player.GetComponent<Transform>());
        //targets.Add(FindObjectOfType<BoomerangController>().GetComponent<Transform>());
    }
    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
    }
    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
    void Zoom()
    {
        //GetGreastestDistance();
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreastestDistance() / zoomLimiter);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newZoom, Time.deltaTime);
    }
    float GetGreastestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
    //buscamos al jugador con una corutina
    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            if (gameManager.playerInstantiated)
            {
                player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
                targets.Add(player.GetComponent<Transform>());
                targets.Add(FindObjectOfType<BoomerangController>().GetComponent<Transform>());
                //Debug.Log("player = " + player.name);
                //Debug.Log("targets count = " + targets.Count);
                if (player != null) yield return null;
            }
        }
    }
}
