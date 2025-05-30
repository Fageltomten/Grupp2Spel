using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonScript : MonoBehaviour, IActivator, IActivatable
{
    /// <summary>
    /// A class for button beahavior
    /// 
    /// Author Clara Lönnkrans
    /// </summary>
    [SerializeField] float moveSpeed = 1f;

    private AudioSource audioSource;

    private Transform  movingPart;
    private float movingPartStartPos;
    private bool atBottom = false;
    private bool isActivated = false;
    private AudioClip click;

    public ActivationCaller ActivationCaller { get; set; }

    void IActivatable.Activate()
    {
        isActivated = true;
        audioSource.clip = click;
        audioSource.loop = false;
        audioSource.Play();
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        audioSource.clip = click;
        audioSource.loop = false;
        audioSource.Play();
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null )
        {
            Console.WriteLine("no audiosource")
;       }
        
    }
    private void Start()
    {
        click = SoundBank.Instance.GetSFXSound("ButtonActivation");
        movingPart = transform.Find("Button");
        if (movingPart != null)
        {
            movingPartStartPos = movingPart.localPosition.y;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && movingPart.localPosition.y > movingPartStartPos - 0.33f)
        {
            movingPart.localPosition -= new Vector3(0, moveSpeed * Time.deltaTime, 0);

        }
        if (movingPart.localPosition.y <= (movingPartStartPos - 0.30f))
        {
            movingPart.localPosition = new Vector3(0, (movingPartStartPos - 0.30f), 0);
            if(!atBottom)
            {
                
                if (isActivated)
                {
                    ActivationCaller.SendDeactivation();
                }
                else
                {
                    ActivationCaller.SendActivation();
                }
                atBottom = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        atBottom = false;
        StartCoroutine(MoveUp());
    }
    private IEnumerator MoveUp()
    {
        while (movingPart.localPosition.y < movingPartStartPos)
        {
            movingPart.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }
}
