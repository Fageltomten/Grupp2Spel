using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/* Author: Anton Andersson */

public enum Level
{
    MainMenu,
    Persistance,
    Hub,
    HardDrive,
    CPU,
    RAM
}

public class SceneHandler : Singleton<SceneHandler>
{
    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image progressBar;
    private float target;

    [Header("Levels")]
    [SerializeField] Level currentLevel;
    [SerializeField] Level previousLevel;

    public static Dictionary<Level, string> LevelToString = new Dictionary<Level, string>
    {
        { Level.MainMenu, "MainMenu" },
        { Level.Persistance, "PersistManagersScene" },
        { Level.Hub, "Hub" },
        { Level.HardDrive, "HDD" },
        { Level.RAM, "RAM" },
        { Level.CPU, "CPULevel" }
    };

    public static Dictionary<Level, Vector3> GetStartingPosition = new Dictionary<Level, Vector3>
    {
        { Level.Hub, new Vector3(0, 1, 0)},
        { Level.HardDrive, new Vector3(-15, 2, -14) },
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

    private void Update()
    {
        /* Updates the progressbar in the loading screen slowly instead of directly */
        if(progressBar.fillAmount != 1)
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }

    public async void ChangeScene(Level level)
    {
        previousLevel = currentLevel;
        currentLevel = level;

        target = 0f;
        progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(LevelToString[level]);
        scene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);

            target = scene.progress;
            progressBar.fillAmount = scene.progress;

        } while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;

        progressBar.fillAmount = 1;
        await Task.Delay(1000);
        loadingCanvas.SetActive(false);

        GameObject.FindAnyObjectByType<SaveManager>().SaveGame();
    }

    public async void ChangeSceneWithPersistance(Level level)
    {
        if(currentLevel == Level.MainMenu)
            GameObject.Find("SaveNotification").SetActive(false);

        progressBar.fillAmount = 0;
        loadingCanvas.SetActive(true);

        await SceneManager.LoadSceneAsync(LevelToString[Level.Persistance]);

        ChangeScene(level);
    }

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
            player.GetComponent<PlayerLook>().SetRotation(0, rotation.y);

        player.SetActive(true);
        Debug.Log($"After Position - {player.transform.position} $");
        Debug.Log("Position Changed  $");

        player.GetComponent<PlayerMovement>().SetSpeed(Vector3.zero);
    }
}
