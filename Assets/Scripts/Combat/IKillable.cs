
public interface IKillable
{
    public abstract void Die();
    public abstract HealthbarWS GetHealthbarWS();

    /* BASE IMPLEMENTATION - not making this an abstract class to keep
     * flexibility for different types of entities that might want inheritance
     * 
    private HealthbarWS healthbar; 
    public HealthbarWS GetHealthbarWS()
    {
        if (healthbar == null)
        {
            healthbar = GetComponentInChildren<HealthbarWS>(true);
            if (healthbar == null)
            {
                Debug.LogError($"{name} could not find a " +
                    $"{nameof(HealthbarWS)} in itself or its children.");
            }
        }
        return healthbar;
    }
     * 
     */
}
