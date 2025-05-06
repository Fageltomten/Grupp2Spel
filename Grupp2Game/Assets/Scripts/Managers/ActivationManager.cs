using System;
using UnityEngine;

// Class by Carl Åslund

/// <summary>
/// Singleton manager that handles the requests for objects to be activated and deactivated 
/// </summary>
public class ActivationManager : MonoBehaviour
{
    [Tooltip("Colors of Channels")]
    [SerializeField] Color[] chanelColors;

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

    public Color GetChannelColor(int channel)
    {
        return chanelColors[channel%chanelColors.Length];
    }
}
