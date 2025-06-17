using UnityEngine;

//Author Vidar Edlund
/// <summary>
/// Interface for objects that wants to be collectable
/// </summary>
public interface ICollectable
{
    bool IsCollected {  get; set; }
    void Collected();
}
