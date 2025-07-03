using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public bool carrying = false;
    private Transform playerTransform;
    public Vector3 offset = new Vector3(0.5f, 0.5f, 0); // Optional offset

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && !player.carryingEquipment)
        {
            carrying = true;
            playerTransform = player.transform;
            player.carryingEquipment = true;
            gameObject.name = "CarriedEquipment"; // Rename the GameObject
        }
    }

    void Update()
    {
        if (carrying && playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }
}
