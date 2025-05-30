using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSounds : MonoBehaviour
{
    /// <summary>
    /// A class whos methods can be called for playing different player sounds and dash effects
    /// Author Clara Lönnkrans
    /// </summary>

    private AudioSource audioSource;
    [SerializeField] ParticleSystem dashParticle;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void DashParticles()
    {
        dashParticle.Play();
    }
    public void JumpSound()
    {
        AudioClip jumpSound = SoundBank.Instance.GetPlayerSound("Jump");
        if(jumpSound == null )
        {
            Debug.Log("jumpsound not found");
            return;
        }
        if (audioSource.isPlaying)
        {
            return;
        }
        audioSource.clip = jumpSound;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void GrapplinghookSound()
    {
        AudioClip shootSound = SoundBank.Instance.GetPlayerSound("GrapplingHook");
        if (shootSound == null)
        {
            Debug.Log("hooksound not found");
            return;
        } 
        audioSource.loop = false;
        audioSource.PlayOneShot(shootSound, 0.3f);
    }
    public void DashSound(int randomRange)
    {
        AudioClip dashSound = SoundBank.Instance.GetPlayerSound("Dash");
        int random = Random.Range(1, randomRange);
        if (dashSound == null)
        {
            Debug.Log("dashsound not found");
            return;
        }
        if (audioSource.isPlaying)
        {
            return;
        }
        if (random == 1)
        {
            audioSource.clip = dashSound;
            audioSource.loop = false;
            audioSource.Play();
        }
       
    }
}
