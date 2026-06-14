using UnityEngine;

public class FollowPivot : MonoBehaviour
{
    private Player player;

    private void LateUpdate()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();

            if (player == null)
                return;
        }

        transform.position = player.transform.position;
    }
}