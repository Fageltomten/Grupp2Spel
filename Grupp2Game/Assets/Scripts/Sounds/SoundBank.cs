using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A class for storing and getting sounds
/// Author Clara Lönnkrans
/// </summary>
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
        int i = Random.Range(0, s.sound.Length);
        return s?.sound[i];
    }
    public AudioClip GetSFXSound(string name)
    {
        Sound s = Array.Find(enviromentSounds, x => x.soundName == name);
        int i = Random.Range(0, s.sound.Length);
        return s?.sound[i];
    }
}
