using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public Weapon weapon;

    public Dictionary<Monster, float> nextDamageTimes =
        new Dictionary<Monster, float>();

    public float nextAnimationTime;

    void Start()
    {
        weapon =
            GetComponentInParent<Weapon>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Monster monster =
            collision.GetComponentInParent<Monster>();

        if (monster == null)
        {
            return;
        }

        if (monster.isDead)
        {
            return;
        }

        if (
            nextDamageTimes.ContainsKey(monster) &&
            Time.time < nextDamageTimes[monster]
        )
        {
            return;
        }

        if (Time.time >= nextAnimationTime)
        {
            weapon.PlayAttackAnimation();

            nextAnimationTime =
                Time.time +
                weapon.damageInterval;
        }

        weapon.DamageMonster(monster);

        nextDamageTimes[monster] =
            Time.time +
            weapon.damageInterval;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Monster monster =
            collision.GetComponentInParent<Monster>();

        if (monster == null)
        {
            return;
        }

        if (nextDamageTimes.ContainsKey(monster))
        {
            nextDamageTimes.Remove(monster);
        }
    }
}