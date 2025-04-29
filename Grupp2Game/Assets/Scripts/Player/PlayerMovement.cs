using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] float dashForce = 25f;
    [SerializeField] float delayBetweenPresses = 0.25f;
    [SerializeField] float dashCooldown = 2f;
    [SerializeField] bool canDash = true;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        grapplingHook = GetComponent<GrapplingHook>();
        movementInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        jumpInput.performed += _ => OnJump();
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
            forceToAdd += (1 - Mathf.Clamp01(moveSpeed / maxSpeed) * dottedValue) * tempAcceleration * (movementDirection.z * transform.forward + movementDirection.x * transform.right);
            if(!grapplingHook.IsGrappled() && isGrounded)
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
        if (Physics.BoxCast(transform.position - transform.up * (grouldClearance/2), new(0.5f,grouldClearance/2,0.5f), -transform.up, out RaycastHit hit,quaternion.identity, 1 ,groundLayer, QueryTriggerInteraction.Ignore))
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
        }
        else if(!hasAirJumped)
        {
            hasAirJumped = true;
            ResetVerticalVelocity();
            AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
            rigidbody.linearVelocity = Vector3.Scale(movementSpeed, Vector3.one - transform.up);
        }
    }

    public void Dash()
    {
        if (!canDash)
            return;

        if (GetComponent<GrapplingHook>().IsGrappled())
            return;

        if (DoublePressedW())
            StartCoroutine(PerformDash(transform.forward));
        else if (DoublePressedA())
            StartCoroutine(PerformDash(-transform.right));
        else if (DoublePressedS())
            StartCoroutine(PerformDash(-transform.forward));
        else if (DoublePressedD())
            StartCoroutine(PerformDash(transform.right));
        else
            return;

        canDash = false;
        StartCoroutine(StartDashCooldown());
    }

    IEnumerator PerformDash(Vector3 v)
    {
        AddForce(v * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.4f);
        Vector3 lV = rigidbody.linearVelocity;
        rigidbody.linearVelocity -= v * dashForce;//new Vector3(lV.x - v.x*10, lV.y - v.x*10, lV.z - v.z*10);
    }
    IEnumerator StartDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    bool DoublePressedW()
    {
        if (KeyPressed(KeyCode.W))
        {
            if (pressedFirstW)
            {
                bool isDoublePressed = Time.time - lastPressedW <= delayBetweenPresses;

                if (isDoublePressed)
                {
                    Debug.Log("Double Pressed - W");
                    pressedFirstW = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Pressed First - W");
                pressedFirstW = true;
            }

            lastPressedW = Time.time;
        }

        /* Time Ran out*/
        if (pressedFirstW && Time.time - lastPressedW > delayBetweenPresses)
        {
            pressedFirstW = false;
        }

        return false;
    }
    bool DoublePressedA()
    {
        if (KeyPressed(KeyCode.A))
        {
            if (pressedFirstA)
            {
                bool isDoublePressed = Time.time - lastPressedA <= delayBetweenPresses;

                if (isDoublePressed)
                {
                    Debug.Log("Double Pressed - A");
                    pressedFirstA = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Pressed First - A");
                pressedFirstA = true;
            }

            lastPressedA = Time.time;
        }

        /* Time Ran out*/
        if (pressedFirstA && Time.time - lastPressedA > delayBetweenPresses)
        {
            pressedFirstA = false;
        }

        return false;
    }
    bool DoublePressedS()
    {
        if (KeyPressed(KeyCode.S))
        {
            if (pressedFirstS)
            {
                bool isDoublePressed = Time.time - lastPressedS <= delayBetweenPresses;

                if (isDoublePressed)
                {
                    Debug.Log("Double Pressed - S");
                    pressedFirstS = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Pressed First - S");
                pressedFirstS = true;
            }

            lastPressedS = Time.time;
        }

        /* Time Ran out*/
        if (pressedFirstS && Time.time - lastPressedS > delayBetweenPresses)
        {
            pressedFirstS = false;
        }

        return false;
    }
    bool DoublePressedD()
    {
        if (KeyPressed(KeyCode.D))
        {
            if (pressedFirstD)
            {
                bool isDoublePressed = Time.time - lastPressedD <= delayBetweenPresses;

                if (isDoublePressed)
                {
                    Debug.Log("Double Pressed - D");
                    pressedFirstD = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Pressed First - D");
                pressedFirstD = true;
            }

            lastPressedD = Time.time;
        }

        /* Time Ran out*/
        if (pressedFirstD && Time.time - lastPressedD > delayBetweenPresses)
        {
            pressedFirstD = false;
        }

        return false;
    }

    bool KeyPressed(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}
