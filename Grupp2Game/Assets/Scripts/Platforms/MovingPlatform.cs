using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// Author Anton Sundell

public class MovingPlatform : Platform
{
    // Several booleans allow you to customize the platform in the inspector.
    // By adding or editing waypoint objects you may alter the path of the platform. Be sure to add the new waypoints in the waypoints array of the platform object.
    [SerializeField] bool isMovable = true;
    [SerializeField] bool willRotate;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool isTimeBased;
    [SerializeField] private float timeBasedDelay;
    private float movingTimer, breakingTimer;
    private bool shouldMove = true;
    [SerializeField] bool isBreakable;
    [SerializeField] private float breakingDelay = 3f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    protected override void Awake()
    {
        SetInitialStartingPosition();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    protected override void FixedUpdate()
    {
        if (isMovable) 
        { 
            Move(); 
        }
        if ((!isTimeBased && willRotate) || (isTimeBased && shouldMove && willRotate))
        {
            Rotate();
        }
        if (!shouldMove && isTimeBased)
        {
            movingTimer += Time.deltaTime;
            if (movingTimer > timeBasedDelay)
            {
                movingTimer = 0;
                shouldMove = true;
            }
        }
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
        if ((isTimeBased && shouldMove) || !isTimeBased)
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
            transform.parent.position = Vector3.MoveTowards(transform.position, waypoints[i].position, speed * Time.fixedDeltaTime);
        }
    }
    public void Rotate()
    {
        // If the platform is movable it will smoothly rotate toward the next waypoint’s rotation
        // Otherwise it will rotate continuously around the Y axis
        Transform platformTransform = transform.parent;
        if (!isMovable)
        {           
            platformTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            Transform targetWaypoint = waypoints[i];
            platformTransform.rotation = Quaternion.RotateTowards(
                platformTransform.rotation,
                targetWaypoint.rotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (isBreakable)
        {
            StartCoroutine(BreakPlatform());          
        }
        other.transform.SetParent(transform.parent);

    }
    protected override void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null); 
        DontDestroyOnLoad(other.transform);       
    }
    private IEnumerator BreakPlatform()
    {
        // The platform will change color during the breaking countdown, then deactivate, waiting for the platformSpawn script to reactivate it.
        float elapsedTime = 0f;
        Renderer renderer = GetComponent<Renderer>();
        Color startColor = renderer.material.color;
        Color endColor = Color.red;

        while (elapsedTime < breakingDelay)
        {
            float t = elapsedTime / breakingDelay;
            renderer.material.color = Color.Lerp(startColor, endColor, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        renderer.material.color = startColor;

        foreach (Transform child in transform)
        {
            child.SetParent(null);
        }

        gameObject.SetActive(false);
    }
}
