using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour, IActivatable
{
    [SerializeField] private float force = 30;
    [SerializeField] private float windRange = 10;
    [SerializeField] private Transform basePoint;

    private bool isActivated = false;
    private CapsuleCollider capsuleCollider;

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
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null )
        {
            capsuleCollider.height = windRange;
            capsuleCollider.center = new Vector3 ( 0, windRange/2, 0 );
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated == true)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = 2f;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isActivated == true)
        {

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = 0f;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isActivated == true)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float distanceFromBasePoint = Vector3.Distance(basePoint.position, other.transform.position);
                float distancechange = (1f - (distanceFromBasePoint / windRange)); //fix better math for this

                Vector3 appliedForce = transform.up * force * distancechange;
                Debug.Log($"Applied Force: {appliedForce}");
                rb.AddForce(appliedForce);

                //rb.AddForce(transform.up * force * distancechange);

            }
        }
    }
}
