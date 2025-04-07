using System;
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
        { Level.Hub, "Hub" },
        { Level.HardDrive, "HardDrive" },
        { Level.GPU, "GPU" },
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
        SceneManager.LoadScene(LevelToString[Level.Persistance]);

      StartCoroutine(LoadingSavedScene());
     }
    private IEnumerator LoadingSavedScene()
    {
        //Why do I do this? Because I just want to see the middle screen
        float waitTime = 1f;
        yield return new WaitForSeconds(waitTime);

        GameData data = saveSystem.LoadLatest();
        if (data != null)
        {
            SceneManager.LoadScene(data.ActiveScene);

            yield return null;

            SetPlayerStartPosition(data.ActiveScene);
        }
        Level level = Enum.Parse<Level>(data.ActiveScene);
        currentLevel = level;
    }

    private void SetPlayerStartPosition(string sceneToLoad)
    {
        //Maybe I should do some check to see if the value exists?
        //Maybe, but it would only crash if we/I wrote the wrong value

        Level level = Enum.Parse<Level>(sceneToLoad);
        if (GetStartingPosition.TryGetValue(level, out Vector3 startPos))
        {
            if (GameObject.FindAnyObjectByType<PlayerMovement>() == null)
            {
                Debug.Log("Player not found");
            }
            GameObject.FindAnyObjectByType<PlayerMovement>().transform.position = startPos;
        }
        else
        {
            Debug.LogError($"Could not find the specific scene name(key) in the 'PlayerSceneSpawnPosition' dictionary" +
                $" when trying to get the start position for the player in the specific scene. Did you mean to load another scene or did you make a typo?");
            Debug.LogError($"Error occured with the scene: {sceneToLoad}");
        }
    }
    private void ChoosePosition()
    {
        Vector3 pos = Vector3.zero;

        pos = SceneHandler.GetStartingPosition[currentLevel];
        GameObject.FindAnyObjectByType<PlayerMovement>().transform.position = pos;
    }
}
