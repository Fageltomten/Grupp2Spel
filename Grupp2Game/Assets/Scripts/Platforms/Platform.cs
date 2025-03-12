using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected Transform[] waypoints;
    protected int startingPoint, i;

    [SerializeField] protected bool useRandomStartPoint = false;


    protected abstract void Awake();
    protected virtual void Update()
    {

    }
    protected abstract void FixedUpdate();

    protected abstract void SetInitialStartingPosition();
    protected abstract void Move();

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerExit(Collider other);

}
