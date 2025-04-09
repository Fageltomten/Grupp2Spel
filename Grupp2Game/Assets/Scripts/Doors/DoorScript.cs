using UnityEngine;

//Author Anton Sundell

public class DoorScript : MonoBehaviour, IActivatable
{ 
    [SerializeField] protected float speed;
    [SerializeField] protected Transform[] waypoints;
    private int startingPoint, i = 1;
    private bool isActivated;
    private bool isMoving = false;
    private Color channelColor, activatedColor, deactivatedColor;

    void IActivatable.Activate()
    {
        isActivated = true;
        Open();
    }
    void IActivatable.Deactivate()
    {
        isActivated = false;
        Close();
    }
    private void Start()
    {
        channelColor = new Color(0f / 255f, 255f / 255f, 255f / 255f); ;
        deactivatedColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
        activatedColor = new Color(128f / 255f, 255f / 255f, 0f / 255f);

    }
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Open();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")) 
    //    {
    //        Close();
    //    }
    //}

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
