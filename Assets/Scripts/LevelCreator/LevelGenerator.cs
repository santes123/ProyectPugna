using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public string name;
    [Header("Configuracion color-prefab.")]
    public Color32 color;
    public GameObject prefab;
}
[System.Serializable]
public class MapLayer
{
    [Header("Nombre de Capa.")]
    public string name;
    public string ImageFileName;
    [Header("Configuracion de Capa.")]
    public ColorToPrefab[] colorToPrefabSettings;
    public Vector2 offset, spacing;
}
[System.Serializable]
public class Map
{
    public string name;
    public MapLayer[] capasDelMapa;
}


public class LevelGenerator : MonoBehaviour
{
    [Header("Mapa a generar.")]
    public Map mapa;
    //public Texture2D levelMap;
    //public ColorToPrefab[] colorToPrefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach(MapLayer layer in mapa.capasDelMapa) {
            GenerateLayer(layer);
        }
    }

    void GenerateLayer(MapLayer layer) {
        
        //image has to be a png... no compression
        string filePath = Application.dataPath + "/StreamingAssets/" + layer.ImageFileName;
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D levelMap = new Texture2D(2, 2); //doesnt matter the size at this point...
        levelMap.LoadImage(bytes);


        Color32[] allPixels = levelMap.GetPixels32();
        int width = levelMap.width;
        int height = levelMap.height;
        //recorremos la imagen para instanciar los objetos segun la imagen.
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                int index = i * height + j;

                //posicion del objeto a generar segun la imagen.
                SpawnPrefabAtPosition( allPixels[index], i , j, layer.offset, layer.spacing, layer.colorToPrefabSettings );
            }
        }
    }

    void SpawnPrefabAtPosition( Color32 c, int posX, int posY, Vector2 offset, Vector2 spacing, ColorToPrefab[] colorToPrefab) {
        //transparente no hace nada... es para el vacio.
        if(c.a <= 0) {
            return;
        }

        //Change for dictionary
        //escogemos el prefab a ubicar en la posicion.
        foreach(ColorToPrefab ctp in colorToPrefab) {
            if(ctp.color.Equals(c)) {
                //instanciamos el objeto.
                float x = (posX * spacing.x) + offset.x;
                float z = (posY * spacing.y) + offset.y;
                GameObject go = Instantiate( ctp.prefab, new Vector3(x, 0f, z), Quaternion.identity );
                //algo mas para configurar al gameobject?
                return;
            }
        }

        Debug.LogError("No se encontro prefab con el color: " + c.ToString());
    }
}
