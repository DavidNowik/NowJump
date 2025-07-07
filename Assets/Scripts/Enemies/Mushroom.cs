using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;


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

    private bool groundDetected;
    private bool wallDetected;
    private int facingDirection = -1;

    private bool isIdle = false;

    // ?? Flip cooldown
    private float flipCooldown = 1f;
    private float lastFlipTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CollisionCheck();

        if (!isIdle)
        {
            anim.SetBool("running", true);
            rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);
            idleTimeCounter = idleTime;
        }
        else
        {
            anim.SetBool("running", false);
            rb.velocity = new Vector2(0, 0);
            idleTimeCounter -= Time.deltaTime;

            if (idleTimeCounter <= 0)
            {
                isIdle = false;
            }
        }

        // ?? Only flip if cooldown passed
        if ((wallDetected || !groundDetected) && Time.time >= lastFlipTime + flipCooldown)
        {
            Flip();
            StartIdle();
            lastFlipTime = Time.time;
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionCheck()
    {
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        groundDetected = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, whatIsGround);
    }

    private void StartIdle()
    {
        if (!isIdle)
        {
            isIdle = true;
            idleTimeCounter = idleTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);

        Gizmos.color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("touched Mushroom");
        }
    }

    public void Testing()
    {
        gameObject.SetActive(false);
    }

    public void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
