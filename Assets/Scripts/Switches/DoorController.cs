using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform openPosition; // Posici�n abierta de la puerta
    public Transform closedPosition; // Posici�n cerrada de la puerta
    public float openSpeed = 1f; // Velocidad de apertura de la puerta
    public GameObject door1_movable;

    public bool isOpen = false; // Estado actual de la puerta
    private Vector3 targetPosition; // Posici�n objetivo de la puerta
    [HideInInspector]
    public int doorID;
    SwitchDoorIDGenerator generator;

    private void Start()
    {
        //generator = GameObject.Find("GameManager").GetComponent<SwitchDoorIDGenerator>();
        generator = FindObjectOfType<SwitchDoorIDGenerator>();
        doorID = generator.GetAvailableDoorID();
        Debug.Log("DoorID = " + doorID);
        Debug.Log("GO name = " + transform.gameObject.name);
        // Asegurarse de que la puerta est� cerrada al inicio del juego
        targetPosition = closedPosition.position;
    }

    private void Update()
    {
        Debug.Log("isOpen = "+ isOpen);

        // Mover la puerta hacia la posici�n objetivo
        Vector3 currentPosition = door1_movable.transform.position;
        Vector3 newPosition = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z);
        door1_movable.transform.position = Vector3.MoveTowards(currentPosition, newPosition, openSpeed * Time.deltaTime);
        //door1_movable.transform.position = Vector3.Lerp(transform.position, newPosition, openSpeed * Time.deltaTime);
    }

    public void SetDoorState(bool state)
    {
        // Cambiar el estado de la puerta y establecer la posici�n objetivo
        isOpen = state;
        targetPosition = isOpen ? openPosition.position : closedPosition.position;
    }
}

