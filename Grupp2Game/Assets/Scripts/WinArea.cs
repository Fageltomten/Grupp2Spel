using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//Author Vidar Edlund

public class WinArea : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text winAreaText;
    [Header("Data")]
    [SerializeField] private int collectedCollectables;
    private SaveManager saveManager;
    private GameData gameData;
    private void Start()
    {
        UpdateCollectedAmount();
        UpdatedCollectedText();


        saveManager = GameObject.FindAnyObjectByType<SaveManager>();
        saveManager.OnSaveHandler += SaveManager_OnSaveHandler;
    }

    private void SaveManager_OnSaveHandler(object sender, System.EventArgs e)
    {
        UpdateCollectedAmount();
        UpdatedCollectedText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            CheckForWin();
        }
    }
    /// <summary>
    /// If your current amount of collected collectables are equal or more than x collectables
    /// then you win
    /// </summary>
    public void CheckForWin()
    {
        if (GameData.totalCollectables <= collectedCollectables)
        {
            Debug.Log("You win");
            GameObject.FindAnyObjectByType<SaveManager>().SaveGame();
            SceneManager.LoadScene("EndScreen");
        }
    }
    /// <summary>
    /// Display current collected collectables.
    /// </summary>
    private void UpdatedCollectedText()
    {
        winAreaText.text = $"{collectedCollectables}/{GameData.totalCollectables}";
    }
    private void UpdateCollectedAmount()
    {
        collectedCollectables = CalculateCollectedNutsAndScrews();
    }

    //TODO: These 2 methods have duplicate code from GameData class
    //Should fix duplication

    /// <summary>
    /// Retrieves a list of all saved gamedata and 
    /// iterates over each element 
    /// summing up the number of collectables collected on each gamedata object
    /// </summary>
    /// <returns>Collected collectables</returns>
    private int CalculateCollectedNutsAndScrews()
    {
        var savedGameData = GetGameData();
        int collectedCollectables = 0;
        if (savedGameData == null)
        {
            Debug.Log("Why the fuck is this null?");
        }
        foreach (var gamedata in savedGameData)
        {
            collectedCollectables += gamedata.collectedCollectables;
        }
        return collectedCollectables;
    }
    /// <summary>
    /// Retrieves a list of all currently saved gamedata
    /// </summary>
    /// <returns>A list containing all gamedata that is currently saved</returns>
    private List<GameData> GetGameData()
    {
        SaveManager saveManager = GameObject.FindAnyObjectByType<SaveManager>();
        if (saveManager == null)
        {
            Debug.Log("saveManaeger returned null for some reason in WinArea");
        }
        ISaver fileSaver = saveManager.GetCurrentFileSaverSystem;
        if (fileSaver == null)
        {
            Debug.Log("fileSaver returned null for some reason in WinArea");
        }

        List<GameData> savedGameData = fileSaver.GetAllCurrentlySavedGameData();
        return savedGameData;
    }
}
