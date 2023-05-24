using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoorIDGenerator : MonoBehaviour
{
    private static int[] availableKeyIDs; // Array de ID disponibles para asignar Key
    private static int[] availableDoorIDs; // Array de ID disponibles para asignar Door
    private static int lastAssignedKeyID = 0; // Último ID asignado Key
    private static int lastAssignedDoorID = 0; // Último ID asignado Door

    //public int keyID; // ID de la llave

    //private DoorController doorController; // Referencia al componente DoorController

    private void Start()
    {/*
        // Asignar un ID disponible a la llave
        keyID = GetAvailableID();

        // Buscar la puerta correspondiente según el ID en el tag "Door"
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            DoorController doorController = door.GetComponent<DoorController>();
            if (doorController != null && doorController.doorID == keyID)
            {
                this.doorController = doorController;
                break;
            }
        }

        // Comprobar si se encontró la puerta correspondiente
        if (doorController == null)
        {
            Debug.LogWarning("No se encontró la puerta correspondiente al ID de la llave.");
        }*/
    }

    public int GetAvailableKeyID()
    {
        // Comprobar si hay IDs disponibles en el array
        if (availableKeyIDs == null || availableKeyIDs.Length == 0)
        {
            // Generar un nuevo ID si no hay disponibles en el array
            lastAssignedKeyID++;
            return lastAssignedKeyID;
        }
        else
        {
            // Asignar el último ID disponible en el array y eliminarlo de la lista de disponibles
            int availableID = availableKeyIDs[availableKeyIDs.Length - 1];
            availableKeyIDs = RemoveIDFromArray(availableKeyIDs, availableID);
            return availableID;
        }
    }
    public int GetAvailableDoorID()
    {
        // Comprobar si hay IDs disponibles en el array
        if (availableDoorIDs == null || availableDoorIDs.Length == 0)
        {
            // Generar un nuevo ID si no hay disponibles en el array
            lastAssignedDoorID++;
            return lastAssignedDoorID;
        }
        else
        {
            // Asignar el último ID disponible en el array y eliminarlo de la lista de disponibles
            int availableID = availableDoorIDs[availableDoorIDs.Length - 1];
            availableDoorIDs = RemoveIDFromArray(availableDoorIDs, availableID);
            return availableID;
        }
    }
    private int[] RemoveIDFromArray(int[] array, int id)
    {
        // Crear un nuevo array sin el ID especificado
        int[] newArray = new int[array.Length - 1];
        int newArrayIndex = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != id)
            {
                newArray[newArrayIndex] = array[i];
                newArrayIndex++;
            }
        }
        return newArray;
    }
}
