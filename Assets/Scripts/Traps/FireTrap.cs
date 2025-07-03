using UnityEngine;

public class FireTrap : Trap
{
    private bool _isActive = true;
    [SerializeField] private bool startActive = true;
    [SerializeField] private bool periodical = false;
    [SerializeField, Range(1, 5)] private float periodDuration = 2f; // Time to toggle the active state
    private float periodTimer = 0f;

    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Start()
    {
        isActive = startActive;
        periodTimer = periodDuration; // Start with the timer set to the period duration
    }

    private void Update()
    {
        GetComponent<Animator>().SetBool("isActive", isActive);

        if (periodical)
        {
            // Handle the periodical behavior
            periodTimer -= Time.deltaTime;

            // If the timer runs out, toggle the active state
            if (periodTimer <= 0f)
            {
                isActive = !isActive; // Toggle the state
                periodTimer = periodDuration; // Reset the timer to the period duration
            }
        }
    }
}
