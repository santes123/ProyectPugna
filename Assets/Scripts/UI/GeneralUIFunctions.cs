using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralUIFunctions : MonoBehaviour
{
    public void GoBackToPreviousScene()
    {
        string lastScene = GlobalVars.lastSceneMovingOnMenus;
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        /*SceneManager.LoadScene(lastScene);*/
        //usamos el menu manager para cambiar de escena y guardar la escena en una pila
        //FindObjectOfType<MenuManager>().VolverEscenaAnterior();
        MenuManager.VolverEscenaAnterior();
    }
    //metodo para abrir la escena de controles en el menu Settings
    public void OpenControls()
    {
        //guardamos la escena anterior
        GlobalVars.lastSceneMovingOnMenus = SceneManager.GetActiveScene().name;
        //usamos el menu manager para cambiar de escena y guardar la escena en una pila
        MenuManager.CambiarEscena("KeybindingsScene");
        //SceneManager.LoadScene("KeybindingsScene");
    }


}
