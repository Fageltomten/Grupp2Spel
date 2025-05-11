using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Clara Lönnkrans
public class ButtonScript : MonoBehaviour, IActivator, IActivatable
{
    /// <summary>
    /// A class for button beahavior
    /// </summary>
    [SerializeField] float activationWeight = 0.1f;
    [SerializeField] float moveSpeed = 1f;

    private Transform  movingPart;
    private float movingPartStartPos;
    private bool atBottom = false;
    private bool isActivated = false;


    private HashSet<Rigidbody> objectsOnTrigger = new HashSet<Rigidbody>();

    public ActivationCaller ActivationCaller { get; set; }

    void IActivatable.Activate()
    {
        isActivated = true;
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
    }
    private void Start()
    {
        movingPart = transform.Find("Button");
        if (movingPart != null)
        {
            movingPartStartPos = movingPart.localPosition.y;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && movingPart.localPosition.y > movingPartStartPos - 0.33f)
        {
            objectsOnTrigger.Add(rb);
            float weightOnTrigger = TotalWeightOnTrigger();
            if (weightOnTrigger >= activationWeight)
            {
                movingPart.localPosition -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }

        }
        if (movingPart.localPosition.y <= (movingPartStartPos - 0.30f))
        {
            movingPart.localPosition = new Vector3(0, (movingPartStartPos - 0.30f), 0);
            if(!atBottom)
            {
                
                if (isActivated)
                {
                    ActivationCaller.SendDeactivation();
                }
                else
                {
                    ActivationCaller.SendActivation();
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
        while (movingPart.localPosition.y < movingPartStartPos)
        {
            movingPart.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }
    private IEnumerator MoveDown()
    {
        while (!atBottom && movingPart.localPosition.y > movingPartStartPos - 0.33f)
        {
            movingPart.localPosition -= new Vector3(0, 0.5f * Time.deltaTime, 0);
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
