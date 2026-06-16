
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IKillable killable = other.GetComponent<IKillable>();

        if (killable == null)
            return;


        killable.GetHealthbarWS().Hurt(weapon.damage);
    }
}