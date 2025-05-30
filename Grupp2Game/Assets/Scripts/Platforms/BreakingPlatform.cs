using System.Collections;
using UnityEngine;

// Author Anton Sundell
// Script not used, moved to MovingPlatform script
public class BreakingPlatform : Platform, IMovablePlatform
{
    [SerializeField] bool isMovable = true;
    [SerializeField] bool isBreakable;
    [SerializeField] private float breakingDelay = 3f;
    private float timer;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    public bool IsMovable
    {
        get => isMovable;
        set => isMovable = value;
    }
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
        StartCoroutine(BreakPlatform());

    }
    protected override void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
    private IEnumerator BreakPlatform()
    {
        
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
