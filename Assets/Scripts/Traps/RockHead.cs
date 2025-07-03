using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead : ActivatableObject
{
    private bool _isActive = true;
    private Animator anim;

    [SerializeField] private bool startActive = true;

    [Header("Periodic Logic")]
    [SerializeField] private bool periodical = false;
    [SerializeField, Range(0.5f, 5)] private float periodDuration = 2f; // Time to toggle the active state
    [SerializeField, Range(0, 5)] private float withDelay = 0f; // Initial delay before starting periodic switching
    private float periodTimer = 0f;
    private bool delayDone = false;

    private void Awake()
    {
        isActive = startActive;
        anim = GetComponent<Animator>();
        periodTimer = withDelay; // Start with initial delay
        delayDone = withDelay <= 0f; // Skip delay if it's 0 or less
    }

    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Update()
    {
        anim.SetBool("isActive", isActive);

        if (periodical)
        {
            periodTimer -= Time.deltaTime;

            if (!delayDone)
            {
                if (periodTimer <= 0f)
                {
                    delayDone = true;
                    periodTimer = periodDuration; // Start the periodic timer after delay
                }
            }
            else
            {
                if (periodTimer <= 0f)
                {
                    isActive = !isActive;
                    periodTimer = periodDuration;
                }
            }
        }
    }
}
