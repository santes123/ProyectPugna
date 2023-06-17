using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    public GameObject mapCanvas;
    public Camera mapCamera;
    public Image playerIcon;
    private GameObject player;
    private GameManager gameManager;

    private bool isMapVisible = false;

    private void Start()
    {
        mapCanvas.SetActive(false);
        playerIcon.enabled = false;
        player = FindObjectOfType<PlayerStats>().gameObject;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Abriendo mapa...");
            isMapVisible = !isMapVisible;
            mapCanvas.SetActive(isMapVisible);
            //playerIcon.enabled = isMapVisible;
            mapCamera.gameObject.SetActive(isMapVisible);
            gameManager.onPause = isMapVisible;
            if (isMapVisible)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        if (isMapVisible)
        {
            // Actualiza la posición del icono del jugador en el mapa
            Vector2 playerPosition = player.transform.position;
            Vector3 playerPositionOnMap = mapCamera.WorldToViewportPoint(playerPosition);
            playerIcon.rectTransform.anchorMin = playerPositionOnMap;
            playerIcon.rectTransform.anchorMax = playerPositionOnMap;
        }
    }
}
