using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Clara Lönnkrans
public class ButtonScript : MonoBehaviour, IActivator
{
    [SerializeField] float activationWeight = 0.1f;

    private Transform  movingPart, basePart;
    private Transform[] activationIndicatorParts;
    private float movingPartStartPos;
    private bool atBottom = false;
    private bool isActivated = false;

    private Color channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f);
    private Color deactivatedColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    private Color activatedColor = new Color(128f / 255f, 255f / 255f, 0f / 255f);

    private HashSet<Rigidbody> objectsOnTrigger = new HashSet<Rigidbody>();

    public ActivationCaller ActivationCaller { get; set; }

    private void Start()
    {

        movingPart = transform.Find("Button");
        basePart = transform.Find("Base");
        activationIndicatorParts = new Transform[]
        {
            transform.Find("MiddlePart"),
            movingPart.Find("Text")
        };
        movingPart = transform.Find("Button");
        if (movingPart != null)
        {
            movingPartStartPos = movingPart.localPosition.y;
        }


        SetColor(basePart, channelColor);
        SetColor(movingPart, Color.black);
        foreach (Transform part in activationIndicatorParts)
        {
            SetColor(part, deactivatedColor);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && movingPart.localPosition.y > movingPartStartPos - 0.33f)
        {
            objectsOnTrigger.Add(rb);
            float weightOnTrigger = TotalWeightOnTrigger();
            if(weightOnTrigger>= activationWeight)
            {
                movingPart.localPosition -= new Vector3(0, 0.5f * Time.deltaTime, 0);
            }
            
        }
        if(movingPart.localPosition.y <= (movingPartStartPos - 0.30f))
        {
            movingPart.localPosition = new Vector3(0, (movingPartStartPos - 0.30f), 0);
            if(!atBottom)
            {
                
                if (isActivated)
                {
                    //Debug.Log("isnotActivated: ");
                    isActivated = false;
                    ActivationCaller.SendDeactivation();
                    foreach (Transform part in activationIndicatorParts)
                    {
                        SetColor(part, deactivatedColor);
                    }
                }
                else
                {
                    isActivated = true;
                    ActivationCaller.SendActivation();
                    foreach (Transform part in activationIndicatorParts)
                    {
                        SetColor(part, activatedColor);
                    }
                }
                atBottom = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        atBottom = false;
        StartCoroutine(MoveUp());
    }
    private IEnumerator MoveUp()
    {
        while (!atBottom && movingPart.localPosition.y < movingPartStartPos)
        {
            movingPart.localPosition += new Vector3(0, 0.5f * Time.deltaTime, 0);
            yield return null;
        }
    }
    private float TotalWeightOnTrigger()
    {
        float totalWeight = 0f;
        foreach (Rigidbody rb in objectsOnTrigger)
        {
            totalWeight += rb.mass;
        }
        return totalWeight;
    }
    private void SetColor(Transform part, Color color)
    {
        if (part != null)
        {
            part.GetComponent<Renderer>().material.color = color;
        }
    }
}
