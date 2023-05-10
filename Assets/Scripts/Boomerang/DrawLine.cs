using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isDrawing = false;
    public Transform pointer;
    public FollowLine followScript;
    public BoomerangController BoomerangScript;

    private Vector3 startPoint;
    public float maxLineLength;
    float manaCost = 20f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (BoomerangScript.onHand)
        {
            if (Input.GetMouseButtonDown(1))
            {
                lineRenderer.positionCount = 0;
                startPoint = transform.position;
                isDrawing = true;
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, GetMouseWorldPosition());
            }

            if (isDrawing)
            {
                if (GetLineLength(lineRenderer) <= maxLineLength) // comprobar si la línea está dentro del límite
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, GetMouseWorldPosition());
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                //gastamos mana al lanzar el efecto especial del boomerang
                PlayerStats player = GameObject.Find("Player").GetComponent<PlayerStats>();
                if (player.currentMana > 0)
                {
                    isDrawing = false;
                    lineRenderer.enabled = false;
                    followScript.UpdateWayPoints();
                    BoomerangScript.onHand = false;
                    transform.SetParent(null);
                    BoomerangScript.specialThrow = true;
                    BoomerangScript.rotation = true;
                    player.UseSkill(manaCost);
                    //BoomerangScript.isFlying = true;                
                }
                else
                {
                    lineRenderer.enabled = false;
                    followScript.ClearWayPoints();
                    Debug.Log("NO TIENES SUFICIENTE MANA");
                }
                
            }
        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return hit.point + new Vector3(0f, 1f, 0f); // adjust height of point to be above terrain
        }
        else
        {
            Plane plane = new Plane(transform.up, transform.position);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
    float GetLineLength(LineRenderer line)
    {
        float length = 0;
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);

        for (int i = 0; i < positions.Length - 1; i++)
        {
            length += Vector3.Distance(positions[i], positions[i + 1]);
        }

        return length;
    }

}
