using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    private static Stack<string> escenasAnteriores = new Stack<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void CambiarEscena(string nombreEscena)
    {
        // Guardar la escena actual en la pila
        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        escenasAnteriores.Push(nombreEscenaActual);

        Debug.Log("excena anterior = " + nombreEscenaActual);
        Debug.Log("excena cargada = " + nombreEscena);
        // Cargar la nueva escena
        SceneManager.LoadScene(nombreEscena);
    }

    public static void VolverEscenaAnterior()
    {
        // Verificar si hay escenas anteriores en la pila
        if (escenasAnteriores.Count > 0)
        {
            // Extraer el nombre de la escena anterior de la pila
            string nombreEscenaAnterior = escenasAnteriores.Pop();
            Debug.Log("escena anterior cargada = " + nombreEscenaAnterior);
            // Cargar la escena anterior
            SceneManager.LoadScene(nombreEscenaAnterior);
        }
        else
        {
            Debug.Log("No hay escenas anteriores en la pila.");
        }
    }
}
