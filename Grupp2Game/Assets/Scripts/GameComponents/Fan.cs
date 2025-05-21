using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//Author Clara Lönnkrans
public class Fan : MonoBehaviour, IActivatable
{
    /// <summary>
    /// A class for fan beahavior
    /// </summary>
    /// 
    [SerializeField] private float force = 30;
    [SerializeField] private float windRange = 10;

    private PlayerMovement playerMovement;
    private Transform basePoint;
    private float colliderLenght;
    private Vector3 currentGravity;
    private bool isActivated = false;
    private CapsuleCollider capsuleCollider;
    Animator animator;
    ParticleSystem windParticles;
    [SerializeField] private AudioSource audioSource;
    private AudioClip wind, activation;



    void IActivatable.Activate()
    {
        isActivated = true;
        animator.SetBool("isActive", true);
        windParticles.Play();

        activation = SoundBank.Instance.GetSFXSound("FanActivation");
        audioSource.clip = activation;
        audioSource.loop = false;
        audioSource.Play();
        StartCoroutine(PlayWindSound(activation.length));
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        animator.SetBool("isActive", false);
        windParticles.Stop();

        activation = SoundBank.Instance.GetSFXSound("FanActivation");
        audioSource.clip = activation;
        audioSource.loop = false;
        audioSource.Play();


    }
    private void Start()
    {
        
        

        colliderLenght = windRange / transform.localScale.z;
        currentGravity = Physics.gravity;
        animator = GetComponent<Animator>();
        basePoint = transform.Find("Blades");
        windParticles = transform.Find("WindParticle").GetComponent<ParticleSystem>();

        var main = windParticles.main;
        main.startLifetime = colliderLenght / 10;

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
            capsuleCollider.height = colliderLenght;
            capsuleCollider.center = new Vector3(0, colliderLenght / 2, 0);
        }
    }
    private IEnumerator PlayWindSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        wind = SoundBank.Instance.GetSFXSound("FanWind");

        audioSource.clip = wind;
        audioSource.loop = true;
        audioSource.Play();
    }
}
