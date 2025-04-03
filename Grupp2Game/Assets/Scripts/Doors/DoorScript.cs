using UnityEngine;

//Author Anton Sundell
// TO DO:
// Make the door activatable, scrap trigger system
// Fix prefab
public class DoorScript : MonoBehaviour
{ 
    [SerializeField] protected float speed;
    [SerializeField] protected Transform[] waypoints;
    private int startingPoint, i = 1;

    private bool isMoving = false;

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[i].position) < 0.05f)
        { 
            isMoving = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[i].position, speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Close();
        }
    }

    private void Open()
    {
        if (i > 0) 
        {
            i--;
            isMoving = true;
        }
    }
    private void Close()
    {
        if (i < waypoints.Length - 1)
        {
            i++;
            isMoving = true;
        }
    }
}
