using UnityEngine;


//Author Vidar Edlund
public class PlayerInteract : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectable c))
        {
            c.Collected();
        }
    }
}
