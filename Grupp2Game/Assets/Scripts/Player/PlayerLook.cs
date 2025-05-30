using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
/* Author: Anton Andersson */

public class PlayerLook : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform cameraPivotPoint;

    [Header("MouseInputs")]
    [SerializeField] Vector2 mouseVector;
    [SerializeField] float sensetivity;

    [Header("Rotation")]
    [SerializeField] float horizontalRotation;
    [SerializeField] float verticalRotation;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Player Initiated");
    }

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

        int min = -45;
        int max = 90;

        verticalRotation = Mathf.Clamp(verticalRotation, min, max);
    }

    void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0); 
        cameraPivotPoint.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    public void SetRotation(float verticalRotation, float horizontalRotation)
    {
        this.verticalRotation = verticalRotation;
        this.horizontalRotation = horizontalRotation;
    }
}
