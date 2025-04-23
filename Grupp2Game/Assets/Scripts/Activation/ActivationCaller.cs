using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class by Carl Åslund
/// <summary>
/// Component that has logic to send activation and deactivation requests on a channel to the activationmanager
/// </summary>

[DisallowMultipleComponent]
public class ActivationCaller : ActivationAgent
{
    
    protected override void Start()
    {
        base.Start();
        List<IActivator> activators = GetComponents<IActivator>().ToList();

        foreach (IActivator activator in activators)
        {
            activator.ActivationCaller = this;
        }
    }

    public void SendActivation()
    {
        ActivationManager.Instance.Activate(channel);
    }
    public void SendDeactivation()
    {
        ActivationManager.Instance.Deactivate(channel);
    }

    
}
