using UnityEngine;

// author Carl Åslund
/// <summary>
/// Component that changes color if items when activated and deactivated
/// </summary>
[RequireComponent(typeof(ActivationListener))]
public class ColorChangeActivation : MonoBehaviour, IActivatable
{
    [Tooltip("Color when activated.")]
    [SerializeField] Color activatedColor = new Color(0, 1, 0);

    [Tooltip("Color when deactivated")]
    [SerializeField] Color deactivatedColor = new Color(1, 0, 0);

    [Tooltip("List of transforms for the objects that will change color")]
    [SerializeField] Transform[] objectsToChange;

    public void Start()
    {
        if (GetComponent<ActivationListener>().IsActivated)
        {
            Activate();
        }
        else 
        {
            Deactivate();
        }
    }
    public void Activate()
    {
        ChangeColor(activatedColor);
    }

    public void Deactivate()
    {
        ChangeColor(deactivatedColor);
    }

    void ChangeColor(Color changeColor)
    {
        if (objectsToChange.Length > 0)
        {
            foreach (Transform t in objectsToChange)
            {
                t.GetComponent<Renderer>().material.color = changeColor;
            }
        }
    }
}
