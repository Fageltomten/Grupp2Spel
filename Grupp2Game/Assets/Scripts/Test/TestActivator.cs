using System;
using UnityEngine;

public class testActivator : MonoBehaviour, IActivator
{
    public ActivationCaller ActivationCaller { get; set; }

    [SerializeField] KeyCode activationKey;
    [SerializeField] KeyCode deactivationKey;
    [SerializeField] string activatorName;
    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            Debug.Log("Sending Activation from " + activatorName);
            ActivationCaller.SendActivation();
            
        }

        if (Input.GetKeyDown(deactivationKey))
        {
            Debug.Log("Sending Deactivation from " + activatorName);
            ActivationCaller.SendDeactivation();
        }
    }

}
