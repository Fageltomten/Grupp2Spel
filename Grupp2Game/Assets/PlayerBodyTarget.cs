using UnityEngine;

/// <summary>
/// Component that animates the player body
/// author Carl Åslund
/// </summary>
public class PlayerBodyTarget : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float rotationAmountX;
    [SerializeField] float rotationAmountY;
    [SerializeField] float extraY;
    [SerializeField] float smoothTime;

    Vector3 previusRotation;
    float rotVelY = 0;
    float rotVelX = 0;

    Vector3 targetRotation;

    float lerp = 1;

    void Start()
    {
        targetRotation = new Vector3(0, extraY, 0);
        previusRotation = player.rotation.eulerAngles;
    }

    
    void Update()
    {
        Vector3 difference = player.rotation.eulerAngles - previusRotation;
        Vector3 playerRot = player.rotation.eulerAngles;

        float newX = playerRot.x + Mathf.SmoothDamp(transform.eulerAngles.x, (difference.y * rotationAmountX), ref rotVelX, smoothTime);
        float targetY = extraY + playerRot.y + (difference.y * rotationAmountY);
        float newY = Mathf.SmoothDamp(transform.eulerAngles.y, targetY, ref rotVelY, smoothTime);

        targetRotation = new Vector3 (newX, newY, playerRot.z);

        Vector3 currentRotation = targetRotation;

        transform.rotation = Quaternion.Euler(currentRotation);


        previusRotation = playerRot;
        
    }
}
