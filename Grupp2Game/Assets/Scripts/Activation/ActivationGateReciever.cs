using UnityEngine;

/// <summary>
/// Component that takes input from activation system and sends it forward to a gate
/// Author Carl Åslund
/// </summary>
public class ActivationGateReciever : MonoBehaviour, IActivatable
{

    [Tooltip("Gate to send to")]
    [SerializeField] ActivationGate gate;

    public void Activate()
    {
        gate.UpdateValues();
    }

    public void Deactivate()
    {
        gate.UpdateValues();   
    }

    
}
