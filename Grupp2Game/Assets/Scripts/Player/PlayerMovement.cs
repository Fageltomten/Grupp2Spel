using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
/* Author: Anton Andersson och Hugo Clarke */

enum DashMode
{
    DoubleTap,
    Shift
}

// Author: Hugo Clarke
/// <summary>
/// Handles the movement of the player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 movementDirection;
    private float inputMagnitude;
    private Vector3 movementSpeed;
    private float currentSpeed;
    private bool isGrounded;
    private bool hasAirJumped;
    private GrapplingHook grapplingHook;

    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stopSpeed = 5f;
    [SerializeField] private float sidewaysStopSpeed = 5f;
    [SerializeField] private float grappleMovementMultiplier = 0.2f;

    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float grouldClearance = 0.1f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float groundStickiness = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")]
    [SerializeField] DashMode dashMode = DashMode.Shift;
    [SerializeField] float dashForce = 25f;
    [SerializeField] float delayBetweenPresses = 0.25f;
    [SerializeField] float dashCooldown = 2f;
    [SerializeField] float dashTime = 0.4f;
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = false;
    /* Used only for double tap dash */
    bool pressedFirstW = false;
    bool pressedFirstA = false;
    bool pressedFirstS = false;
    bool pressedFirstD = false;
    float lastPressedW = 0f;
    float lastPressedA = 0f;
    float lastPressedS = 0f;
    float lastPressedD= 0f;

    private InputAction movementInput;
    private InputAction jumpInput;

    private PlayerSounds playerSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        grapplingHook = GetComponent<GrapplingHook>();
        movementInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        jumpInput.performed += _ => OnJump();

        playerSounds = GetComponent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
    }

    private void FixedUpdate()
    {
        SetData();
        CheckGrounded();
        Move();
    }

    private void Move()
    {
        Vector3 forceToAdd = Vector3.zero;
        if (inputMagnitude == 0 && currentSpeed != 0 && isGrounded && !grapplingHook.IsGrappled())
        {
            if(!isDashing)
                forceToAdd -= rigidbody.linearVelocity * stopSpeed;
        }
        else if (inputMagnitude == 0)
        {
            return;
        }
        else //if (isGrounded)
        {
            Vector3 targetVelocity = (movementDirection.z * transform.forward + movementDirection.x * transform.right);
            float dottedValue = (Vector3.Dot(rigidbody.linearVelocity.normalized, movementDirection.z * transform.forward + movementDirection.x * transform.right)+1)/2 ;
            Vector3 sidewaysVelocity = Vector3.Cross(transform.up, targetVelocity) * Vector3.Dot(Vector3.Cross(transform.up, targetVelocity.normalized), rigidbody.linearVelocity);
            sidewaysVelocity = Vector3.Scale(sidewaysVelocity, Vector3.one - transform.up);
            var moveVector = Vector3.Scale(rigidbody.linearVelocity, Vector3.one - transform.up);
            var moveSpeed = moveVector.magnitude;
            float tempAcceleration = acceleration;
            if (grapplingHook.IsGrappled())
            {
                tempAcceleration = acceleration * grappleMovementMultiplier;
            }

            if(!isDashing)
                forceToAdd += (1 - Mathf.Clamp01(moveSpeed / maxSpeed) * dottedValue) * tempAcceleration * (movementDirection.z * transform.forward + movementDirection.x * transform.right);
            
            if(!grapplingHook.IsGrappled() && isGrounded && !isDashing)
                forceToAdd -= sidewaysVelocity * sidewaysStopSpeed;
            
            Debug.DrawLine(transform.position, transform.position + forceToAdd, Color.red);
            Debug.DrawLine(transform.position, transform.position + targetVelocity, Color.green);
            Debug.DrawLine(transform.position, transform.position + rigidbody.linearVelocity, Color.blue);
            Debug.DrawLine(transform.position, transform.position + (sidewaysVelocity) , Color.yellow);
        }
        forceToAdd = Vector3.Scale(forceToAdd, Vector3.one - transform.up);
        AddForce(forceToAdd, ForceMode.Acceleration);
    }

    private void CheckGrounded()
    {
        isGrounded = false;
        if (Physics.BoxCast(transform.position - transform.up * (grouldClearance/2), new(0.45f,grouldClearance/2,0.45f), -transform.up, out RaycastHit hit,quaternion.identity, 1 ,groundLayer, QueryTriggerInteraction.Ignore))
        {
            transform.position += (hit.normal * grouldClearance - hit.normal * hit.distance) * groundStickiness * Time.fixedDeltaTime;
            if(grouldClearance>= hit.distance)
            {   
                rigidbody.linearVelocity = Vector3.Scale(rigidbody.linearVelocity, Vector3.one - transform.up);
                isGrounded = true;
                hasAirJumped = false;
            }
        }
    }
    

    private void OnJump()
    {
        if(isGrounded)
        {
            hasAirJumped = false;
            ResetVerticalVelocity();
            AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerSounds.JumpSound();
        }
        else if(!hasAirJumped)
        {
            hasAirJumped = true;
            ResetVerticalVelocity();
            AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerSounds.JumpSound();
        }
    }

    private IEnumerator JumpBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        hasAirJumped = false;
    }

    private void SetData()
    {
        movementDirection = new Vector3(movementInput.ReadValue<Vector2>().x, 0, movementInput.ReadValue<Vector2>().y);
        inputMagnitude = movementDirection.magnitude;
        currentSpeed = rigidbody.linearVelocity.magnitude;
    }
    
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        if (grapplingHook.IsGrappled())
        {
            grapplingHook.AddForce(force, forceMode);
        }
        else
        {
            rigidbody.AddForce(force, forceMode);
        }
    }
    public void SetSpeed(Vector3 speed)
    {
        if (grapplingHook.IsGrappled())
        {
            grapplingHook.SetSpeed(speed);
        }
        else
        {
            rigidbody.linearVelocity = speed;
        }
    }

    public void ResetVerticalVelocity()
    {
        if (grapplingHook.IsGrappled())
        {
            grapplingHook.ResetVerticalVelocity();
        }
        else
        {
            //rigidbody.linearVelocity = Vector3.Scale(movementSpeed, Vector3.one - transform.up);
            Vector3 v = rigidbody.linearVelocity;
            rigidbody.linearVelocity = new Vector3(v.x, 0, v.z);
        }
    }


    /* Dash */
    void Dash()
    {
        if(dashMode == DashMode.Shift)
            ShiftDash();
        else if(dashMode==DashMode.DoubleTap)
            DoubleTapDash();
    }
    void ShiftDash()
    {
        if (!canDash)
            return;

        if (GetComponent<GrapplingHook>().IsGrappled())
            return;

        if (KeyPressed(KeyCode.LeftShift))
        {
            playerSounds.DashSound(1);
            playerSounds.DashParticles();
            canDash = false;
            StartCoroutine(DashInCurrentDirection());
            StartCoroutine(StartDashCooldown());
        }
        else
            return;
    }
    IEnumerator DashInCurrentDirection()
    {
        /* Start */
        isDashing = true;
        rigidbody.useGravity = false;
        ResetVerticalVelocity();
        SetSpeed(rigidbody.linearVelocity.normalized * dashForce);

        yield return new WaitForSeconds(dashTime); //Wait

        /* Stop */
        rigidbody.useGravity = true;
        SetSpeed(rigidbody.linearVelocity.normalized * maxSpeed);
        isDashing = false;
    }
    void DoubleTapDash()
    {
        if (!canDash)
            return;

        if (GetComponent<GrapplingHook>().IsGrappled())
            return;

        if (DoublePressedButton(KeyCode.W, ref pressedFirstW, ref lastPressedW))
            StartCoroutine(DashInVectorDirection(transform.forward));
        else if (DoublePressedButton(KeyCode.A, ref pressedFirstA, ref lastPressedA))
            StartCoroutine(DashInVectorDirection(-transform.right));
        else if (DoublePressedButton(KeyCode.S, ref pressedFirstS, ref lastPressedS))
            StartCoroutine(DashInVectorDirection(-transform.forward));
        else if (DoublePressedButton(KeyCode.D, ref pressedFirstD, ref lastPressedD))
            StartCoroutine(DashInVectorDirection(transform.right));
        else
            return;

        canDash = false;
        StartCoroutine(StartDashCooldown());
    }
    bool DoublePressedButton(KeyCode key, ref bool pressedFirst, ref float lastPressed)
    {
        if (KeyPressed(key))
        {
            if (pressedFirst)
            {
                bool isDoublePressed = Time.time - lastPressed <= delayBetweenPresses;

                if (isDoublePressed)
                {
                    Debug.Log("Double Pressed - W");
                    pressedFirst = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Pressed First - W");
                pressedFirst = true;
            }

            lastPressed = Time.time;
        }

        /* Time Ran out*/
        if (pressedFirst && Time.time - lastPressed > delayBetweenPresses)
        {
            pressedFirst = false;
        }

        return false;
    }

    IEnumerator DashInVectorDirection(Vector3 v)
    {
        /* Start */
        isDashing = true;
        rigidbody.useGravity = false;
        ResetVerticalVelocity();
        AddForce(v * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashTime); //Wait

        /* Stop */
        rigidbody.useGravity = true;
        SetSpeed(rigidbody.linearVelocity.normalized * maxSpeed);
        isDashing = false;
    }
    IEnumerator StartDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void ForceResetDash()
    {
        rigidbody.useGravity = true;
        isDashing = false;
        canDash = true;
    }

    bool KeyPressed(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}
