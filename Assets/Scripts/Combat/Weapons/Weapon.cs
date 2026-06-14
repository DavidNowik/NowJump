using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float speed = 1;
    public float damage;

    protected bool canAttack = true;
}