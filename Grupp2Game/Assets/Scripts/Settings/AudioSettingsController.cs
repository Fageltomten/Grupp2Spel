using System;
using UnityEngine;
using UnityEngine.UI;

//Author: Carl Åslund
/// <summary>
/// Component that handles the changes in values in the Audio part of the settings menu.
/// </summary>
public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider playerSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider ambientSlider;

    AudioMenu audioMenu;
    SettingsSave settings;

    private void Awake()
    {
        audioMenu = GetComponent<AudioMenu>();
        settings = SettingsManager.GetSettings();
        LoadSettings();
        ChangeVolume();
    }

    private void LoadSettings()
    {
        masterSlider.value = settings.masterVolume;
        playerSlider.value = settings.playerVolume;
        effectsSlider.value = settings.effectsVolume;
        musicSlider.value = settings.musicVolume;
        ambientSlider.value = settings.ambientVolume;
    }

    public void SetMasterVolume(float volume)
    {
        settings.masterVolume = volume;
        ChangeVolume();
    }

    public void SetPlayerVolume(float volume)
    {
        settings.playerVolume = volume;
        ChangeVolume();
    }

    public void SetEffectsVolume(float volume)
    {
        settings.effectsVolume = volume;
        ChangeVolume();
    }

    public void SetMusicVolume(float volume)
    {
        settings.musicVolume = volume;
        ChangeVolume();
    }

    public void SetAmbientVolume(float volume)
    {
        settings.ambientVolume = volume;
        ChangeVolume();
    }

    void ChangeVolume()
    {
        audioMenu.SetMasterVolume(settings.masterVolume + 0.01f);
        audioMenu.SetMusicVolume(settings.musicVolume + 0.01f);
        audioMenu.SetplayerVolume(settings.playerVolume + 0.01f);
        audioMenu.SetSFXVolume(settings.effectsVolume + 0.01f);
        audioMenu.SetAmbienceVolume(settings.ambientVolume + 0.01f);
    }

}
