using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerLook : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform cameraPivotPoint;
    [SerializeField] Transform playerBody;

    [Header("MouseInputs")]
    [SerializeField] Vector2 mouseVector;
    [SerializeField] float sensetivity;

    [Header("Rotation")]
    [SerializeField] float horizontalRotation;
    [SerializeField] float verticalRotation; //Isn't used
    [SerializeField] bool rotatePlayerBody;
    [SerializeField] Vector3 playerDirection;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();  
        CalculateRotation();
        ApplyRotation();
        RotatePlayer();
    }

    void GetInputs()
    {
        mouseVector = new Vector2(Input.GetAxisRaw("Mouse X") * sensetivity, Input.GetAxisRaw("Mouse Y") * sensetivity);
    }

    void CalculateRotation()
    {
        horizontalRotation += mouseVector.x;
        verticalRotation -= mouseVector.y;

        int min = -90;
        int max = 90;

        verticalRotation = Mathf.Clamp(verticalRotation, min, max);
    }

    void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0); //Horizontal
        /* Vertical */
        cameraPivotPoint.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void RotatePlayer()
    {
        if (!rotatePlayerBody)
            return;


        
        InputAction moveInput = InputSystem.actions.FindAction("Move");
        if (moveInput.ReadValue<Vector2>() == Vector2.zero)
            return;

        Vector3 movementDirection =  new Vector3(moveInput.ReadValue<Vector2>().x, 0, moveInput.ReadValue<Vector2>().y);
        playerBody.localRotation = Quaternion.Slerp(playerBody.localRotation, Quaternion.LookRotation(movementDirection.normalized), 0.2f);
    }
}
