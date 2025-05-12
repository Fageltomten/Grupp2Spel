using UnityEngine;


/// <summary>
/// Component that moves the target that the inverse Kinematics use
/// author Carl Åslund
/// </summary>
public class PlayerFootTarget : MonoBehaviour
{

    [SerializeField] float stepDistance;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSpeed;
    [SerializeField] Transform middlePoint;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float waitTimeBeforeIdle;

    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldPosition;


    float lerp = 1;
    float idleTimer = 0;
    bool idle = false;
    bool fast = false;

    public bool IsActive { get; set; } = true;
    

    
    void Start()
    {
        currentPosition = transform.position;
        oldPosition = transform.position;
    }

    
    void Update()
    {
        //early return
        if (!IsActive)
        {
            return;
        }

        transform.position = currentPosition;
        Vector3 hitPos = new Vector3();
        
        
        // gets position to move to
        if (Physics.Raycast(middlePoint.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            hitPos = hit.point;

            if (Vector3.Distance(newPosition, hit.point) > stepDistance)
            {
                //interupt previous
                if(lerp < 1)
                {
                    fast = true;
                    
                } else
                {
                    lerp = 0;
                    fast = false;
                }
                newPosition = hit.point + (Vector3.Normalize(hit.point - newPosition) * stepDistance * (Random.Range(0.5f, 0.9f)));
                idleTimer = 0;
                idle = false;

            }

        }
        
        // moves point
        if (lerp < 1)
        {
            Vector3 footPos = Vector3.Slerp(oldPosition, newPosition, lerp);
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            if (fast)
            {
                currentPosition = newPosition;
            }
            else
            {
                currentPosition = footPos;
                lerp += Time.deltaTime * stepSpeed;
            }
           
            
            if (!(lerp < 1))
            {
                //finished moving
                
            }
        }
        else
        {
            oldPosition = newPosition;
        }

        if (idleTimer > 0.2f)
        {
            fast = false;
        }

        // idle timer
        if (idleTimer > waitTimeBeforeIdle && !idle)
        {
            newPosition = hitPos;
            lerp = 0;
            idle = true;
        } else
        {
            idleTimer += Time.deltaTime;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPosition, 0.3f);
    }
}
