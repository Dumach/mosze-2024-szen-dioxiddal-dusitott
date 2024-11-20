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
            float volume = 1f;
            if(PlayerPrefs.HasKey("Volume")) volume = PlayerPrefs.GetFloat("Volume");
            backgroundMusic.volume = volume;
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
            PlayerPrefs.SetFloat("Volume", volume);
            PlayerPrefs.Save();
        }

        UpdateVolumeText(volume);
    }

    private void UpdateVolumeText(float volume)
    {
        int volumePercentage = Mathf.RoundToInt(volume * 100);
        volumeText.text = volumePercentage + "%";
    }
}

