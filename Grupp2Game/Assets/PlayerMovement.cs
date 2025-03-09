using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    [SerializeField] float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3 (horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate (movementDirection * speed * Time.deltaTime);
    }
}
