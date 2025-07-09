using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantshooter : MonoBehaviour
{
    public float idleTime = 2f;
    public float shotSpeed = 10f; // Adjustable in the Inspector
    public float bulletLifeTime = 5f;
    public float bulletWeight = 0.2f;
    public GameObject bulletPrefab;
    public Transform shootFrom;  // Set this in the Inspector
    public bool flipped;

    private Animator anim;
    private float timer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        timer = idleTime;
        if (flipped)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Debug.Log("Shoot");
            anim.SetTrigger("shoot");
            timer = idleTime;
        }
    }

    // This method can be called by an Animation Event or directly
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootFrom.position, shootFrom.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().lifetime = bulletLifeTime;
        rb.mass = bulletWeight;
        if(flipped)
            rb.velocity = new Vector2(shotSpeed, 0);
        else
            rb.velocity = new Vector2(-shotSpeed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null ||
            collision.gameObject.tag == "Block")
        {
            Destroy(GetComponent<CapsuleCollider2D>());
            anim.SetTrigger("die");
        }
    }
    public void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
