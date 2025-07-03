using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : Trap
{
    private bool _isActive = true;
    [SerializeField] private bool startsOnTop = true;

    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private Animator anim;

    [Header("Move Info")]
    [SerializeField, Range(1, 20)] private float speed;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform center;
    private Vector2 currentDirection;

    public bool startActive = true;
    // Rigidbody2D reference for movement and physics interactions
    private Rigidbody2D rb;

    // Variable to track the lock time when no collision is detected
    private float lockTime = 0f;
    private const float LOCK_DURATION = 0.1f; // Lock duration in seconds

    void Start()
    {
        isActive = startActive;
        anim = GetComponent<Animator>();

        // Get the Rigidbody2D component attached to the trap
        rb = GetComponent<Rigidbody2D>();
        // Ensure the Rigidbody2D's properties are set correctly
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity if not needed
            rb.drag = 0; // Set drag to 0 for no resistance (or adjust as needed)
            rb.angularDrag = 0; // Optional: You can adjust angular drag if needed.
        }

        if (startsOnTop)
            currentDirection = Vector2.right; // Initial direction to move right
        else 
            currentDirection = Vector2.left;
    }

    void FixedUpdate()
    {
        anim.SetBool("isActive", isActive);
        if (!isActive)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // If the saw is in the lock state, count down the lock time
        if (lockTime > 0)
        {
            lockTime -= Time.deltaTime;
            // While locked, we continue to move in the current direction
            Move();
            return;
        }

        // Move the saw trap using the Rigidbody2D
        Move();
        CheckCollision();
    }

    void Move()
    {
        if (rb != null)
        {
            // Apply movement force in the current direction
            rb.velocity = currentDirection * speed;
        }
    }

    void CheckCollision()
    {
        // Raycast checks in all four directions
        RaycastHit2D hitRight = Physics2D.Raycast(center.position, Vector2.right, groundCheckDistance, whatIsGround);
        RaycastHit2D hitLeft = Physics2D.Raycast(center.position, Vector2.left, groundCheckDistance, whatIsGround);
        RaycastHit2D hitUp = Physics2D.Raycast(center.position, Vector2.up, groundCheckDistance, whatIsGround);
        RaycastHit2D hitDown = Physics2D.Raycast(center.position, Vector2.down, groundCheckDistance, whatIsGround);

        // If no hit detected in any direction, rotate the direction clockwise
        if (hitRight.collider == null && hitLeft.collider == null && hitUp.collider == null && hitDown.collider == null)
        {
            RotateClockwise(); // Immediately rotate clockwise to the next direction
            lockTime = LOCK_DURATION; // Lock in the new direction for 1 second
        }
    }

    void RotateClockwise()
    {
        // Rotate the currentDirection clockwise (90 degrees)
        if (currentDirection == Vector2.right)
        {
            currentDirection = Vector2.down; // Move down after right
        }
        else if (currentDirection == Vector2.down)
        {
            currentDirection = Vector2.left; // Move left after down
        }
        else if (currentDirection == Vector2.left)
        {
            currentDirection = Vector2.up; // Move up after left
        }
        else if (currentDirection == Vector2.up)
        {
            currentDirection = Vector2.right; // Move right after up
        }
    }

    void OnDrawGizmos()
    {
        if (center != null)
        {
            // Draw Gizmos to show the raycast directions from the center
            Gizmos.color = Color.red;
            Gizmos.DrawLine(center.position, center.position + Vector3.up * groundCheckDistance);
            Gizmos.DrawLine(center.position, center.position + Vector3.down * groundCheckDistance);
            Gizmos.DrawLine(center.position, center.position + Vector3.left * groundCheckDistance);
            Gizmos.DrawLine(center.position, center.position + Vector3.right * groundCheckDistance);
        }
    }

    // Handle collision with player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && isActive)
        {
            GameManager.instance.canvasManager.Reset(true);
        }
    }
}
