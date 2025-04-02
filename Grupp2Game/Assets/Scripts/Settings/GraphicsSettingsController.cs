using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Author: Carl Åslund
public class GraphicsSettingsController : MonoBehaviour
{
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Awake()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        PopulateResDropDown();
        bool isFullscreen = SettingsManager.GetSettings().isFullscreen;
        fullscreenToggle.isOn = isFullscreen;
        ChangeFullscreen(isFullscreen);
    }

    void PopulateResDropDown()
    {
        int currentResIndex = 0;
        List<string> resNames = new List<string>();

        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resNames.Add(resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString());

            if (Screen.currentResolution.ToString() == resolutions[i].ToString())
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resNames);
        resolutionDropdown.value = currentResIndex;

    }

    public void ChangeResolution(int resIndex)
    {
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Screen.fullScreen);
    }

    public void ChangeFullscreenFromToggle()
    {
        Debug.Log(fullscreenToggle.isOn);
        ChangeFullscreen(fullscreenToggle.isOn);
    }

    void ChangeFullscreen(bool active)
    {
        Screen.fullScreen = active;
        SettingsManager.GetSettings().isFullscreen = active;
        
    }

}
