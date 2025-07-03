using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goggles : MonoBehaviour
{
    private void Update()
    {
        CheckPlayer();
    }
    private void CheckPlayer()
    {
        // Check if the player is within a small radius behind the enemy
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Player"));

        if (hitCollider != null)
        {
            if (hitCollider.name == "Player") // Check if player is detected and if the chicken isn't already jumping
            {
                CameraMotor.lookAround = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
