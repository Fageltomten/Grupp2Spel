using System.IO;
using UnityEngine;


// author: Carl Åslund
/// <summary>
/// static class that handles the loading and saving of the user settings
/// </summary>
public static class SettingsManager
{

    static string filePath = Application.persistentDataPath + "/savedSettings.json";


    static SettingsSave settings;

    public static SettingsSave GetSettings()
    {
        
        if (settings == null)
        {
            LoadSettings();
        }
        return settings;
    }

    static void CreateNewSettings()
    {
        settings = CreateDefaultSettings();
        SaveSettings();
    }

    static void LoadSettings()
    {
        // load settings from file;
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<SettingsSave>(jsonString);
        } else
        {
            CreateNewSettings();
            
        }
    }

    public static void SaveSettings()
    {
        // save settings to file;
        string jsonString = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, jsonString);
    }

    static SettingsSave CreateDefaultSettings()
    {
        SettingsSave defaultSettings = new SettingsSave();
        defaultSettings.isFullscreen = true;
        defaultSettings.masterVolume = 1;
        defaultSettings.musicVolume = 1;
        defaultSettings.playerVolume = 1;
        defaultSettings.effectsVolume = 1;
        defaultSettings.ambientVolume = 1;
        defaultSettings.timerToggle = false;
        defaultSettings.cameraSensitivity = 1;
        defaultSettings.invertedCamera = false;

        return defaultSettings;
    }

}

public class SettingsSave
{
    public bool isFullscreen;
    public float masterVolume;
    public float musicVolume;
    public float playerVolume;
    public float effectsVolume;
    public float ambientVolume;
    public bool timerToggle;
    public float cameraSensitivity;
    public bool invertedCamera;
};

