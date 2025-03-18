using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IActivator
{
    [SerializeField] float activationWeight = 0.1f;

    //private HashSet<Rigidbody> objectsForTrigger = new HashSet<Rigidbody>();

    public ActivationCaller ActivationCaller { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Triggered");
            ActivationCaller.SendActivation();

            //objectsForTrigger.Add(rb);
            //float totalWeight = TotalWeightOnTrigger();
            //if(totalWeight>=activationWeight)
            //{
            //    ActivationCaller.SendActivation();
            //}

        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    Rigidbody rb = other.GetComponent<Rigidbody>();
    //    if (rb != null)
    //    {
    //        objectsForTrigger.Remove(rb);
    //        TotalWeightOnTrigger();
    //    }
    //}
    //private float TotalWeightOnTrigger()
    //{
    //    float totalWeight = 0f;
    //    foreach (Rigidbody rb in objectsForTrigger)
    //    {
    //        totalWeight += rb.mass;
    //    }
    //    return totalWeight;
    //}
}
