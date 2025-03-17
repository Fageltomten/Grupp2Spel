using System;
using UnityEngine;


//Author Vidar Edlund
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
