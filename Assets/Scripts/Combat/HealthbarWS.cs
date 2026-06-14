using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World-space health bar component intended to be attached as a child of an enemy.
/// Tracks the parent's health and updates the displayed health bar to reflect
/// the enemy's current health in real time.
/// </summary>
public class HealthbarWS : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private SpriteRenderer healthVisual;
    private float startScaling;
    //Starting positiong for saving manual offset, when assigning healthbar in inspector.
    private Vector3 startPosition;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    [Header("Entity")]
    [SerializeField] private IKillable killableEntity;
    // The direct parent of the Healthbar, meaning the enemy that
    // is supposed to be tracked by this.

    #region Unity Events

    /// <summary>
    /// Initializes references and default values when the object is created.
    /// Retrieves the tracked entity from the parent hierarchy, stores the
    /// original width of the health bar sprite, sets health to maximum,
    /// and updates the visual representation.
    /// </summary>
    private void Awake()
    {
        startPosition = healthVisual.transform.localPosition; 
        killableEntity = GetComponentInParent<IKillable>();
        startScaling = healthVisual.transform.localScale.x;
        health = maxHealth;
        AdjustVisuals();
    }

    /// <summary>
    /// Continuously updates the health bar visuals every frame.
    /// This ensures that any externally modified health values are
    /// immediately reflected in the displayed bar.
    /// </summary>
    private void Update()
    {
        AdjustVisuals();
    }

    #endregion

    #region Public

    /// <summary>
    /// Applies damage to the tracked entity's health.
    /// Updates the health bar visuals and triggers the entity's death
    /// behavior if health reaches zero or below.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    /// <returns>
    /// True if the entity survived the damage; otherwise false.
    /// </returns>
    public bool Hurt(float amount)
    {
        health -= amount;
        AdjustVisuals();

        if (health <= 0)
        {
            Debug.Log("Healthbar.Hurt returns false because health <= 0!");
            killableEntity.Die();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Restores health to the tracked entity.
    /// Health is clamped so it cannot exceed the configured maximum.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        AdjustVisuals();
    }

    #endregion

    #region Visual Updates

    /// <summary>
    /// Updates all visual aspects of the health bar based on the current
    /// health percentage. This includes the bar color, width, and position.
    /// </summary>
    private void AdjustVisuals()
    {
        float percent = (float)health / maxHealth;

        Vector3 scale = healthVisual.transform.localScale;
        scale.x = startScaling * percent;
        healthVisual.transform.localScale = scale;
    }


    #endregion
}