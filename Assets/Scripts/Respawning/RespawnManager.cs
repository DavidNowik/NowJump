using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private Torch currentCheckpoint;

    public void SetCheckpoint(Torch torch)
    {
        currentCheckpoint = torch;
    }

    public Vector3 GetRespawnPosition()
    {
        return currentCheckpoint.transform.position;
    }

    /// <summary>
    /// Respawns the specified player at the currently active checkpoint.
    /// </summary>
    /// <param name="player">The player to respawn.</param>
    public void RespawnPlayer(Player player)
    {
        if (currentCheckpoint == null)
        {
            Debug.LogWarning("No checkpoint is currently active.");
            return;
        }

        player.transform.position = currentCheckpoint.transform.position;
    }
}