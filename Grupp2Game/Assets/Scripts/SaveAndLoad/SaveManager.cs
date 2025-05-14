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
    GameData lastGameData;
    public ISaver GetCurrentFileSaverSystem => _fileSaverSystem;
    public event EventHandler OnSaveHandler;


    public void Awake()
    {
        // _fileSystem.Init();
        _fileSaverSystem = new JsonSaver(_useEncryption);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded; ;
      //  SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        _saveables = GetAllSaveableObjects();
        LoadGame();
    }
    //private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    //{
    //    if(SceneManager.GetActiveScene().name == arg0.name)
    //    SaveGame();
    //}
    private void Start()
    {
        _saveables = GetAllSaveableObjects();
        LoadGame();
    }
    private List<ISaveable> GetAllSaveableObjects()
    {
        //Question, is this the only way to find interfaces?
        var saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToList();
        return saveables;
    }
    private void Update()
    {
        //Testing purpose
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SaveGame();
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    LoadGame();
        //}
    }
    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void SaveGame()
    {
        //Don't wanna save this middle scene
        //Its only used to load our gameobjects that we need
        if (SceneManager.GetActiveScene().name == "PersistManagersScene") return;

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
        //Don't wanna load this middle scene
        //Its only used to load our gameobjects that we need
        if (SceneManager.GetActiveScene().name == "PersistManagersScene") return;

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
    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
