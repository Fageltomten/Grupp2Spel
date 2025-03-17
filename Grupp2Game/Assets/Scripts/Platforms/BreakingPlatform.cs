using System.Collections;
using UnityEngine;

public class BreakingPlatform : Platform
{
    //TODO
    //Platform that breaks after you jump on it
    //And then despawn again
    //Make the platform shake or change color before it breaks
    //Make it spawn again after some time
    [SerializeField] private float breakingDelay = 3f;
    [SerializeField] private float spawnDelay = 10f;
    private float timer;
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
        StartCoroutine(BreakPlatform());

    }
    protected override void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
    private IEnumerator BreakPlatform()
    {
        yield return new WaitForSeconds(breakingDelay);
        foreach (Transform child in transform)
        {
            child.SetParent(null);
        }
        gameObject.SetActive(false);
        StartCoroutine(RespawnAfterDelay());
    }
    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        RespawnPlatform();
    }
    private void RespawnPlatform()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
    }
}
