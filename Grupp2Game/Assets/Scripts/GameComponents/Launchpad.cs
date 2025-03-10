using UnityEngine;

//Author Clara Lönnkrans
public class Launchpad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(0, 700, 0);
        }
    }
}
