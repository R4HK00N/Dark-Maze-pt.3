using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsBehaviour : MonoBehaviour
{
    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    [Header("Audio Settings")]
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambienceVolumeSlider;

    [Header("Mouse Settings")]
    public Slider mouseSensitivitySlider;
    public float mouseSensitivityValue;

    public Resolution[] resolutions;
    public List<Resolution> validResolutions = new List<Resolution>();

    void Start()
    {
        LoadResolutions();
        LoadSettings();
    }

    void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        validResolutions.Clear();
        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            if (res.refreshRate == 144)
            {
                validResolutions.Add(res);
                options.Add(res.width + "x" + res.height + " @144Hz");
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        if (index >= 0 && index < validResolutions.Count)
        {
            Resolution res = validResolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen, 144);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        // Assign volume to your SFX audio mixer
    }

    public void SetMusicVolume(float volume)
    {
        // Assign volume to your Music audio mixer
    }

    public void SetAmbienceVolume(float volume)
    {
        // Assign volume to your Ambience audio mixer
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivityValue = sensitivity;
        // Apply sensitivity to player controller
    }

    public void LoadSettings()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        masterVolumeSlider.value = AudioListener.volume;
        mouseSensitivitySlider.value = mouseSensitivityValue;

        // Load resolution index
        for (int i = 0; i < validResolutions.Count; i++)
        {
            if (validResolutions[i].width == Screen.currentResolution.width &&
                validResolutions[i].height == Screen.currentResolution.height)
            {
                resolutionDropdown.value = i;
                break;
            }
        }
    }
}
