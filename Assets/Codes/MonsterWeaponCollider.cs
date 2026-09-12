using UnityEngine;

public class MonsterWeaponCollider : MonoBehaviour
{
    public MonsterWeapon weapon;

    public float nextDamageTime;

    void Start()
    {
        weapon =
            GetComponentInParent<MonsterWeapon>();
    }

    void OnTriggerStay2D(
        Collider2D other
    )
    {
        if (
            Time.time <
            nextDamageTime
        )
        {
            return;
        }

        Player player =
            other.GetComponentInParent<Player>();

        if (player == null)
        {
            return;
        }

        if (player.isDead)
        {
            return;
        }

        weapon.PlayAttackAnimation();

        weapon.DamagePlayer(
            player
        );

        nextDamageTime =
            Time.time +
            weapon.damageInterval;
    }
}