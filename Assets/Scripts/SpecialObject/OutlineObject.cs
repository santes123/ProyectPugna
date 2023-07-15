using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OutlineObject : MonoBehaviour
{
    public Material outlineMaterial;
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.05f;

    private Renderer objectRenderer;
    private Material originalMaterial;
    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
        EnableOutline();
        // Cargar el archivo .asset
        /*var settings = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>("Assets/Settings/URP-HighFidelity.asset");

        // Obtener acceso al submenu "Rendering"
        SerializedObject serializedSettings = new SerializedObject(settings);
        SerializedProperty renderingSettings = serializedSettings.FindProperty("m_RenderingSettings");

        // Obtener acceso a la propiedad "Depth Priming Mode" dentro del submenu "Rendering"
        SerializedProperty depthPrimingMode = renderingSettings.FindPropertyRelative("depthPrimingMode");

        // Acceder al valor de la propiedad "Depth Priming Mode"
        var depthPrimingModeValue = (DepthPrimingMode)depthPrimingMode.enumValueIndex;

        // Imprimir el valor de la propiedad "Depth Priming Mode"
        Debug.Log("Depth Priming Mode: " + depthPrimingModeValue);*/
    }

    public void EnableOutline()
    {
        objectRenderer.material = outlineMaterial;
        objectRenderer.material.SetColor("_OutlineColor", outlineColor);
        objectRenderer.material.SetFloat("_OutlineWidth", outlineWidth);
    }

    public void DisableOutline()
    {
        objectRenderer.material = originalMaterial;
    }
}
