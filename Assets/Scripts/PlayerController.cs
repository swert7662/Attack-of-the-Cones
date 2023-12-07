using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;    

    private void Update()
    {
        NormalMovement();
    }

    private void NormalMovement()
    {
        // Get input from WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a movement vector based on the input and speed
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;

        // Move the player
        transform.Translate(movement);
    }

    private void MouseBasedMovement()
    {
        // Convert the mouse position to world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Ensure there is no change in the z-axis

        // Move the player towards the mouse position
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
    }
}