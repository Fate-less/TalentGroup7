using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    private Resolution[] resolutions;


    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        LoadSettings();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        SaveSettings();
    }
    [HideInInspector]
    public void SetVolume(float volume)
    {
        volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
        SaveSettings();
    }

    private void LoadSettings()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;

        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        SetVolume(volume);
    }
    public void SaveSettings()
    {
        int isFullscreen = fullscreenToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen);

        float volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volume);

        PlayerPrefs.Save();
    }
}
