using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerPrefab, transform.position + new Vector3( 2.5f, 0f, 2.5f ), Quaternion.identity);
    }

   
}
