using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinArea : MonoBehaviour
{
    [SerializeField] private TMP_Text winAreaText;
    private GameData gameData;

    private void Start()
    {
        
    }
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
            //Loop through each save file
            //Somehow
            collectedCollectables += gamedata.collectedCollectables;
        }
        return collectedCollectables;
    }
    public void CheckForWin()
    {
        int collectedCollectables = CalculateCollectedNutsAndScrews();
        if (GameData.totalCollectables <= collectedCollectables)
        {
            Debug.Log("You win");
            SceneManager.LoadScene("EndScreen");
        }
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            CheckForWin();
        }
    }
}
