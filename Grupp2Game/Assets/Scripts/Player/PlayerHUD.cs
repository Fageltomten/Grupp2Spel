using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Author Vidar Edlund
public class PlayerHUD : MonoBehaviour, ISaveable
{
    [SerializeField] private TMP_Text collectedCollectablesText;
    [SerializeField] private TMP_Text timerScoreText;
    private float time;
    private int collectedCollectables = 0;
    private int collectablesInScene = 0;
    private TimeSpan timePlayed;
    private TimeSpan savedTimePlayed;



    [SerializeField] private Image crosshair; //Change color and stuff when interacting?
    [SerializeField] private Color canGrappleColor;
    [SerializeField] private Color canNotGrappleColor;

    [SerializeField] private SaveManager saveManager;
    private GrapplingHook grapplingHook;

    public void Awake()
    {
        //Call LevelManager to update collect score
        //Or SaveManager
        // DefaultTextValues();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
       // SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        FindAnyObjectByType<PlayerInteract>().OnPickedUpCollectable += PlayerInteract_OnPickedUpCollectable;
    }

    private void Start()
    {
        grapplingHook = GameObject.FindAnyObjectByType<GrapplingHook>();
        if(grapplingHook == null )
        {
            Debug.Log("Playermovement is null in playerhud");
        }
    }

    //private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    //{
    //   // if(SceneManager.GetActiveScene().name == arg0.name)
    //   // saveManager.SaveGame();
    //}

    private void PlayerInteract_OnPickedUpCollectable()
    {
        collectedCollectables++;
        saveManager.SaveGame();
        UpdateHud();
    }
    //private void DefaultTextValues()
    //{
    //    collectedCollectablesText.text = $"{0}/10";
    //}
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        collectablesInScene = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ICollectable>().Count();
        //saveManager.LoadGame();
        //UpdateHud();
    }

    private void Update()
    {
        UpdateTimer();
        UpdateCrosshairColor();
    }

    private void UpdateCrosshairColor()
    {
       if(grapplingHook.CanGrapple())
        {
            crosshair.color = canGrappleColor;
        }
       else
        {
            crosshair.color = canNotGrappleColor;
        }
    }

    private void UpdateTimer()
    {
        time = Time.timeSinceLevelLoad;
        timePlayed = TimeSpan.FromSeconds(time);
        timerScoreText.text = $"Time: {(savedTimePlayed + timePlayed).Hours:00}:{(savedTimePlayed + timePlayed).Minutes:00}:{(savedTimePlayed + timePlayed).Seconds:00}";
    }
    public void UpdateHud()
    {
        collectedCollectablesText.text = $"{collectedCollectables}/{collectablesInScene}";
        timerScoreText.text = $"Time: {timePlayed.Hours:00}:{timePlayed.Minutes:00}:{timePlayed.Seconds:00}";
    }

    public void LoadData(GameData data)
    {
       collectedCollectables = data.collectedCollectables;
        savedTimePlayed = new TimeSpan(data.TimePlayed.Hours, data.TimePlayed.Minutes, data.TimePlayed.Seconds);
        Debug.Log("Loading playerdata");
        UpdateHud();
    }

    public void SaveData(GameData data)
    {
        data.collectedCollectables = collectedCollectables;
        data.TimePlayed.Hours = (savedTimePlayed + timePlayed).Hours;
        data.TimePlayed.Minutes = (savedTimePlayed + timePlayed).Minutes;
        data.TimePlayed.Seconds = (savedTimePlayed + timePlayed).Seconds;
    }
}
