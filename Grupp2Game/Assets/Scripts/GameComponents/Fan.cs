using UnityEngine;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour, IActivatable
{
    [SerializeField] private float force = 30;
    [SerializeField] private float windRange = 10;

    private PlayerMovement playerMovement;
    private Transform basePoint;
    private Vector3 currentGravity;
    private bool isActivated = false;
    private CapsuleCollider capsuleCollider;
    Animator animator;
    ParticleSystem windParticles;



    void IActivatable.Activate()
    {
        isActivated = true;
        animator.SetBool("isActive", true);
        windParticles.Play();
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        animator.SetBool("isActive", false);
        windParticles.Stop();
    }
    private void Start()
    {
        currentGravity = Physics.gravity;
        animator = GetComponent<Animator>();
        basePoint = transform.Find("Blades");
        windParticles = transform.Find("WindParticle").GetComponent<ParticleSystem>();

        playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.Log("OH ´no player is null");
        }

        capsuleCollider = GetComponent<CapsuleCollider>();
        SetWindTunnel();
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
    private void SetWindTunnel()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.height = windRange;
            capsuleCollider.center = new Vector3(0, windRange / 2, 0);
        }
    }
}
