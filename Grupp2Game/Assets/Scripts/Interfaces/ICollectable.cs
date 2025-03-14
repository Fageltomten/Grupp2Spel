using UnityEngine;

public interface ICollectable
{
    bool IsCollected {  get; set; }
    void Collected();
}
