using UnityEngine;
using UnityEngine.InputSystem;

//Author Clara Lönnkrans
public class PlayerSounds : MonoBehaviour
{
    /// <summary>
    /// A class whos methods can be called for playing different player sounds
    /// </summary>
    
    //Note! Check if we want to do like this(calling these methods from other scripts, easier bc wont have to do all the checks here too
    private AudioSource audioSource;
    //private InputAction jumpInput;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //jumpInput = InputSystem.actions.FindAction("Jump");
        //jumpInput.performed += _ => OnJump();
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
    public void ShootSound()
    {
        AudioClip shootSound = SoundBank.Instance.GetPlayerSound("GrapplingHook");
        if (shootSound == null)
        {
            Debug.Log("jumpsound not found");
            return;
        }
        audioSource.clip = shootSound;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void DashSound()
    {
        AudioClip dashSound = SoundBank.Instance.GetPlayerSound("Dash");
        if (dashSound == null)
        {
            Debug.Log("jumpsound not found");
            return;
        }
        if (audioSource.isPlaying)
        {
            return;
        }
        audioSource.clip = dashSound;
        audioSource.loop = false;
        audioSource.Play();
    }
}
