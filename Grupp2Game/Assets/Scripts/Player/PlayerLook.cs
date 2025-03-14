using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerLook : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform cameraPivotPoint;

    [Header("MouseInputs")]
    [SerializeField] Vector2 mouseVector;
    [SerializeField] float sensetivity;

    [Header("Rotation")]
    [SerializeField] float horizontalRotation;
    [SerializeField] float verticalRotation; //Isn't used

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
}
