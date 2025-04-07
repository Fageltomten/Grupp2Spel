using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Loading;
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

        Debug.Log("Loading Level $");
        SceneManager.LoadScene(LevelToString[level]);
        Debug.Log("Level Loaded  $");

        
        /* Spelaren hittas inte*/
        if (GameObject.FindAnyObjectByType<PlayerMovement>() != null)
            Debug.Log("Player Found $");
        else
            Debug.Log("Player Not Found $");

        /* Slutar funka när man kör med persistance */
        GameObject.FindAnyObjectByType<PlayerMovement>().transform.position = GetStartingPosition[level];
        Debug.Log("Position Changed  $");

        GameObject.FindAnyObjectByType<SaveManager>().SaveGame();

        return null;
    }

    public void ChangeSceneWithPersistance(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        SceneManager.LoadScene(LevelToString[Level.Persistance]);

        StartCoroutine(ChangeScene(level));
        //ChangeScene(level);
    }
    

    /* Load with needing save data */
    public void ChangeToLatestScene()
    {
        GameData data = saveSystem.LoadLatest();
        if (data != null)
        {
            currentLevel = LevelToString.ToDictionary(x => x.Value, x => x.Key)[data.ActiveScene];

            ChangeSceneWithPersistance(currentLevel);
        }
     }

    private void ChoosePosition()
    {
        Vector3 pos = Vector3.zero;

        pos = SceneHandler.GetStartingPosition[currentLevel];
        GameObject.FindAnyObjectByType<PlayerMovement>().transform.position = pos;
    }
}
