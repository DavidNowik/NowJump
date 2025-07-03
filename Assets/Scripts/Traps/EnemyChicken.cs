using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChicken : MonoBehaviour, IKillable
{
    private Rigidbody2D rb;
    private Animator anim;

    public HealthbarWS healthbar;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float idleTime = 2;
    private float idleTimeCounter;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform playerCheck; // For detecting player behind

    private bool groundDetected;
    private bool wallDetected;
    private int facingDirection = -1;

    private bool isHit = false;
    private bool isIdle = false; // Flag to track if the enemy is idling

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        CollisionCheck();

        // If the enemy is not idle and not jumping, move normally
        if (!isIdle)
        {
            rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);
            idleTimeCounter = idleTime; // Reset idle counter if moving
        }
        else
        {
            // If the enemy is idle, stop moving
            rb.velocity = new Vector2(0, 0);
            idleTimeCounter -= Time.deltaTime; // Count down idle time

            if (idleTimeCounter <= 0)
            {
                // Once idle time is over, start moving again
                isIdle = false;
            }
        }

        if (wallDetected || !groundDetected)
        {
            Flip();
            StartIdle(); // Start idling after wall detection or falling off
        }

        // Check if player is behind the enemy (using playerCheck)
        CheckPlayerBehind();
    }

    private void Flip()
    {
        if (isHit ) return;
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionCheck()
    {
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void StartIdle()
    {
        if (!isIdle)
        {
            isIdle = true;
            idleTimeCounter = idleTime; // Reset idle counter when idle starts
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground check and wall check raycasts in the Scene view
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        // Visualize the detection range of the playerCheck
        Gizmos.color = Color.red; // You can change the color to something visible
        Gizmos.DrawWireSphere(playerCheck.position, 0.5f); // Adjust the radius as needed
    }

    // Detect collision with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            // Print "BGAAHG" when the enemy hits the player
            Debug.Log("BGAAHG");
            GameManager.instance.canvasManager.Reset(true);
        }
    }

    public void Testing()
    {
        gameObject.SetActive(false);
    }

    // Method to check if player is behind the enemy
    private void CheckPlayerBehind()
    {
        // Check if the player is within a small radius behind the enemy
        Collider2D hitCollider = Physics2D.OverlapCircle(playerCheck.position, 0.5f, LayerMask.GetMask("Player", "Ground"));

        if(hitCollider != null) 
        if (hitCollider.name == "Player" || hitCollider.name.StartsWith("RockHead")) // Check if player is detected and if the chicken isn't already jumping
        {
                Die();
        }
    }
    public void Die()
    {
        rb.velocity = new Vector2(facingDirection * 5, 5f); // Apply horizontal jump force
        isHit = true;
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("hit");
    }
    public void DeleteSelf()
    {
        Destroy(gameObject);
    }

    public HealthbarWS GetHealthbarWS()
    {
        return healthbar;
    }
}
