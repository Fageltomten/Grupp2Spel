using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour
{
    [SerializeField] private float upForce = 30f;
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearDamping = 2f;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearDamping = 0f;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null )
        {
            //New Code
            rb.AddForce(transform.up * upForce);

            //Old code 
            //rb.AddForce(0, 30, 0); You didn't use your upForce  variable?

        }
    }
}
