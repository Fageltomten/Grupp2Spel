using System.Collections.Generic;
using System.IO;
using UnityEngine;
//Author Vidar Edlund

/// <summary>
/// Interface that contains methods for saving, retrieving, deleting and modyfing data
/// </summary>
public interface ISaver
{
    void Save(GameData gameData);
    GameData Load(string currentScene);
    GameData LoadLatest();
    bool FileExists(string filePath);
    bool FilesExists();
    void DeleteAllFiles();
    bool DeleteAllFilesConfirmation(bool shouldDelete);
    public FileInfo[] GetAllCurrentlySavedFiles();
    public List<GameData> GetAllCurrentlySavedGameData();
}
