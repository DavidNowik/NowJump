using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Move info")]
    [SerializeField] private bool flipOnAwake;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance = 0.3f;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform wallCheck;

    private int facingDirection = -1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (flipOnAwake) Flip();
    }

    void Start()
    {
        StartMovement();
    }

    void Update()
    {
        CheckForWall();
    }

    private void StartMovement()
    {
        rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
        StartMovement();
    }

    private void CheckForWall()
    {
        // Cast a ray in front to detect a wall
        Vector2 direction = new Vector2(facingDirection, 0);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, direction, wallCheckDistance, whatIsWall);

        if (hit.collider != null)
        {
            Flip();
        }

        Debug.DrawRay(wallCheck.position, direction * wallCheckDistance, Color.red); // Visual debug
    }
}
