using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour, IActivatable
{
    [SerializeField] private float force = 30;
    [SerializeField] private float windRange = 10;
    [SerializeField] private Transform basePoint;
    private Vector3 currentGravity;

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
        currentGravity = Physics.gravity;
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null )
        {
            capsuleCollider.height = windRange;
            capsuleCollider.center = new Vector3 ( 0, windRange/2, 0 );
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = 2f;
               // Debug.LogWarning(rb.linearDamping);
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (isActivated)
        {

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = 0f;
                //Debug.LogWarning(rb.linearDamping);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isActivated)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float distanceFromBasePoint = Vector3.Distance(basePoint.position, other.transform.position);
                float distancechange = (1f - (distanceFromBasePoint / windRange)); //fix better math for this

                Vector3 appliedForce = transform.up * ((-currentGravity.y * 1.2f) + force * distancechange);
                Debug.Log($"Applied Force: {appliedForce}");
                rb.AddForce(appliedForce);



            }
        }
    }
}
