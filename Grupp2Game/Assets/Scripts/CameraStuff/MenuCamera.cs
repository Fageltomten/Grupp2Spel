using UnityEngine;

//Author Vidar Edlund
public class MenuCamera : MonoBehaviour
{
    private int currentIndex = 0;
    private readonly int startingPoint = 0;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] bool useRandomStartPoint = false;

    private void Awake()
    {
        SetInitialStartingPosition();
    }
   
    void Update()
    {
        Move();
        Rotate();
    }
    private void SetInitialStartingPosition()
    {
        if (useRandomStartPoint)
        {
            int startIndex = Random.Range(0, waypoints.Length);
            transform.position = waypoints[startIndex].position;
        }
        else
        {
            transform.position = waypoints[startingPoint].position;
        }
    }
    private void Move()
    {
        if (Vector3.Distance(waypoints[currentIndex].position, transform.position) < 1f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0;
                transform.position = waypoints[startingPoint].position;
            }
        }
       
        

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndex].position, moveSpeed * Time.deltaTime);
    }
    private void Rotate()
    {
        // transform.LookAt(waypoints[currentIndex].position);
        //transform.LookAt(waypoints[currentIndex].position * (rotateSpeed * Time.deltaTime));

        //  var dir = transform.forward;
        //var dir = (waypoints[currentIndex].position - transform.position).normalized;
        //var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, (angle * rotateSpeed) * Time.deltaTime , 0);
        //  transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        // transform.forward =  Vector3.Lerp(transform.forward, waypoints[currentIndex].forward, rotateSpeed * Time.deltaTime);

        // transform.rotation = Quaternion.FromToRotation(transform.forward, waypoints[currentIndex].forward);

        Vector3 targetDirection = (waypoints[currentIndex].position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        //How do you make it more smooth and less hacky?
    }
}
