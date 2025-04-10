using UnityEngine;

// Author Carl Åslund
public abstract class ActivationAgent : MonoBehaviour
{

    Color channelColor;
    [Header("Activation Settings")]
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
