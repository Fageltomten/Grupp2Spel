using System;
using UnityEngine;

// Class by Carl �slund
public class ActivationManager : MonoBehaviour
{

    public static ActivationManager Instance { get; private set; }

    public event Action<int> OnActivation;
    public event Action<int> OnDeactivate;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
    }

    public void Activate(int channel)
    {
        if (OnActivation != null)
        {
            OnActivation.Invoke(channel);
        }
    }

    public void Deactivate(int channel)
    {
        if (OnDeactivate != null)
        {
            OnDeactivate.Invoke(channel);
        }
    }
}
