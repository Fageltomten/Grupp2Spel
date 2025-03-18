using UnityEngine;

// Anton Sundell, 030325

public class MovingPlatform : Platform
{
    protected override void Awake()
    {
        SetInitialStartingPosition();
    }

    protected override void FixedUpdate()
    {
        Move();
    }
    protected override void SetInitialStartingPosition()
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
    protected override void Move()
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

    protected override void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);

    }
    protected override void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
        DontDestroyOnLoad(other.transform);
    }
}
