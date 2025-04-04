using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour, IActivatable
{
    [SerializeField] private float force = 30;
    [SerializeField] private float windRange = 10;

    private Transform basePoint;
    private Transform[] fanParts;
    private Vector3 currentGravity;
    private bool isActivated = false;
    private CapsuleCollider capsuleCollider;
    private Color channelColor;
    private Color activatedColor;
    private Color deactivatedColor;

    void IActivatable.Activate()
    {
        isActivated = true;
        basePoint.GetComponent<Renderer>().material.color = activatedColor;
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        basePoint.GetComponent<Renderer>().material.color = deactivatedColor;
    }
    private void Start()
    {
        currentGravity = Physics.gravity;
        basePoint = transform.Find("Blades");
        capsuleCollider = GetComponent<CapsuleCollider>();
        channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f); ;
        deactivatedColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
        activatedColor = new Color(128f / 255f, 255f / 255f, 0f / 255f);

        fanParts = new Transform[]
        {
            transform.Find("Center"),
            transform.Find("Base")
        };

        if (capsuleCollider != null )
        {
            capsuleCollider.height = windRange;
            capsuleCollider.center = new Vector3 ( 0, windRange/2, 0 );
        }
        if(basePoint != null)
        {
            basePoint.GetComponent<Renderer>().material.color = deactivatedColor;
        }
        foreach ( Transform parts in fanParts )
        {
            if ( parts != null )
            {
                parts.GetComponent<Renderer>().material.color = channelColor;
            }
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
                float distancechange = (1f - (distanceFromBasePoint / windRange));

                Vector3 appliedForce = transform.up * ((-currentGravity.y * 1.2f) + force * distancechange);
                //Debug.Log($"Applied Force: {appliedForce}");
                rb.AddForce(appliedForce);
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
            }
        }
    }
}
