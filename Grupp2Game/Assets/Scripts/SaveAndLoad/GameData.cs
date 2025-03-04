using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    //Frågor
    //Hur ska man veta vilken data som ska hämtas baserat på scene
    //Enums är svaret
    //Note this is NOT the final save thing, its just pseudo code



    public PlayerData? PlayerData;
    public CollectableData? CollectableData;
}
public class CollectableData
{
    public List<Collectable> CPUs;
    public List<Collectable> GPUs;
    public List<Collectable> Motherboards;

    public CollectableData(List<Collectable> cPUs, List<Collectable> gPUs, List<Collectable> motherboards)
    {
        CPUs = cPUs;
        GPUs = gPUs;
        Motherboards = motherboards;
    }
    public CollectableData GetCollectableData()
    {
        return new CollectableData(CPUs, GPUs, Motherboards);
    }
}
public class PlayerData
{
    public Vector3 Position;
    public int CPUsCollected;
    public int GPUsCollected;
    public int MotherboardsCollected;
    public PlayerData(Vector3 position, int cPUsCollected, int gPUsCollected, int motherboardsCollected)
    {
        Position = position;
        CPUsCollected = cPUsCollected;
        GPUsCollected = gPUsCollected;
        MotherboardsCollected = motherboardsCollected;
    }
    public PlayerData GetPlayerData()
    {
        return new PlayerData(Position, CPUsCollected, GPUsCollected, MotherboardsCollected);
    }
}
public class Collectable
{
    public Vector3? Position;
    public bool IsCollected;
}
