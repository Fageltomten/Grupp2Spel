using System.Collections;
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

    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stopSpeed = 5f;
    [SerializeField] private float sidewaysStopSpeed = 5f;

    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float grouldClearance = 0.1f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float groundStickiness = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private InputAction movementInput;
    private InputAction jumpInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        movementInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        jumpInput.performed += _ => OnJump();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        SetData();
        IsGrounded();
        Move();
    }

    private void Move()
    {
        Vector3 forceToAdd = Vector3.zero;
        if (inputMagnitude == 0 && currentSpeed != 0)
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
            print(dottedValue);
            var moveSpeed = moveVector.magnitude;
            forceToAdd += (1 - Mathf.Clamp01(moveSpeed / maxSpeed) * dottedValue) * acceleration * (movementDirection.z * transform.forward + movementDirection.x * transform.right);
            forceToAdd -= sidewaysVelocity * sidewaysStopSpeed;
            
            Debug.DrawLine(transform.position, transform.position + forceToAdd, Color.red);
            Debug.DrawLine(transform.position, transform.position + targetVelocity, Color.green);
            Debug.DrawLine(transform.position, transform.position + rigidbody.linearVelocity, Color.blue);
            Debug.DrawLine(transform.position, transform.position + (sidewaysVelocity) , Color.yellow);
        }
        forceToAdd = Vector3.Scale(forceToAdd, Vector3.one - transform.up);
        rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
    }

    private void IsGrounded()
    {
        isGrounded = false;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            transform.position += (hit.normal * grouldClearance - hit.normal * hit.distance) * groundStickiness * Time.fixedDeltaTime;
            if(grouldClearance>= hit.distance)
            {   
                rigidbody.linearVelocity = Vector3.Scale(rigidbody.linearVelocity, Vector3.one - transform.up);
                isGrounded = true;
            }
        }
    }
    

    private void OnJump()
    {
        if(isGrounded)
        {
            hasAirJumped = false;
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if(!hasAirJumped)
        {
            hasAirJumped = true;
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

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
}
