using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    private GameObject door; // Referencia al objeto de la puerta
    public bool isOn = false; // Estado inicial del interruptor

    private bool switchState = false; // Estado actual del switch
    public Renderer keyRenderer; // Referencia al componente Renderer de la llave
    public Material greenMaterial; // Material verde de la llave
    public Material redMaterial; // Material rojo de la llave

    private int keyID; // ID de la llave

    SwitchDoorIDGenerator generator;

    private void Start()
    {
        keyRenderer = GetComponentInChildren<Renderer>();
        UpdateKeyColor();
        generator = GameObject.Find("GameManager").GetComponent<SwitchDoorIDGenerator>();
        keyID = generator.GetAvailableKeyID();
        Debug.Log("keyID = " + keyID);
        Debug.Log("GO name = " + transform.gameObject.name);

        // Buscar la puerta correspondiente según el ID en el tag "Door"
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            DoorController doorController = door.GetComponent<DoorController>();
            if (doorController != null && doorController.doorID == keyID)
            {
                this.door = doorController.gameObject;
                break;
            }
        }

        // Comprobar si se encontró la puerta correspondiente
        if (door == null)
        {
            Debug.LogWarning("No se encontró la puerta correspondiente al ID de la llave.");
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador está en el área del collider del interruptor

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("cambiando estado...");
                ActivateSwitch();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpecialObject"))
        {
            // El jugador está en el área del collider del interruptor
            Debug.Log("cambiando estado...");
            ActivateSwitch();
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boomerang"))
        {
            // El jugador está en el área del collider del interruptor
            Debug.Log("cambiando estado...");
            ActivateSwitch();
        }
    }
    private void UpdateKeyColor()
    {
        // Cambiar el color de la llave según el estado del switch
        if (switchState)
        {
            keyRenderer.material = greenMaterial;
        }
        else
        {
            keyRenderer.material = redMaterial;
        }
    }
    public void ActivateSwitch()
    {
        // Cambiar el estado del switch cuando se active
        switchState = !switchState;

        // Cambiar el estado de la puerta según el estado del switch
        Debug.Log("door name = " + door.gameObject.name);
        door.GetComponent<DoorController>().SetDoorState(switchState);

        // Actualizar el color de la llave
        UpdateKeyColor();
    }
    //Interfaz para GameObject que es interactuado por otros gameObject
    public void Interact(Interaction interaction)
    {
        Debug.Log("tipo de objetivo = " + interaction.source);
        ActivateSwitch();
    }
}
