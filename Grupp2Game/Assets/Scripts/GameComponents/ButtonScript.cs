using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Clara Lönnkrans
public class ButtonScript : MonoBehaviour, IActivator
{
    [SerializeField] float activationWeight = 0.1f;

    private Transform  movingPart, basePart, middlePart, textPart;
    private float movingPartStartPos;
    private bool atBottom = false;
    private bool isActivated = false;
    private Color channelColor, activatedColor,deactivatedColor;

    private HashSet<Rigidbody> objectsOnTrigger = new HashSet<Rigidbody>();

    public ActivationCaller ActivationCaller { get; set; }

    private void Start()
    {
        movingPart = transform.Find("Button");
        textPart = movingPart.Find("Text");
        basePart = transform.Find("Base");
        middlePart = transform.Find("MiddlePart");
        channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f); ;
        deactivatedColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
        activatedColor = new Color(128f / 255f, 255f / 255f, 0f / 255f);


        if (movingPart != null)
        {
            textPart.GetComponent<Renderer>().material.color = deactivatedColor;
            middlePart.GetComponent<Renderer>().material.color = deactivatedColor;
            movingPart.GetComponent<Renderer>().material.color = Color.black;
            movingPartStartPos = movingPart.localPosition.y;
        }
        if (basePart != null)
        {
            basePart.GetComponent<Renderer>().material.color = channelColor;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && movingPart.localPosition.y > movingPartStartPos - 0.24f)
        {
            objectsOnTrigger.Add(rb);
            float weightOnTrigger = TotalWeightOnTrigger();
            if(weightOnTrigger>= activationWeight)
            {
                movingPart.localPosition -= new Vector3(0, 0.5f * Time.deltaTime, 0);
            }
            
        }
        if(movingPart.localPosition.y <= (movingPartStartPos - 0.21f))
        {
            movingPart.localPosition = new Vector3(0, (movingPartStartPos - 0.21f), 0);
            if(!atBottom)
            {
                
                if (isActivated)
                {
                    Debug.Log("isnotActivated: ");
                    isActivated = false;
                    ActivationCaller.SendDeactivation();
                    textPart.GetComponent<Renderer>().material.color = deactivatedColor;
                    middlePart.GetComponent<Renderer>().material.color = deactivatedColor;
                }
                else
                {
                    isActivated = true;
                    ActivationCaller.SendActivation();
                    textPart.GetComponent<Renderer>().material.color = activatedColor;
                    middlePart.GetComponent<Renderer>().material.color = activatedColor;
                    Debug.Log("isActivated: ");
                }
                Debug.Log("isActivated: " + isActivated);
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
}
