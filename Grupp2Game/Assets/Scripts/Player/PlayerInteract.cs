using System;
using UnityEngine;


//Author Vidar Edlund
/// <summary>
/// Component used to be able to interact with collectables
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    public event Action OnPickedUpCollectable;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectable c))
        {
            c.Collected();
            OnPickedUpCollectable?.Invoke();
        }
    }
}
