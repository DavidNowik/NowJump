using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    [SerializeField] private KeyCode attackKey;

    // Prevents attack spam while an attack animation is playing.
    private bool canAttack = true;

    public Weapon Weapon { get; private set; }

    private Animator[] animators;

    private void Awake()
    {
        Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }


    /// <summary>
    /// Finds the current Weapon and all child Animators.
    /// Weapon speed is applied to all animators, affecting attack cadence.
    /// </summary>
    public void Refresh()
    {
        Weapon = GetComponentInChildren<Weapon>(true);
        animators = GetComponentsInChildren<Animator>(true);

        foreach (Animator animator in animators)
        {
            animator.speed = Weapon.speed;
        }
    }

    /// <summary>
    /// Starts an attack if the previous attack has finished.
    /// </summary>
    public void Attack()
    {
        if (!canAttack)
            return;

        canAttack = false;

        foreach (Animator animator in animators)
        {
            animator.SetTrigger("attack");
        }
    }

    /// <summary>
    /// Called by an Animation Event near the end of the attack animation.
    /// Re-enables attacking.
    /// </summary>
    public void AllowNextAttack()
    {
        canAttack = true;
    }

}