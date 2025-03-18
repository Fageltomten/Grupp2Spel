using UnityEngine;

//Author Clara Lönnkrans
public class Launchpad : MonoBehaviour
{
    [SerializeField] private float launchForce = 200f; //Added a variable for force
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //New Code
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(transform.up * launchForce, ForceMode.Impulse); //Make it launch in the up direction of the launchpad

            //Old code 
           // rb.AddForce(0, launchForce, 0);
        }
    }
}
