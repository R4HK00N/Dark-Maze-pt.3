using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer _audioMixer;

    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _ambientSlider;
    [SerializeField] Slider _sfxSlider;

    [Header("Video")]
    List<Resolution> _resolutions = new();
    [SerializeField] TMP_Dropdown _resDropDown;

    [SerializeField] TMP_Dropdown _fullScreenDrowdown;




    // Start is called before the first frame update
    void Start()
    {
        GetAndSetResolution();

        #region Audio Start

        // Master Volume
        float masterVol = PlayerPrefs.GetFloat("MasterVol", 1f);
        InitializeVolume("Master", masterVol, _masterSlider);

        // Music Volume
        float ambientVol = PlayerPrefs.GetFloat("MusicVol", 1f);
        InitializeVolume("Ambient", ambientVol, _ambientSlider);

        // SFX Volume
        float sfxVol = PlayerPrefs.GetFloat("SfxVol", 1f);
        InitializeVolume("SFX", sfxVol, _sfxSlider);

        #endregion
    }

    void InitializeVolume(string volumeParameter, float volumeLevel, Slider slider)
    {
        // Set volume based on whether volume level is zero or non-zero
        if (volumeLevel > 0)
        {
            _audioMixer.SetFloat(volumeParameter, Mathf.Log10(volumeLevel) * 20);
        }
        else
        {
            _audioMixer.SetFloat(volumeParameter, -80f); // Minimum dB level when volume level is 0
        }

        // Set slider and input field values
        if (slider != null)
        {
            slider.value = volumeLevel;
        }
    }

    #region Audio
    public void SetMasterVol(float masterLvl)
    {
        if (masterLvl > 0)
        {
            _audioMixer.SetFloat("Master", Mathf.Log10(masterLvl) * 20);
        }
        else
        {
            _audioMixer.SetFloat("Master", -80f); // Set to a minimum dB level when slider is at 0
        }
        PlayerPrefs.SetFloat("Master", masterLvl);
    }

    public void SetAmbientVol(float musicLvl)
    {
        if (musicLvl > 0)
        {
            _audioMixer.SetFloat("Ambient", Mathf.Log10(musicLvl) * 20);
        }
        else
        {
            _audioMixer.SetFloat("Ambient", -80f); // Set to a minimum dB level when slider is at 0
        }
        PlayerPrefs.SetFloat("Ambient", musicLvl);
    }

    public void SetSFXVol(float sfxLvl)
    {
        if (sfxLvl > 0)
        {
            _audioMixer.SetFloat("SFX", Mathf.Log10(sfxLvl) * 20);
        }
        else
        {
            _audioMixer.SetFloat("SFX", -80f); // Set to a minimum dB level when slider is at 0
        }
        PlayerPrefs.SetFloat("Sfx", sfxLvl);
    }
    #endregion

    #region Video

    #region Resolution

    public void GetAndSetResolution()
    {
        Resolution[] tempRes = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        for (int i = tempRes.Length - 1; i > 0; i--)
        {
            _resolutions.Add(tempRes[i]);
        }
        _resDropDown.ClearOptions();

        List<string> options = new();

        for (int i = 0; i < _resolutions.Count; i++)
        {
            string Option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(Option);
        }
        // options.Reverse();
        _resDropDown.AddOptions(options);
        _resDropDown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
        _resDropDown.value = index;
        _resDropDown.RefreshShownValue();
    }

    public void NewResolution(int index)
    {
        _resDropDown.value = index;
        _resDropDown.RefreshShownValue();

        SetResolution(index);
    }
    #endregion

    #region Fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void FullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }
    void Borderless()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
    void Windowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    #endregion

    public void DoVsync(bool value)
    {
        if (value)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = _resolutions[0].refreshRate;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 0;
        }
    }

    #endregion
}