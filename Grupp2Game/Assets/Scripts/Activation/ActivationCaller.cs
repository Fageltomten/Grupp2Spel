using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class by Carl Åslund
public class ActivationCaller : MonoBehaviour
{
    [Header("Activation Caller Settings")]
    [SerializeField] int CallingChannel;

    private void Start()
    {
        List<IActivator> activators = GetComponents<IActivator>().ToList();

        foreach (IActivator activator in activators)
        {
            activator.ActivationCaller = this;
        }
    }

    public void SendActivation()
    {
        ActivationManager.instance.Activate(CallingChannel);
    }
    public void SendDeactivation()
    {
        ActivationManager.instance.Deactivate(CallingChannel);
    }
}
