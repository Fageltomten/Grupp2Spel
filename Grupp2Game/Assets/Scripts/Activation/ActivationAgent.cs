using UnityEngine;

// Author Carl �slund
/// <summary>
/// Class that holds a channel for derived classes to use
/// Also has logic to change items to the color of that channel
/// </summary>
public abstract class ActivationAgent : MonoBehaviour
{

    Color channelColor;
    [Header("Activation Settings")]
    [Tooltip("Channel to use")]
    [SerializeField] public int channel;
    [SerializeField] Transform[] objectsToHaveChannelColor;

    protected virtual void Start()
    {
        GetChannelColor();
        RecolorToChannelColor();
    }

    void GetChannelColor()
    {
        channelColor = ActivationManager.Instance.GetChannelColor(channel);
    }

    void RecolorToChannelColor()
    {
        if (objectsToHaveChannelColor != null)
        {
            foreach (Transform t in objectsToHaveChannelColor) 
            {
                t.GetComponent<Renderer>().material.color = channelColor;
            }
        }
    }

}
