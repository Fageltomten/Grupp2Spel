using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinArea : MonoBehaviour
{
    public void CheckForWin()
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
        int collectedCollectables = 0;

        List<GameData> savedGameData = fileSaver.GetAllCurrentlySavedGameData();
        if (savedGameData == null)
        {
            Debug.Log("Why the fuck is this null?");
            return;
        }
        foreach (var gamedata in savedGameData)
        {
            //Loop through each save file
            //Somehow
            collectedCollectables += gamedata.collectedCollectables;
        }

        if (GameData.totalCollectables <= collectedCollectables)
        {
            Debug.Log("You win");
            SceneManager.LoadScene("EndScreen");
        }
        //else
        //{
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            CheckForWin();
        }
    }
}
