using UnityEngine;

/// <summary>
/// Proximity-activated torch that automatically enables its light
/// when the player enters a specified detection range and disables
/// it when the player moves away.
/// </summary>
public class Torch : MonoBehaviour
{
    [Header("Detection")]
    private Transform player;
    [SerializeField] private float activationRange = 5f;

    [Header("Visuals")]//TODO
    [SerializeField] private Light torchLight;

    private bool isLit;
    private Animator animator;

    #region Unity Events

    /// <summary>
    /// Ensures required references are assigned before gameplay begins.
    /// Attempts to locate the player automatically if none was assigned.
    /// </summary>
    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Torch couldn't find an Animator component.");
        }
    }

    /// <summary>
    /// Checks the player's distance from the torch and updates its
    /// active state when the player enters or leaves the detection range.
    /// </summary>
    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= activationRange && !isLit)
        {
            TurnOn();
        }
    }

    #endregion

    #region Torch Control

    /// <summary>
    /// Enables the torch's lighting effects and marks it as active.
    /// </summary>
    private void TurnOn()
    {
        RespawnManager.Instance.SetCheckpoint(this);
        isLit = true;
        animator.SetBool("isLit", true);
    }

    /// <summary>
    /// Disables the torch's lighting effects and marks it as inactive.
    /// </summary>
    private void TurnOff()
    {
        isLit = false;
        animator.SetBool("isLit", false);
    }

    #endregion
}