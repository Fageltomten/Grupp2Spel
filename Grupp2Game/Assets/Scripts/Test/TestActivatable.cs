using UnityEngine;

public class testActivatable : MonoBehaviour, IActivatable
{

    [SerializeField] string activationName;
    public void Activate()
    {
        Debug.Log("Activated " + activationName);
    }

    public void Deactivate()
    {
        Debug.Log("Deactivated " + activationName); 
    }

    
}
