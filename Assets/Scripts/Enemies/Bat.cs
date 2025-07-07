using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move info")]
    [SerializeField] private bool flipOnAwake;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float idleTime = 2;
    private float idleTimeCounter;

    [Header("Collision info")]
    [SerializeField] private float ceilingCheckRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform ceilingCheck;

    private bool ceilingDetected;
    private int facingDirection = -1;
    private bool isIdle = false;


    // ⏱️ New timer to disable collision check temporarily
    private float collisionDisabledUntil = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Awake()
    {
        collisionDisabledUntil = 1f;
        if (flipOnAwake) Flip();
    }

    void Update()
    {
        // ⛔ Skip ceiling detection if in cooldown window
        if (Time.time >= collisionDisabledUntil)
        {
            CollisionCheck();
        }
        else
        {
            ceilingDetected = false;
        }

        if (!isIdle)
        {
            anim.SetBool("flying", true);
            idleTimeCounter = idleTime;
        }
        else
        {
            anim.SetBool("flying", false);
            rb.velocity = Vector2.zero;
            idleTimeCounter -= Time.deltaTime;

            if (idleTimeCounter <= 0)
            {
                isIdle = false;
            }
        }

        if (ceilingDetected)
        {
            Flip();
            StartIdle();

            // ⏳ Disable collision detection for idleTime + 0.5 sec
            collisionDisabledUntil = Time.time + idleTime + 1f;
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionCheck()
    {
        ceilingDetected = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, whatIsGround);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("touched Bat");
            GameManager.instance.canvasManager.Reset(true);
        }

        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("Bat hit by Block");
            Destroy(GetComponent<CapsuleCollider2D>());
            anim.SetTrigger("die");
        }
    }

    public void StartMovement()
    {
        rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);
    }
    public void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
