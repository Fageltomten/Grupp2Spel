using UnityEngine;
using UnityEngine.InputSystem;

//Author Clara Lönnkrans
public class PlayerSounds : MonoBehaviour
{
    private AudioSource audioSource;
    private InputAction jumpInput;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        jumpInput = InputSystem.actions.FindAction("Jump");
        jumpInput.performed += _ => OnJump();
    }
    private void OnJump()
    {
        AudioClip jumpSound = SoundBank.Instance.GetPlayerSound("Jump");
        if(jumpSound == null )
        {
            Debug.Log("jumpsound not found");
            return;
        }
        if (audioSource.isPlaying && audioSource.clip == jumpSound)
        {
            return;
        }
        //audioSource.Stop();
        audioSource.clip = jumpSound;
        audioSource.loop = false;
        audioSource.Play();

    }
}
