using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    public abstract void Die();
    public abstract HealthbarWS GetHealthbarWS();
}
