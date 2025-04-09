using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class by Carl Åslund
public class ActivationListener : ActivationAgent
{

    event Action OnActivation;
    event Action OnDeactivation;

    public bool IsActivated { get; private set; }
    public bool IsListening { get; set; }

    [Header("Activation Listener Settings")]
    [SerializeField] bool startActivated = false;
    [SerializeField] bool startListening = true;
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
