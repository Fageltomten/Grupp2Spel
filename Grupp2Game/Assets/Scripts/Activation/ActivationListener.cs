using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class by Carl Åslund
public class ActivationListener : MonoBehaviour
{

    event Action OnActivation;
    event Action OnDeactivation;

    public bool IsActivated { get; private set; }
    public bool IsListening { get; set; }

    [Header("Activation Listener Settings")]
    [SerializeField] int listeningChannel;
    [SerializeField] bool startActivated = false;
    [SerializeField] bool startListening = true;

    void Start()
    {
        IsActivated = false;
        IsListening = startListening;

        List<IActivatable> activatables = GetComponents<IActivatable>().ToList();

        foreach (IActivatable activatable in activatables) 
        {
            OnActivation += activatable.Activate;
            OnDeactivation += activatable.Deactivate;
        }

        if (startActivated)
        {
            ActivateObject();
        }

        ActivationManager.Instance.OnActivation += ReceiveActivationRequest;
        ActivationManager.Instance.OnDeactivate += ReceiveDeactivationRequest;

    }

    void ReceiveActivationRequest(int channel)
    {
        if (IsListening && channel == listeningChannel)
        {
            ActivateObject();
        }
    }

    void ActivateObject()
    {
        if (!IsActivated)
        {
            IsActivated = true;
            OnActivation.Invoke();
        }
    }

    void ReceiveDeactivationRequest(int channel)
    {
        if (IsListening && channel == listeningChannel)
        {
            DeactivateObject();
        }
    }

    void DeactivateObject()
    {
        if (IsActivated)
        {
            IsActivated = false;
            OnDeactivation.Invoke();
        }
    }

    private void OnDestroy()
    {
        ActivationManager.Instance.OnActivation -= ReceiveActivationRequest;
        ActivationManager.Instance.OnDeactivate -= ReceiveDeactivationRequest;
    }
}
