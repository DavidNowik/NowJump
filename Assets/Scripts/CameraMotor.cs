using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;

    [Header("Follow Bounds")]
    public float boundX;
    public float boundY;

    public static bool lookAround = false;
    private float lookSpeed;
    private Vector3 offset;

    private void Awake()
    {
        // Start camera at player position with fixed Z
        transform.position = lookAt.position + new Vector3(0, 0, -10);
        offset = transform.position - lookAt.position;
        lookSpeed = 10;
    }

    private void LateUpdate()
    {
        if (lookAround)
        {
            // Get input from horizontal and vertical axes (WASD or Arrow keys)
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            // Set the camera's new position based on input, scale it by speed (for smoother movement)
            Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0) * lookSpeed * Time.deltaTime;

            // Calculate the distance from the player
            Vector3 camToPlayer = newPosition - lookAt.position;

            // Clamp the camera's distance to the max distance
            camToPlayer = Vector3.ClampMagnitude(camToPlayer, 35);

            // Apply the clamped position to the camera while keeping the Z-axis at -10

            newPosition = lookAt.position + camToPlayer;
            newPosition.z = -10f;

            // Apply the new position to the camera
            transform.position = newPosition;
        }

        else
        {
            // Follow player within bounds
            Vector3 delta = Vector3.zero;

            float deltaX = lookAt.position.x - transform.position.x;
            if (deltaX > boundX || deltaX < -boundX)
            {
                delta.x = transform.position.x < lookAt.position.x ? deltaX - boundX : deltaX + boundX;
            }

            float deltaY = lookAt.position.y - transform.position.y;
            if (deltaY > boundY || deltaY < -boundY)
            {
                delta.y = transform.position.y < lookAt.position.y ? deltaY - boundY : deltaY + boundY;
            }

            transform.position += new Vector3(delta.x, delta.y, 0f);
        }

    }
}
