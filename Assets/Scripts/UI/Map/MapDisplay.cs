using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    public GameObject mapCanvas;
    public Camera mapCamera;
    public CameraController mainCamera;
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
        mainCamera = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ActiveDisableMapFromMenu();
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
    public bool GetIsMapVisible()
    {
        return isMapVisible;
    }
    public void SetIsMapVisible(bool value)
    {
        isMapVisible = value;
    }
    public void ActiveDisableMapFromMenu()
    {
        //SetIsMapVisible(true);
        Debug.Log("Abriendo mapa...");
        isMapVisible = !isMapVisible;
        mapCanvas.SetActive(isMapVisible);
        //playerIcon.enabled = isMapVisible;
        //activamos una camera y desactivamos la otra
        mapCamera.gameObject.SetActive(isMapVisible);
        mainCamera.gameObject.SetActive(!isMapVisible);
        gameManager.onPause = isMapVisible;
        if (isMapVisible)
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
            FindObjectOfType<Crosshairs>().enabled = false;
            FindObjectOfType<PlayerController>().enabled = false;
            FindObjectOfType<PlayerStats>().enabled = false;
            FindObjectOfType<UseBoomerang>().enabled = false;
            FindObjectOfType<DashController>().enabled = false;
            FindObjectOfType<PushAwaySkill>().enabled = false;
            FindObjectOfType<ManaRegeneration>().enabled = false;
            FindObjectOfType<BoomerangController>().enabled = false;
            FindObjectOfType<DrawLine>().enabled = false;
            FindObjectOfType<BoomerangUpgradeController>().enabled = false;
        }
        else
        {
            Cursor.visible = false;
            Time.timeScale = 1f;
            FindObjectOfType<Crosshairs>().enabled = true;
            FindObjectOfType<PlayerController>().enabled = true;
            FindObjectOfType<PlayerStats>().enabled = true;
            FindObjectOfType<UseBoomerang>().enabled = true;
            FindObjectOfType<DashController>().enabled = true;
            FindObjectOfType<PushAwaySkill>().enabled = true;
            FindObjectOfType<ManaRegeneration>().enabled = true;
            FindObjectOfType<BoomerangController>().enabled = true;
            FindObjectOfType<DrawLine>().enabled = true;
            FindObjectOfType<BoomerangUpgradeController>().enabled = true;
        }
    }
}
