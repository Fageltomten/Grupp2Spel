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
    Powersupply,
    RAM
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
        { Level.HardDrive, "HDD" },
        { Level.GPU, "GPU" },
        { Level.RAM, "RAM" },
        { Level.CPU, "CPULevel" }
    };

    public static Dictionary<Level, Vector3> GetStartingPosition = new Dictionary<Level, Vector3>
    {
        { Level.Hub, new Vector3(0, 1, 0)},
        { Level.HardDrive, new Vector3(-15, 2, -14) },
        { Level.GPU, new Vector3(14, 4, 0) },
        { Level.CPU, new Vector3(90, 36, -129) },
        { Level.RAM, new Vector3(3, 1, -6) }
    };

    public static Dictionary<Level, Vector3> GetStartingRotation = new Dictionary<Level, Vector3>
    {
        { Level.RAM, new Vector3(0, 90, 0) }
    };

    ISaver saveSystem;
    public override void Awake()
    {
        base.Awake();
        saveSystem = new JsonSaver(false);

        SceneManager.sceneLoaded += (sender, e) => ChoosePosition();
    }

    /* Load without needing save data*/
    public IEnumerator ChangeScene(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        Debug.Log("Loading Level $");
        SceneManager.LoadScene(LevelToString[level]);
        Debug.Log("Level Loaded  $");



        ///* Spelaren hittas inte*/
        //if (GameObject.FindAnyObjectByType<PlayerMovement>() != null)
        //    Debug.Log("Player Found $");
        //else
        //    Debug.Log("Player Not Found $");

        ////bool looking = true;
        ////while (looking)
        ////{
        ////    if (GameObject.FindAnyObjectByType<PlayerMovement>() != null)
        ////    {
        ////        Debug.Log("Player Found $");
        ////        looking = false;
        ////    }
        ////    else
        ////    {
        ////        Debug.Log("Player Not Found $");
        ////        Thread.Sleep(1000);
        ////    }
        ////}

        ///* Slutar funka n�r man k�r med persistance */
        //GameObject.FindAnyObjectByType<PlayerMovement>().transform.position = GetStartingPosition[level];
        //Debug.Log("Position Changed  $");

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

    public void ChoosePosition()
    {
        Vector3 pos = Vector3.zero;


        pos = GetStartingPosition[currentLevel];

        //GameObject player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            Debug.Log("Player Found $");
        else
            Debug.Log("Player Not Found $");

        Debug.Log($"Before Position - {player.transform.position} $");
        Debug.Log($"New Position - {pos} $");

        player.SetActive(false);

        player.transform.position = pos;
        if (GetStartingRotation.TryGetValue(currentLevel, out Vector3 rotation))
            player.GetComponent<PlayerLook>().SetRotation(rotation.z, rotation.y);

        player.SetActive(true);
        Debug.Log($"After Position - {player.transform.position} $");
        Debug.Log("Position Changed  $");

        player.GetComponent<PlayerMovement>().SetSpeed(Vector3.zero);
    }
}
