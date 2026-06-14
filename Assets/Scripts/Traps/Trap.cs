using UnityEngine;

public abstract class Trap : ActivatableObject
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;



        Debug.Log($"RespawnManager.Instance: {RespawnManager.Instance}");

        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Killed by "+name);
            RespawnManager.Instance.RespawnPlayer(collision.GetComponent<Player>());
        }
    }
}
