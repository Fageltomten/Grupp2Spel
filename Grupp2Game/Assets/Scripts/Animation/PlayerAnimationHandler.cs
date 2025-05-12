using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Commponent that chacks distance to ground to send to animation
/// Can disable and enable legs
/// Author: Carl Åslund
/// </summary>
public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] float groundCheckWidth;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Assign References")]
    [SerializeField] GameObject[] legs;
    
    
    Animator animator;
    PlayerMovement movement;
    [SerializeField] PlayerFootTarget[] playerFootTargets;
    [SerializeField] ParticleSystem disapearParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();   
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.IsGrounded())
        {
            LegsMove();
            ActivateLegs();
        }
        else
        {
            LegsStill();
           DeactivateLegs();
        }
    }

    public void ActivateLegs()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            legs[i].SetActive(true);
        }
    }

    public void DeactivateLegs()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            legs[i].SetActive(false);
        }
    }

    public void LegsMove()
    {
        for (int i = 0; i < playerFootTargets.Length; i++)
        {
            playerFootTargets[i].IsActive = true;
        }
    }
    
    public void LegsStill()
    {
        for (int i = 0; i < playerFootTargets.Length; i++)
        {
            playerFootTargets[i].IsActive = false;
        }
    }
}
