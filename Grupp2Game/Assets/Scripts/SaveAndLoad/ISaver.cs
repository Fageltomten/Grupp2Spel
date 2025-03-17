using UnityEngine;
//Author Vidar Edlund
public interface ISaver
{
    void Save(GameData gameData);
    GameData Load(string currentScene);
    GameData LoadLatest();
    bool FileExists(string filePath);
    bool FilesExists();
    void DeleteAllFiles();
    bool DeleteAllFilesConfirmation(bool shouldDelete);
}
