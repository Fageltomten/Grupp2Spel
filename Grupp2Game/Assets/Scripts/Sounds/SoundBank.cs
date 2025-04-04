using System;
using UnityEngine;

//Author Clara Lönnkrans
public class SoundBank : MonoBehaviour
{
    public Sound[] playerSounds, enviromentSounds;
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
    public AudioClip GetPlayerSound(string name)
    {
        Sound s = Array.Find(playerSounds, x => x.soundName == name);
        return s?.sound;
    }
    public AudioClip GetSFXSound(string name)
    {
        Sound s = Array.Find(enviromentSounds, x => x.soundName == name);
        return s?.sound;
    }
}
