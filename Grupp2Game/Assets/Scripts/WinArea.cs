using UnityEngine;

public class WinArea : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
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

        //foreach (var item in fileSaver.)
        //{
        //    //Loop through each save file
        //    //Somehow
        //}

        if(GameData.totalCollectables <= collectedCollectables)
        {
            Debug.Log("You win");
        }
        //else
        //{
        //}
    }
}
