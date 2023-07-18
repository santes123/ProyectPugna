using System.Collections;
using UnityEngine;

public class CameraLayerChanger : MonoBehaviour
{
    public Camera mainCamera;
    public string targetLayerName = "Everything"; // Nombre de la capa de destino que deseas cambiar
    public GameObject Fade;
    public bool finished = false;
    private void Start()
    {
        StartCoroutine(ChangeLayer());
    }
    private IEnumerator ChangeLayer()
    {
        yield return new WaitForSeconds(2f);
        //int layerMask = LayerMask.GetMask("Everything");
        int everythingLayerIndex = LayerMask.NameToLayer(targetLayerName);
        Debug.Log("Index of 'Everything' layer: " + everythingLayerIndex);
        mainCamera.cullingMask = everythingLayerIndex;
        finished = true;
        GetComponent<FadeOutController>().timer = GetComponent<FadeOutController>().fadeDuration;
    }
}
