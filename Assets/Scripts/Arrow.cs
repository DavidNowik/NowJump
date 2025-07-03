using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour, IWeaponPower
{

    [Header("Weapon info")]
    [SerializeField] private int power = 1; 
    public int Power { get => power; set => power = value; }


    [Header("HitColliderInfo")]
    [SerializeField] private Transform peak;

    [Header("PickUpInfo")]
    [SerializeField] private GameObject flashPrefab;
    [SerializeField] private BoxCollider2D coll;
    private bool stuck = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        Vector2 localPeakOffset = rb.transform.InverseTransformPoint(peak.position);
        rb.centerOfMass = localPeakOffset;
    }

    private void Update()
    {
        CheckHit();
        if (stuck) CheckPickUp();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(peak.position, 0.2f); // hit check

            Gizmos.color = Color.green;
            Bounds bounds = coll.bounds;
            bounds.Expand(bounds.size * 0.1f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    private void CheckHit()
    {
        // Check if the player is within a small radius behind the enemy
        Collider2D hitCollider = Physics2D.OverlapCircle(peak.position, 0.5f, LayerMask.GetMask("Enemy"));

        if (hitCollider != null)
            if (hitCollider.tag == "Enemy") // Check if player is detected and if the chicken isn't already jumping
            {
                Destroy(coll);
                if (stuck)
                {
                    Destroy(gameObject);
                }
                else
                {
                    hitCollider.GetComponent<IKillable>().GetHealthbarWS().Hurt(power);
                    GameManager.instance.DropText(power.ToString(), transform.position);
                    Destroy(gameObject);
                }
            }
    }
    private void CheckPickUp()
    {
        // Expand bounds by 10%
        Bounds expandedBounds = coll.bounds;
        expandedBounds.Expand(expandedBounds.size * 0.1f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(expandedBounds.center, expandedBounds.size, 0f);

        foreach (var hit in hits)
        {
            if (hit.name.StartsWith("Player"))
            {
                Debug.Log("Picked up");
                Destroy(gameObject);
                Instantiate(flashPrefab, transform.position, Quaternion.identity);
                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !stuck)
        {
            StickIntoGround();
        }
    }
    private void StickIntoGround()
    {
        stuck = true;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Optional: slightly bury the tip into the ground for looks
        transform.position += transform.right * 0.05f;
    }

}
