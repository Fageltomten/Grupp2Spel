using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    // Anton Sundell, 030325
    [SerializeField] float speed;
    [SerializeField] Transform[] waypoints;
    private int startingPoint, i;
    

    void Start()
    {
        transform.position = waypoints[startingPoint].position;
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
