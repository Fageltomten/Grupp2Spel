using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour
{
    private float upForce = 15f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.linearDamping = 2f;
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.linearDamping = 0f;
    }
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null )
        {
            rb.AddForce(0, 30, 0);
            
        }
    }
}
