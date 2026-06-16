using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D platformCollider;

    private Player player;
    private Rigidbody2D playerRb;

    private bool forceDisabled;

    private float lastSPressTime;
    private float holdStartTime;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        FindPlayer();

        if (playerRb == null)
            return;

        // Holding S keeps the platform disabled.
        if (Input.GetKey(KeyCode.S))
        {
            platformCollider.enabled = false;
            return;
        }

        if (!forceDisabled)
        {
            platformCollider.enabled = playerRb.velocity.y <= 0;
        }

        HandleDropDown();
    }

    private void FindPlayer()
    {
        if (player != null)
            return;

        player = FindFirstObjectByType<Player>();

        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void HandleDropDown()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.time - lastSPressTime < 0.3f)
            {
                StartCoroutine(DisablePlatformTemporarily());
            }

            lastSPressTime = Time.time;
            holdStartTime = Time.time;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (Time.time - holdStartTime > 1f)
            {
                StartCoroutine(DisablePlatformTemporarily());
                holdStartTime = float.MaxValue;
            }
        }
    }

    private IEnumerator DisablePlatformTemporarily()
    {
        forceDisabled = true;
        platformCollider.enabled = false;

        playerRb.velocity = new Vector2(
            playerRb.velocity.x,
            -1f);

        yield return new WaitForSeconds(0.2f);

        forceDisabled = false;
    }
}