using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author Vidar Edlund
public class SaveManager : MonoBehaviour
{
    [Header("File Config")]
    //[SerializeField] private string _fileName; //Might add this again later
    [SerializeField] private bool _useEncryption;

    private List<ISaveable> _saveables = new List<ISaveable>();
    private ISaver _fileSaverSystem;
    public GameData _gameData;
    public GameData lastGameData;
    public ISaver GetCurrentFileSaverSystem => _fileSaverSystem;
    public event EventHandler OnSaveHandler;


    public void Awake()
    {
        // _fileSystem.Init();
        _fileSaverSystem = new JsonSaver(_useEncryption);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //If the current scene you load into have any gamedata saved then load it
        _saveables = GetAllSaveableObjects();
        LoadGame();
    }
    
    private void Start()
    {
        //Load in saved gamedata when saveManager is first initialized.
        //But I think this one should be removed, I don't think this one does anything right now
        //The event handler above should handle everything^
        _saveables = GetAllSaveableObjects();
        LoadGame();
    }

    /// <summary>
    /// Find all gameobject that have a component that implement the ISaveable interface
    /// </summary>
    /// <returns></returns>
    private List<ISaveable> GetAllSaveableObjects()
    {
        var saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToList();
        return saveables;
    }
   
    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void SaveGame()
    {
        if (SkipScene()) return;

        if (_gameData == null) return;
        if(!_fileSaverSystem.FileExists(SceneManager.GetActiveScene().name))
        {
            NewGame();
        }
        //Go through each object that should be saved and gather its data
        foreach (var s in _saveables)
        {
            s.SaveData(_gameData);
        }

        _gameData.ActiveScene = SceneManager.GetActiveScene().name;
        //Now that we have the data that should be saved we give it to the file system
        lastGameData = _gameData;
        _fileSaverSystem.Save(_gameData);

        //Tell WinArea and other UI/Objects that we saved so that they go do things
        OnSaveHandler?.Invoke(this, EventArgs.Empty);
    }
    public void LoadGame()
    {
        if(SkipScene()) return;

        //To pass time from the latest save
        //We need to do it here before Load gets called which will change
        //the variable "fileName" in JsonSaver which causes this to not work
        //If we where to add it after the load
        //GameData latestGameData = _fileSaverSystem.LoadLatest();

        _gameData = _fileSaverSystem.Load(SceneManager.GetActiveScene().name);
        //If no data is found then we want to create a new data
        if (_gameData == null)
        {
            Debug.Log("No game data could be found, initializing data to default values");
            NewGame();
        }

        if (lastGameData != null && lastGameData.TimePlayed != null)
            _gameData.TimePlayed = lastGameData.TimePlayed.Clone();
        //_gameData.TimePlayed = latestGameData.TimePlayed;

        //Give each saveable object the saved data
        foreach (var s in _saveables)
        {
            s.LoadData(_gameData);
        }
    }
    private bool SkipScene()
    {
        //Some scenes shouldn't be saved so we skip over them.
        if (SceneManager.GetActiveScene().name == "PersistManagersScene" || SceneManager.GetActiveScene().name == "EndScreen") return true;
        return false;
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
