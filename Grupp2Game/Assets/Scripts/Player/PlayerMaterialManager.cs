using System;
using UnityEngine;

// Author: Hugo Clarke
/// <summary>
/// The changing of the emission material for the player
/// </summary>
public class PlayerMaterialManager : MonoBehaviour
{
    [SerializeField] private GameObject body;

    [SerializeField] private Color[] Colors;

    private int currentColorIndex = 0;
    private int lastColorIndex = 0;
    [SerializeField] private float colorChangeTime = 1f;
    [SerializeField] private float EmissionStrength = 10f;
    private float t = 0f;
    
    void Update()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        if(t > colorChangeTime)
        {
            lastColorIndex = currentColorIndex;
            currentColorIndex = (currentColorIndex + 1) % Colors.Length;
            t = 0f;
        }
        t += Time.deltaTime;
        Color color = Color.Lerp(Colors[lastColorIndex], Colors[currentColorIndex], t / colorChangeTime);
        body.GetComponent<MeshRenderer>().materials[2].SetColor("_EmissionColor", color * EmissionStrength);
        GetComponent<LineRenderer>().material.SetColor("_EmissionColor", color * EmissionStrength);
    }
}
