using UnityEngine;

//Author Clara Lönnkrans
public class Launchpad : MonoBehaviour, IActivatable
{
    /// <summary>
    /// A class for launchpad beahavior
    /// </summary>
    [SerializeField] private float force = 30;

    private PlayerMovement playerMovement;
    private bool isActivated = false;
    void IActivatable.Activate()
    {
        isActivated = true;
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
    }
    private void Start()
    {
        playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
       if(isActivated)
        {
            playerMovement.ResetVerticalVelocity();
            Vector3 appliedForce = transform.up *  force;
            playerMovement.AddForce(appliedForce, ForceMode.Impulse);
        }
    }
}
