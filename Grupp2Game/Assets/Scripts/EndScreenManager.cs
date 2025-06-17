using TMPro;
using UnityEngine;
//Author Vidar Edlund

/// <summary>
/// Script that is responsible for adding a scrolling effect for the endscreen,
/// Getting the final score and adding logic for terminating the game on quit
/// </summary>
public class EndScreenManager : MonoBehaviour
{
    [Header("End Screen Config")]
    [SerializeField] private GameObject scrollTextGameobject;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollDistance;
    [SerializeField] private bool finishedScrolling;

    [Header("Text")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timePlayedText;

    private void Start()
    {
        RemoveNotNeededGameObjects();
        GetScore();
    }
    /// <summary>
    /// Retrieves total amount of collectables collected and time played
    /// to display it to the player
    /// </summary>
    private void GetScore()
    {
        var saveManager = GameObject.FindAnyObjectByType<SaveManager>();
        var score = GameData.CalculateCollectedCollectables();
        var timePlayed = saveManager._gameData.TimePlayed;

        scoreText.text = $"You collected\r\n{score}";
        timePlayedText.text = timePlayed.ToString();
    }

    void Update()
    {
        if (!finishedScrolling)
        Scrolling();
    }
    /// <summary>
    /// Moves a gameobject vertically upwards and stops when all end credits have been shown on screen
    /// determined by a specific y position
    /// </summary>
    private void Scrolling()
    {
        if(scrollTextGameobject.transform.position.y < scrollDistance && !finishedScrolling)
        {
            scrollTextGameobject.transform.Translate(0, scrollSpeed * Time.deltaTime, 0);
            if(scrollTextGameobject.transform.position.y >= scrollDistance)
            {
                finishedScrolling = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            finishedScrolling = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

   
    //We don't need the Player object, PlayerUI, playerCamera and Pause canvas
    //in the EndScreen so thye are removed
    private void RemoveNotNeededGameObjects()
    {
        GameObject PlayerHud = GameObject.Find("PlayerHUDCanvas");
        GameObject PlayerGameObject = GameObject.Find("Player");
        GameObject PlayerMainCamera = GameObject.Find("Main Camera");
        GameObject PlayerPauseCanvas = GameObject.Find("PauseCanvas");

        Destroy(PlayerHud);
        Destroy(PlayerGameObject);
        Destroy(PlayerMainCamera);
        Destroy(PlayerPauseCanvas);
    }
    /// <summary>
    ///We decided that when you beat the game your save files should be deleted so that you can 
    ///start the game from a clean state next time you play the game.
    ///(This was a design decision, we could simply have made you spawn in the HUB level when you press continue
    ///after having beaten the game, and let you roam around freely)
    /// </summary>
    public void TerminateApplication()
    {
       DeleteAllSaveFiles();
       Application.Quit(); 
    }

    
    /// <summary>
    /// Gets the current fileSaverSystem that is being used by the SaveManager
    /// and calls its DeleteAllFiles
    /// </summary>
    private void DeleteAllSaveFiles()
    {
        SaveManager saveManager = GameObject.FindAnyObjectByType<SaveManager>();
        if (saveManager == null)
        {
            Debug.Log("saveManaeger returned null for some reason in EndScreen");
        }
        ISaver fileSaver = saveManager.GetCurrentFileSaverSystem;
        if (fileSaver == null)
        {
            Debug.Log("fileSaver returned null for some reason in EndScreen");
        }
        fileSaver.DeleteAllFiles();
    }
}
