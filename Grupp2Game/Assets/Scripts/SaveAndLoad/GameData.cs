using System.Collections.Generic;
using UnityEngine;

//Author Vidar Edlund
public class GameData
{
    //This number should be equal to the total amount of collectables that exists in all scenes
    //So if that number changes than you need to update this value here
    public const int totalCollectables = 16; //Change it to 16


    public int collectedCollectables;
    public string ActiveScene;
    public List<CollectableData> Collectable;
    public TimePlayed TimePlayed;

    //Default values when creating a new GameData
    public GameData()
    {
        collectedCollectables = 0;
        ActiveScene = null;
        TimePlayed = new TimePlayed(0, 0, 0);
        Collectable = new List<CollectableData>();
        Debug.Log("GameData with Default values initialized");
    }
    public static int CalculateCollectedCollectables()
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
    private static List<GameData> GetGameData()
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
[System.Serializable]
public class PlayerData
{
    public Vector2 Position;
    public float CurrentHealth;
    public int Deaths;
}
[System.Serializable]
public class CollectableData
{
    public string ID;
    public Vector3 Position;
    public bool IsCollected;

    public CollectableData(string iD, Vector3 position, bool isCollected)
    {
        ID = iD;
        Position = position;
        IsCollected = isCollected;
    }
}
[System.Serializable]
public class TimePlayed
{
    public int Hours;
    public int Minutes;
    public int Seconds;
    public TimePlayed(int hours, int minutes, int seconds)
    {
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }

    public TimePlayed Clone()
    {
        return new TimePlayed(Hours, Minutes, Seconds);
    }

    public override string ToString()
    {
        return $"Hours{Hours}, Minutes,{Minutes}, Seconds:{Seconds}";
    }
}
