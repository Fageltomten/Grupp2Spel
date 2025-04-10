using UnityEngine;

// author Carl Åslund
public class ColorChangeActivation : MonoBehaviour, IActivatable
{
    [SerializeField] Color activatedColor = new Color(0, 1, 0);
    [SerializeField] Color deactivatedColor = new Color(1, 0, 0);

    [SerializeField] Transform[] objectsToChange;

    public void Start()
    {
        Deactivate();
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
