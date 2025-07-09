using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // You can set this in the Inspector or via script
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            GameManager.instance.canvasManager.Reset(true);
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }
}
