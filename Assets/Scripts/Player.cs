using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    [SerializeField] private Vector2 wallJumpDirection;
    [HideInInspector]public bool onScoutPlatform;

    [Header("Jump Buffer & Coyote Time")]
    [SerializeField] private float coyoteTime = 0.07f;

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsCloud;
    [SerializeField] private LayerMask whatIsTrampoline;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    [Header("References")]
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject dustPrefab;

    [HideInInspector]
    public bool carryingEquipment = false;
    private int jumpsLeft;
    private float coyoteTimeCounter;
    private float activeSpeedBuff = 0f;
    private float activeJumpBuff = 0f;

    private float movingInput;
    private float lastWallJumpX = Mathf.Infinity;

    private bool isWallSliding;
    private bool canWallSlide = false;
    private bool canWallJump = false;
    private bool canDashJump = false;
    private bool isGrounded;
    private bool isWallDetected;
    private bool facingRight = true;
    [HideInInspector]
    public int facingDirection = 1;
    private bool isMoving;
    private float maxFallSpeed = -20f;

    public void Awake()
    {
        CheckPrefs();
        if(GameManager.instance != null)
        if (GameManager.instance.checkPoint != null)
            transform.localPosition = GameManager.instance.checkPoint.position;
    }

    public void CheckPrefs()
    {
        if (PlayerPrefs.GetInt("WallJump") == 1)
        {
            canWallSlide = true;
            canWallJump = true;
        }
        if(PlayerPrefs.GetInt("DashJump") == 1)
        {
            canDashJump = true;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void FixedUpdate()
    {
        CollisionCheck();//HAS to be here
        HandleWallSlide();
        HandleAnimations();
        CapFallSpeed();
    }

    private void HandleMovement()
    {
        if (isWallSliding) return;
        if (isGrounded && CameraMotor.lookAround) { 
            rb.velocity = new Vector2(0, rb.velocity.y);
            isMoving = false;
            return;
        }


        CameraMotor.lookAround = false;
        movingInput = Input.GetAxisRaw("Horizontal");
        float targetSpeed = movingInput * (speed + activeSpeedBuff);
        float lerpFactor = isGrounded ? 15f : 6f;
        float smoothSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, Time.deltaTime * lerpFactor);
       
        if(movingInput != 0)
            rb.velocity = new Vector2(smoothSpeed, rb.velocity.y);
        else if(isGrounded)
            rb.velocity = new Vector2(smoothSpeed, rb.velocity.y);

        isMoving = Mathf.Abs(movingInput) > 0.01f;

        if (movingInput > 0 && !facingRight) Flip();
        else if (movingInput < 0 && facingRight) Flip();
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraMotor.lookAround = false;
            if (isWallSliding)
            {
                WallJump();
            }
            else if(coyoteTimeCounter > 0f && Input.GetAxisRaw("Vertical") < -0.1f && canDashJump)
            {
                Instantiate(dustPrefab, transform.position, Quaternion.identity);
                rb.velocity = new Vector2(facingDirection * 50, GetTotalJumpForce() / 2); // DashJump
                coyoteTimeCounter = 0f;
            }
            else if (coyoteTimeCounter > 0f)
            {
                Instantiate(dustPrefab, transform.position, Quaternion.identity);
                rb.velocity = new Vector2(rb.velocity.x, GetTotalJumpForce());
                coyoteTimeCounter = 0f;
            }
            else if (jumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, GetTotalJumpForce());
                jumpsLeft--;
            }
        }
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        Collider2D trampoline = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsTrampoline);
        if (trampoline)
        {
            Trampoline t = trampoline.GetComponent<Trampoline>();
            if (t)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(rb.velocity.x, jumpForce * t.force), ForceMode2D.Impulse);
            }
        }

        if (isGrounded)
        {
            jumpsLeft = extraJumps;
            lastWallJumpX = Mathf.Infinity;
        }

        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.fixedDeltaTime;

        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        canWallJump = !isGrounded && isWallDetected;
    }

    private void HandleWallSlide()
    {
        if (Mathf.Abs(transform.position.x - lastWallJumpX) < 0.5f)
        {
            isWallSliding = false;
        }
        else if (isWallDetected && canWallSlide && !isGrounded && rb.velocity.y < 5f)
        {
            isWallSliding = true;
            if(Input.GetAxis("Vertical") > -0.1f)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);
        }
        else
        {
            isWallSliding = false;
        }
        if (isGrounded) isWallSliding = false;
    }

    private void CapFallSpeed()
    {
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private void WallJump()
    {
        if (Mathf.Abs(transform.position.x - lastWallJumpX) < 0.5f) return;

        rb.velocity = Vector2.zero;
        Vector2 direction = new Vector2(
            wallJumpDirection.x * -facingDirection,
            wallJumpDirection.y + activeJumpBuff);
        rb.AddForce(direction, ForceMode2D.Impulse);

        lastWallJumpX = transform.position.x;
        canWallJump = false;
        Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        facingDirection = facingRight ? 1 : -1;
        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    // --------- Buff Methods ---------

    [SerializeField] private float maxJumpBuff = 10f;

    public void IncreaseSpeed(float buffValue)
    {
        activeSpeedBuff += buffValue;
    }

    public void DecreaseSpeed(float buffValue)
    {
        activeSpeedBuff -= buffValue;
    }

    public void IncreaseJump(float amount)
    {
        activeJumpBuff = Mathf.Min(activeJumpBuff + amount, maxJumpBuff);
    }
    public void IncreaseJumpCount(int amount)
    {
        extraJumps += amount;
        jumpsLeft += amount;
    }

    public void DecreaseJump(float amount)
    {
        activeJumpBuff -= amount;
    }
    public void DecreaseJumpCount(int amount)
    {
        extraJumps -= amount;
        if(extraJumps < 0) extraJumps = 0;
    }

    public float GetTotalJumpForce()
    {
        float total = jumpForce + activeJumpBuff;

        if (isGrounded && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsCloud))
        {
            total *= 2f; // Cloud boost
        }

        return total;
    }
    public int GetJumpCount()
    {
        return 1 + extraJumps;
    }

    public float GetActiveSpeedBuff()
    {
        return activeSpeedBuff;
    }

    public float GetActiveJumpBuff()
    {
        return activeJumpBuff;
    }

    // --------- Animations & Gizmos ---------
    private void HandleAnimations()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * facingDirection * wallCheckDistance);
    }
}
