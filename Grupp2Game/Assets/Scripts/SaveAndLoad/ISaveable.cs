using UnityEngine;

//Author Vidar Edlund
/// <summary>
/// Interface for GameObject that want to be able to save data
/// </summary>
public interface ISaveable
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}
