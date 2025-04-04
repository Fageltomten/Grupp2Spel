using System;
using UnityEngine;
using UnityEngine.UI;

//Author: Carl Åslund
public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider playerSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider ambientSlider;


    private void Awake()
    {
        
        LoadSettings();
        ChangeVolume();
    }

    private void LoadSettings()
    {
        masterSlider.value = SettingsManager.GetSettings().masterVolume;
        playerSlider.value = SettingsManager.GetSettings().playerVolume;
        effectsSlider.value = SettingsManager.GetSettings().effectsVolume;
        musicSlider.value = SettingsManager.GetSettings().musicVolume;
        ambientSlider.value = SettingsManager.GetSettings().ambientVolume;
    }

    public void SetMasterVolume(float volume)
    {
        SettingsManager.GetSettings().masterVolume = volume;
        ChangeVolume();
    }

    public void SetPlayerVolume(float volume)
    {
        SettingsManager.GetSettings().playerVolume = volume;
        ChangeVolume();
    }

    public void SetEffectsVolume(float volume)
    {
        SettingsManager.GetSettings().effectsVolume = volume;
        ChangeVolume();
    }

    public void SetMusicVolume(float volume)
    {
        SettingsManager.GetSettings().musicVolume = volume;
        ChangeVolume();
    }

    public void SetAmbientVolume(float volume)
    {
        SettingsManager.GetSettings().ambientVolume = volume;
        ChangeVolume();
    }

    void ChangeVolume()
    {
        
    }

}
