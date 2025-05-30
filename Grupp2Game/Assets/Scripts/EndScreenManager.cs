using TMPro;
using UnityEngine;
//Author Vidar Edlund

public class EndScreenManager : MonoBehaviour
{
    [Header("End Screen Config")]
    [SerializeField] private GameObject scrollTextGameobject;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollDistance;

    [Header("Text")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timePlayedText;

    private void Start()
    {
        RemoveNotNeededGameObjects();
        GetScore();
    }
    private void GetScore()
    {
        var saveManager = GameObject.FindAnyObjectByType<SaveManager>();
        var score = saveManager.lastGameData.collectedCollectables;
        var timePlayed = saveManager._gameData.TimePlayed;

        scoreText.text = $"You collected\r\n{score}/{GameData.totalCollectables}\r\n";
        timePlayedText.text = timePlayed.ToString();
    }

    void Update()
    {
        Scrolling();
    }
    private void Scrolling()
    {
        if(scrollTextGameobject.transform.position.y < scrollDistance)
        {
            scrollTextGameobject.transform.Translate(0, scrollSpeed * Time.deltaTime, 0);
        }
    }

    //This should probably be moved to a "EndScreenManager"
    //or something like that. This class should be for scrolling. Not doing this
    //We don't want the Player, PlayerUI, to follow into the EndScreen so I will remove them
    private void RemoveNotNeededGameObjects()
    {
        GameObject PlayerHud = GameObject.Find("PlayerHUDCanvas");
        GameObject PlayerGameObject = GameObject.Find("Player");
        GameObject PlayerMainCamera = GameObject.Find("Main Camera");

        Destroy(PlayerHud);
        Destroy(PlayerGameObject);
        Destroy(PlayerMainCamera);
    }
    public void TerminateApplication()
    {
       DeleteAllSaveFiles();
        Application.Quit(); //Kill it
    }

    //When you have beaten the game you shouldn't be able to click "continue" in the mainmenu
    private void DeleteAllSaveFiles()
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
        fileSaver.DeleteAllFiles();
    }
}
