using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FollowLine : MonoBehaviour
{
    public float speed;
    public float minDistanceToWaypoint;
    public bool loop;

    private LineRenderer lineRenderer;
    public List<Vector3> waypoints = new List<Vector3>();
    private int currentIndex = 0;
    public GameObject destination;
    private bool hasCompletedLine = false;
    public BoomerangController BoomerangScript;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (hasCompletedLine)
        {
            BoomerangScript.specialThrow = false;
            // Moverse al transform de destino progresivamente
            Vector3 targetPosition = destination.transform.position;
            Vector3 currentPosition = transform.position;
            float distance = Vector3.Distance(targetPosition, currentPosition);
            if (distance < 0.4f)
            {
                hasCompletedLine = false;
                BoomerangScript.onHand = true;
                BoomerangScript.rotation = false;
                transform.SetParent(destination.transform);
            }
            else
            {
                transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
            }
        }
        else
        {
            
            if (waypoints.Count > 0)
            {

                Vector3 targetWaypoint = waypoints[currentIndex];
                float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint);

                if (distanceToWaypoint < minDistanceToWaypoint)
                {
                    print("MIN DISTANCE REACHED");
                    currentIndex++;

                    if (currentIndex >= waypoints.Count)
                    {
                        if (loop)
                        {
                            currentIndex = 0;
                            hasCompletedLine = true;
                        }
                        else
                        {
                            waypoints.Clear();
                            currentIndex = 0;
                            hasCompletedLine = true;
                        }
                    }
                }

                if (currentIndex < waypoints.Count)
                {
                    //MOVIMIENTO ANTERIOR
                    //Vector3 direction = (targetWaypoint - transform.position).normalized;
                    //transform.position += direction * speed * Time.deltaTime;
                    //MOVIMIENTO NUEVO
                    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
                }
            }
        }

    }
    public void UpdateWayPoints()
    {
        waypoints.Clear();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            waypoints.Add(lineRenderer.GetPosition(i));
        }
        currentIndex = 0;
    }
}
