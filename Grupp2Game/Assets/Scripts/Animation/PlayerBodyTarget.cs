using UnityEngine;

/// <summary>
/// Component that animates the player body
/// author Carl Åslund
/// </summary>
public class PlayerBodyTarget : MonoBehaviour
{
    [SerializeField] Transform player;
    [Header("Rotation")]
    [SerializeField] bool doRotation;
    [SerializeField] float rotationAmountX;
    [SerializeField] float rotationAmountY;
    [SerializeField] float extraY;
    [SerializeField] float rotationSpeed;

    [SerializeField] float fallTiltAmount;
    [SerializeField] float fallTiltMax;

    Vector3 previusRotation;

    
    int rotateDirection = 0;

    Rigidbody playerRigidBody;

    [Header("Bobbing")]
    [SerializeField] bool doBob;
    [SerializeField] float bobAmount;
    [SerializeField] float bobTime;
    [SerializeField] float extraBob;


    float rotX;
    float rotY;

    void Start()
    {
        
        previusRotation = player.rotation.eulerAngles;
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (doRotation)
        {
            Rotate();
        }
        if (doBob)
        {
            Bob();
        }

    }

    void Rotate()
    {

        Vector3 playerRot = player.rotation.eulerAngles;
        Vector3 difference = playerRot - previusRotation;
        
        // selects direction to rotate towards
        rotateDirection = (difference.y < 0) ? -1 : 1;
        if (difference.y == 0)
        {
            rotateDirection = 0;
        }

        // selects falltilt
        float fallTilt = Mathf.Clamp((playerRigidBody.linearVelocity.y * -fallTiltAmount), -fallTiltMax, fallTiltMax);
        

        // selects rotation
        float xRot = Mathf.SmoothDampAngle(transform.eulerAngles.x, (playerRot.x + rotationAmountX * rotateDirection), ref rotX, 1/rotationSpeed);
        float yRot = Mathf.SmoothDampAngle(transform.eulerAngles.y, (playerRot.y + extraY + rotationAmountY * rotateDirection), ref rotY, 1/rotationSpeed);
        //Debug.Log(yRot);
        Vector3 newRotation = new Vector3(xRot, yRot, playerRot.z + fallTilt);
        transform.rotation = Quaternion.Euler(newRotation);

        previusRotation = playerRot;
    }

    void Bob()
    {
        float newX = player.position.x;
        float newY = player.position.y + extraBob + bobAmount * Mathf.Abs(Mathf.Sin(Mathf.PI * Time.time / bobTime));
        float newZ = player.position.z;

        Vector3 newPos = new Vector3(newX, newY, newZ);
        transform.position = newPos;
    }
}
