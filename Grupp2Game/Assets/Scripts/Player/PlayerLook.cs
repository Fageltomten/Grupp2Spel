using UnityEngine;

//Anton Andersson
public class PlayerLook : MonoBehaviour
{
    Rigidbody rb;

    [Header("MouseInputs")]
    [SerializeField] Vector2 mouseVector;
    [SerializeField] float sensetivity;

    [Header("Rotation")]
    [SerializeField] float horizontalRotation;
    [SerializeField] float verticalRotation; //Isn't used

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
    }
}
