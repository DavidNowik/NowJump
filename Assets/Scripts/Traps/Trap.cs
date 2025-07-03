// Trap base class: handles general trap behavior
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Trap : ActivatableObject
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Killed by "+name);
            GameManager.instance.canvasManager.Reset(true);
        }
    }
}
