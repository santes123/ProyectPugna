using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;

    //functions buttons settings
    public void OpenAudioPanel()
    {
        settingsPanel.SetActive(true);
        settingsPanel.GetComponentInChildren<Slider>().value = GlobalVars.generalVolume;
        settingsPanel.GetComponentInChildren<Toggle>().isOn = GlobalVars.muteOn;
    }
    public void CloseAudioPanel()
    {
        settingsPanel.SetActive(false);
    }
}
