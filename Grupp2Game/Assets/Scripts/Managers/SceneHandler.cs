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
    public Level CurrentLevel { get { return currentLevel; } }
    public Level PreviousLevel { get { return previousLevel; } }

    public static Dictionary<Level, string> LevelToString = new Dictionary<Level, string>
    {
        { Level.MainMenu, "MainMenu" },
        { Level.Persistance, "PersistManagersScene" },
        { Level.Hub, "HubLevel" },
        { Level.HardDrive, "HarddriveLevel" },
        { Level.GPU, "GPULevel" },
    };

    public static Dictionary<Level, Vector3> GetStartingPosition = new Dictionary<Level, Vector3>
    {
        { Level.Hub, new Vector3(0, 1, 0)},
        { Level.HardDrive, new Vector3(0, 21, -36) },
        { Level.GPU, new Vector3(14, 4, 0) }
    };

    ISaver saveSystem;
    public override void Awake()
    {
        base.Awake();
        saveSystem = new JsonSaver(false);
    }

    /* Load without needing save data*/
    public IEnumerator ChangeScene(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        SceneManager.LoadScene(LevelToString[level]);

        GameObject.FindAnyObjectByType<SaveManager>().SaveGame();

        return null;
    }

    public void ChangeSceneWithPersistance(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        SceneManager.LoadScene(LevelToString[Level.Persistance]);

        StartCoroutine(ChangeScene(level));
    }
    

    /* Load with needing save data */
    public void ChangeToLatestScene()
    {
        
    }
}
