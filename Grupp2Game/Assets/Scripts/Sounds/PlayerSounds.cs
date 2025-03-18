using UnityEngine;
using UnityEngine.InputSystem;

//Author Clara Lönnkrans
public class PlayerSounds: MonoBehaviour
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
        if (audioSource.clip != SoundBank.Instance.jumpSound)
        {
            audioSource.clip = SoundBank.Instance.jumpSound;
            audioSource.loop = false;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
