using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IActivator
{
    [SerializeField] float activationWeight = 0.1f;
    [SerializeField] Transform  movingPart;
    private float movingPartStartPos;
    private bool atBottom = false;

    private HashSet<Rigidbody> objectsOnTrigger = new HashSet<Rigidbody>();

    public ActivationCaller ActivationCaller { get; set; }

    private void Start()
    {
        movingPartStartPos = movingPart.localPosition.y;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && movingPart.localPosition.y >(-0.1))
        {
            objectsOnTrigger.Add(rb);
            float weightOnTrigger = TotalWeightOnTrigger();
            if(weightOnTrigger>= activationWeight)
            {
                movingPart.localPosition -= new Vector3(0, 0.5f * Time.deltaTime, 0);
            }
            
        }
        if(movingPart.localPosition.y < (-0.1))
        {
            movingPart.localPosition = new Vector3(0, (-0.1f), 0);
        }
        if(movingPart.localPosition.y <= (-0.1))
        {
            ActivationCaller.SendActivation();
            atBottom = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (!atBottom)
        {
            StartCoroutine(MoveUp());
        }
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
