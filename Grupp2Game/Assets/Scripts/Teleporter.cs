using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform exitPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;


        other.gameObject.SetActive(false);

        other.GetComponent<PlayerMovement>().SetSpeed(Vector3.zero);
        other.GetComponent<PlayerMovement>() .ForceResetDash();
        other.transform.position = exitPoint.position;

        other.gameObject.SetActive(true);
    }
}
