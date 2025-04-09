using UnityEngine;

//Author Clara Lönnkrans
public class Launchpad : MonoBehaviour, IActivatable
{
    [SerializeField] private float force = 30;

    private Transform jumpPart;
    private Transform basePart;
    private bool isActivated = false;
    private Color channelColor;
    private Color activatedColor;
    private Color deactivatedColor;
    void IActivatable.Activate()
    {
        isActivated = true;
        jumpPart.GetComponent<Renderer>().material.color = activatedColor;
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        jumpPart.GetComponent<Renderer>().material.color = deactivatedColor;
    }
    private void Start()
    {
        basePart = transform.Find("Bottom");
        jumpPart = transform.Find("Top");
        channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f); ;
        deactivatedColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
        activatedColor = new Color(128f / 255f, 255f / 255f, 0f / 255f);

        if (basePart != null)
        {
            basePart.GetComponent<Renderer>().material.color = channelColor;
        }
        if (jumpPart != null)
        {
            jumpPart.GetComponent<Renderer>().material.color = deactivatedColor;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       if(isActivated)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if(rb != null )
            {
                rb.AddForce(transform.up * force, ForceMode.Impulse);
            }
        }
    }
}
