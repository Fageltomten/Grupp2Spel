using UnityEngine;

//Author Vidar Edlund
public class MenuCamera : MonoBehaviour
{
    private int currentIndex = 0;
    private readonly int startingPoint = 0;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] bool useRandomStartPoint = false;

    private void Awake()
    {
        SetInitialStartingPosition();
    }
   
    void Update()
    {
        Move();
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
            }
        }
            transform.LookAt(waypoints[currentIndex].position * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndex].position, moveSpeed * Time.deltaTime);
    }
}
