using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 movementDirection;
    private float inputMagnitude;
    private Vector3 movementSpeed;
    private float currentSpeed;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stopSpeed = 0.1f;

    private InputAction movementInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        movementInput = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        SetData();
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
        else
        {
            float dottedValue = (Vector3.Dot(rigidbody.linearVelocity.normalized, movementDirection.z * transform.forward + movementDirection.x * transform.right)+1)/2 ;
            print(dottedValue);
            forceToAdd += (1 - (currentSpeed * dottedValue) / maxSpeed) * acceleration * (movementDirection.z * transform.forward + movementDirection.x * transform.right);
            forceToAdd -= (Vector3.one - (movementDirection.z * transform.forward + movementDirection.x * transform.right)) * stopSpeed;
        }

        rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
    }
    

    private void OnMove(InputValue input)
    {
        SetData();
    }

    private void SetData()
    {
        movementDirection = new Vector3(movementInput.ReadValue<Vector2>().x, 0, movementInput.ReadValue<Vector2>().y).normalized;
        inputMagnitude = movementDirection.magnitude;
        currentSpeed = rigidbody.linearVelocity.magnitude;
    }
}
