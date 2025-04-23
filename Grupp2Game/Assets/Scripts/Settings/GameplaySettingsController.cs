using System;
using UnityEngine;
using UnityEngine.UI;

// Author: Carl Åslund
/// <summary>
/// Component that handles the changes in values in the Gameplay part of the settings menu.
/// </summary>
public class GameplaySettingsController : MonoBehaviour
{
    [SerializeField] Toggle timerToggle;
    [SerializeField] Toggle invertedToggle;
    [SerializeField] Slider sensitivitySlider;

    private void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        timerToggle.isOn = SettingsManager.GetSettings().timerToggle;
        invertedToggle.isOn = SettingsManager.GetSettings().invertedCamera;
        sensitivitySlider.value = SettingsManager.GetSettings().cameraSensitivity;
    }

    public void ChangeTimer(bool value)
    {
        SettingsManager.GetSettings().timerToggle = value;
    }

    public void ChangeInverted(bool value)
    {
        SettingsManager.GetSettings().invertedCamera = value;
    }

    public void ChangeSensitivity(float value) 
    {
        SettingsManager.GetSettings().cameraSensitivity = value;
    }

}
