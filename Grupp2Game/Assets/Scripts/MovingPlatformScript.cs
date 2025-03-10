using UnityEngine;

// Anton Sundell, 030325

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform[] waypoints;
    private int startingPoint, i;

    [SerializeField] private bool useRandomStartPoint = false;
    

    void Start()
    {
        //Added an option to choose between random start point or default start point
        if(useRandomStartPoint)
        {
           int startIndex = Random.Range(0, waypoints.Length);
            transform.position = waypoints[startIndex].position;
        }
        else
        {
            transform.position = waypoints[startingPoint].position;
        }
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, waypoints[i].position) < 0.05f)
        {
            i++;

            if (i == waypoints.Length)
            { 
                i = 0; 
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[i].position, speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering platform");
        //  other.transform.SetParent(transform);
        other.transform.parent = transform;

    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
