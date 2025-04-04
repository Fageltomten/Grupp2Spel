using UnityEngine;

public class TimeBasedPlatform : Platform
{
    [SerializeField] private float timeDelay;
    private float timer;
    private bool shouldMove = true;
    protected override void Awake()
    {
        SetInitialStartingPosition();
    }
    protected override void Update()
    {
       if(!shouldMove)
        {
            timer += Time.deltaTime;
            if(timer > timeDelay)
            {
                timer = 0;
                shouldMove = true;
            }
        }
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
            transform.parent.position = waypoints[startIndex].position;
        }
        else
        {
            transform.parent.position = waypoints[startingPoint].position;
        }
    }
    protected override void Move()
    {
        if (shouldMove)
        {
            if (Vector3.Distance(transform.parent.position, waypoints[i].position) < 0.05f)
            {
                i++;
                shouldMove = false;
                if (i == waypoints.Length)
                {
                    i = 0;
                }
            }
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, waypoints[i].position, speed * Time.fixedDeltaTime);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform.parent, true);

    }
    protected override void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null, true);
    }
}
