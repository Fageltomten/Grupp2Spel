using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class by Carl Åslund
/// <summary>
/// A component that recieves requests from the activationmanager 
/// and notifies all components on gameobject of type IActivatable
/// </summary>
public class ActivationListener : ActivationAgent
{

    event Action OnActivation;
    event Action OnDeactivation;

    public bool IsActivated { get; private set; }
    public bool IsListening { get; set; }

    [Header("Activation Listener Settings")]
    [Tooltip("If listener should activate when started.")]
    [SerializeField] bool startActivated = false;

    [Tooltip("If listener should be able to recieve requests.")]
    [SerializeField] bool startListening = true;

    [Tooltip("If listener should listen on Caller on the same Gameobject.")]
    [SerializeField] bool autoListenOnCaller = false;
    protected override void Start()
    {
        base.Start();
        IsActivated = false;
        IsListening = startListening;

        if (autoListenOnCaller)
        {
            ChangeChannelToCaller();
        }

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

    void ChangeChannelToCaller() 
    { 
        ActivationCaller caller = GetComponent<ActivationCaller>();
        if (caller != null)
        {
            channel = caller.channel;
        }
    }

    void ReceiveActivationRequest(int recievedChannel)
    {
        if (IsListening && recievedChannel == channel)
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

    void ReceiveDeactivationRequest(int recievedChannel)
    {
        if (IsListening && recievedChannel == channel)
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
