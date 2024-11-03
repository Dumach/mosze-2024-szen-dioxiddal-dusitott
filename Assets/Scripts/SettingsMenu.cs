using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Slider volumeSlider;
    public Text volumeText;

    void Start()
    {
       
        if (backgroundMusic != null)
        {
            volumeSlider.value = backgroundMusic.volume;
        }

       
        volumeSlider.onValueChanged.AddListener(SetVolume);

        UpdateVolumeText(volumeSlider.value);
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
        }

        UpdateVolumeText(volume);
    }

    private void UpdateVolumeText(float volume)
    {
        int volumePercentage = Mathf.RoundToInt(volume * 100);
        volumeText.text = volumePercentage + "%";
    }
}

