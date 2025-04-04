using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level
{
    MainMenu,
    Persistance,
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
        { Level.MainMenu, "MainMenu" },
        { Level.Persistance, "PersistManagersScene" },
        { Level.Hub, "HubLevel" },
        { Level.HardDrive, "HarddriveLevel" }
    };

    //ISaver saveSystem;
    public override void Awake()
    {
        base.Awake();
        //saveSystem = new JsonSaver(false);
    }

    public IEnumerator ChangeScene(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        SceneManager.LoadScene(LevelToString[level]);

        return null;
    }

    public void ChangeSceneWithPersistance(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        SceneManager.LoadScene(LevelToString[Level.Persistance]);

        StartCoroutine(ChangeScene(level));
    }
    
}
