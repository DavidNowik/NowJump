using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantshooter : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
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
}
