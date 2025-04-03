using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level
{
    Hub,
    HardDrive,
    CPU,
    GPU,
    Powersupply
}

public class SceneHandler : Singleton<SceneHandler>
{
    [Header("Levels")]
    [SerializeField] Level currentLevel;
    [SerializeField] Level previousLevel;

    public static Dictionary<Level, string> LevelToString = new Dictionary<Level, string>
    {
        { Level.Hub, "HubLevel" },
        { Level.HardDrive, "HarddriveLevel" }
    };

    ISaver saveSystem;
    public override void Awake()
    {
        base.Awake();
        saveSystem = new JsonSaver(false);
    }

    public void ChangeScene(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        SceneManager.LoadScene(LevelToString[level]);
    }
    
}
