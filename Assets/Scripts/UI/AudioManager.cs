using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;

    private AudioSource audioSource;
    private float initialVolume;
    public TextMeshProUGUI volumePercentageText;

    private void Start()
    {
        
        if (GetComponent<AudioSource>())
        {
            audioSource = GetComponent<AudioSource>();
            initialVolume = audioSource.volume;
        }
        

        // Establecer el valor inicial del slider y el estado del checkbox
        volumeSlider.value = initialVolume;
        //muteToggle.isOn = !audioSource.isPlaying;
        muteToggle.isOn = false;

        // Actualizar el texto del porcentaje de volumen
        UpdateVolumePercentageText();

        // Suscribirse a los eventos del slider y el checkbox
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteStateChanged);
    }

    private void OnVolumeChanged(float value)
    {
        // Actualizar el volumen del AudioSource en base al valor del slider
        if (GetComponent<AudioSource>())
        {
            audioSource.volume = value;
            Debug.Log("audioSource.volume = " + audioSource.volume);
        }
        GlobalVars.generalVolume = value;
        // Actualizar el texto del porcentaje de volumen
        UpdateVolumePercentageText();
        //llamamos a la funcion del gamemaner para actualizar el volumen (si existe)
        if (FindObjectOfType<GameManager>())
        {
            FindObjectOfType<GameManager>().SetAudioSourcesVolume();
        }
    }

    private void OnMuteStateChanged(bool isMuted)
    {
        // Mutea o desmutea el AudioSource en base al estado del checkbox
        if (GetComponent<AudioSource>())
        {
            audioSource.mute = isMuted;
            Debug.Log("audioSource.mute = " + audioSource.mute);
        }

        GlobalVars.muteOn = isMuted;
        //llamamos a la funcion del gamemaner para actualizar el volumen (si existe)
        if (FindObjectOfType<GameManager>())
        {
            FindObjectOfType<GameManager>().SetAudioSourcesVolume();
        }
    }
    private void UpdateVolumePercentageText()
    {
        // Calcular el porcentaje de volumen
        int volumePercentage = Mathf.RoundToInt(volumeSlider.value * 100f);

        // Actualizar el texto del porcentaje de volumen
        volumePercentageText.text = volumePercentage.ToString() + "%";
    }
}
