using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarWS : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private SpriteRenderer healthVisual;
    private float startScaling;


    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    [Header("Entity")]
    [SerializeField] private IKillable killableEntity;
    //The direct parent of the Healthbar, meaning the enemy that
    //is supposed to be tracked by this

    private void Awake()
    {
        killableEntity = transform.parent.parent.GetComponent<IKillable>();
        startScaling = healthVisual.transform.localScale.x;
        health = maxHealth;
        AdjustVisuals();
    }
    private void Update()
    {
        AdjustVisuals();
    }

    public bool Hurt(int amount)
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
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        AdjustVisuals();
    }
    private void AdjustVisuals()
    {
        float percent = (float)health / maxHealth;

        healthVisual.color = gradient.Evaluate(percent);

        Vector3 currentScale = healthVisual.transform.localScale;
        currentScale.x = startScaling * percent;
        healthVisual.transform.localScale = currentScale;

        float shiftValue = CalculateDynamicShift(percent);

        Vector3 currentPosition = healthVisual.transform.localPosition;
        currentPosition.x = -shiftValue;
        healthVisual.transform.localPosition = currentPosition;
    }
    private float CalculateDynamicShift(float percent)
    {
        float a = 2.0f;
        float b = 2.5f;

        float shift = a * Mathf.Pow(1 - percent, b);
        return shift;
    }


}
