using UnityEngine;
//Author Vidar Edlund
public interface ISaver
{
    void Save(GameData gameData);
    GameData Load(string currentScene);
    bool FileExists(string filePath);
}
