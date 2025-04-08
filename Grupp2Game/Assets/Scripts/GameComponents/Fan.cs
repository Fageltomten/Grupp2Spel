using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour, IActivatable
{
    [SerializeField] private float force = 30;
    [SerializeField] private float windRange = 10;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Transform basePoint;
    private Transform[] fanParts;
    private Vector3 currentGravity;
    private bool isActivated = false;
    private CapsuleCollider capsuleCollider;

    private Color channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f);
    private Color deactivatedColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    private Color activatedColor = new Color(128f / 255f, 255f / 255f, 0f / 255f);

    void IActivatable.Activate()
    {
        isActivated = true;
        SetColor(basePoint, activatedColor);
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        SetColor(basePoint, deactivatedColor);
    }
    private void Start()
    {
        channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f);
        currentGravity = Physics.gravity;

        player = GameObject.Find("Player ");//need space after, should fix but not doing that right now
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        capsuleCollider = GetComponent<CapsuleCollider>();
        SetWindTunnel();

        basePoint = transform.Find("Blades");
        SetColor(basePoint, deactivatedColor);

        fanParts = new Transform[]
        {
            transform.Find("Center"),
            transform.Find("Base")
        };
        foreach (Transform part in fanParts)
        {
            SetColor(part, channelColor);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated && other.TryGetComponent(out Rigidbody rb))
        {
            rb.linearDamping = 2f;
        }  
    }
    private void OnTriggerStay(Collider other)
    {
        if (isActivated)
        {
            float distanceFromBasePoint = Vector3.Distance(basePoint.position, other.transform.position);
            float distancechange = (1f - (distanceFromBasePoint / windRange));

            Vector3 appliedForce = transform.up * ((-currentGravity.y * 1.2f) + force * distancechange);
            playerMovement.AddForce(appliedForce, ForceMode.Force);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isActivated && other.TryGetComponent(out Rigidbody rb))
        {
            rb.linearDamping = 0f;
        }
    }
    private void SetColor(Transform part, Color color)
    {
        if (part != null)
        {
            part.GetComponent<Renderer>().material.color = color;
        } 
    }
    private void SetWindTunnel()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.height = windRange;
            capsuleCollider.center = new Vector3(0, windRange / 2, 0);
        }
    }
}
