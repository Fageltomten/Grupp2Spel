using UnityEngine;
/// <summary>
/// component that takes two inputs and makes an output depending on gate type
/// author carl åslund
/// </summary>
public class ActivationGate : MonoBehaviour, IActivator
{
    public enum GateTypes { And, Or, Xor };

    public ActivationCaller ActivationCaller { get; set; }
    [Tooltip("which logic gate to use")]
    [SerializeField] GateTypes logicType;
    [Tooltip("If inverse output")]
    [SerializeField] bool not = false;
    [SerializeField] ActivationListener leftListener;
    [SerializeField] ActivationListener rightListener;

    public void Awake()
    {
        UpdateValues();
        Debug.Log("update");
    }

    public void UpdateValues()
    {
        // collect values 
        bool left = leftListener.IsActivated;
        bool right = rightListener.IsActivated;

        // compare values
        bool output = GetOutput(left, right);
        if (not) { output = !output; }

        // send output
        if (output)
        {
            ActivationCaller.SendActivation();
        } else
        {
            ActivationCaller.SendDeactivation();
        }

    }

    bool GetOutput(bool left, bool right)
    {
        bool output = false;

        switch (logicType)
        {
            case GateTypes.And:
                return (left && right);
                break;
            case GateTypes.Or:
                return (left || right);
                break;
            case GateTypes.Xor:
                return (left ^ right);
                break;
        }

        return output;
    } 




}



