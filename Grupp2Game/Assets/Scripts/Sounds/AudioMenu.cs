using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

//Author Clara Lönnkrans
public class AudioMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [Header("Other")]
    [SerializeField] TMP_Text masterValue, musicValue;

    [Header("Enviroment")]
    [SerializeField] TMP_Text ambienceValue, SFXValue;

    [Header("Player")]
    [SerializeField] TMP_Text playerValue;
    //other
    public void SetMasterVolume(float volume)
    {
        int volumeToInt = Mathf.Clamp((int)(volume * 10), 0, 20);
        masterValue.text = volumeToInt.ToString();
        audioMixer.SetFloat("MasterVolume", MathF.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        int volumeToInt = Mathf.Clamp((int)(volume * 10), 0, 20);
        musicValue.text = volumeToInt.ToString();
        audioMixer.SetFloat("musicVolume", MathF.Log10(volume) * 20);
    }
    //World
    public void SetSFXVolume(float volume)
    {
        int volumeToInt = Mathf.Clamp((int)(volume * 10), 0, 20);
        SFXValue.text = volumeToInt.ToString();
        audioMixer.SetFloat("SFXVolume", MathF.Log10(volume) * 20);
    }
    public void SetAmbienceVolume(float volume)
    {
        int volumeToInt = Mathf.Clamp((int)(volume * 10), 0, 20);
        ambienceValue.text = volumeToInt.ToString();
        audioMixer.SetFloat("aAmbienceVolume", MathF.Log10(volume) * 20);
    }
    //Player
    public void SetplayerVolume(float volume)
    {
        int volumeToInt = Mathf.Clamp((int)(volume * 10), 0, 20);
        playerValue.text = volumeToInt.ToString();
        audioMixer.SetFloat("playerVolume", MathF.Log10(volume) * 20);
    }
}