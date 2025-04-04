using UnityEngine;

public class ExhaustFan : MonoBehaviour
{
    [SerializeField] private float spinningBladesSpeed;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, spinningBladesSpeed * Time.deltaTime));
    }
}
