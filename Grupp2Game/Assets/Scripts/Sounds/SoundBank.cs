using UnityEngine;

//Author Clara Lönnkrans
public class SoundBank : MonoBehaviour
{
    //PlayerSounds
    public AudioClip stepSound;
    public AudioClip jumpSound;
    public AudioClip swingSound;
    public AudioClip pickUpSound;

    //OtherSounds
    public AudioClip fanSound;
    public AudioClip launchpadSound;
    public AudioClip doorSound;
    public AudioClip portalSound;

    public static SoundBank Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
