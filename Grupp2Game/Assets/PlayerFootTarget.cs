using UnityEngine;


/// <summary>
/// Component that moves the target that the inverse Kinematics use
/// author Carl Åslund
/// </summary>
public class PlayerFootTarget : MonoBehaviour
{

    [SerializeField] float stepDistance;
    [SerializeField] float extraStepPercentage;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSpeed;
    [SerializeField] Transform middlePoint;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;

    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldPosition;


    float lerp;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPosition = transform.position;
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = currentPosition;

        if (Physics.Raycast(middlePoint.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            
            if (Vector3.Distance(newPosition, hit.point) > stepDistance)
            {
                oldPosition = hit.point;
                newPosition = hit.point + (Vector3.Normalize(hit.point - newPosition) * stepDistance * extraStepPercentage);
                lerp = 0;
                
            }
            
        }
        if (lerp < 1)
        {
            Vector3 footPos = Vector3.Slerp(oldPosition, newPosition, lerp);
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPos;
            lerp += Time.deltaTime * stepSpeed;
            
        }
        else
        {
            
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPosition, 0.3f);
    }
}
