using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    public float damage = 10f;
    public float damageInterval = 0.5f;

    public Monster monster;
    public SpriteRenderer monsterSprite;

    public Transform armPivot;
    public Animator armAnimator;

    public Vector3 startPivotScale;

    void Start()
    {
        monster =
            GetComponentInParent<Monster>();

        if (monster != null)
        {
            monsterSprite =
                monster.GetComponent<SpriteRenderer>();
        }

        armPivot =
            transform.parent;

        if (armPivot != null)
        {
            armAnimator =
                armPivot.GetComponent<Animator>();

            startPivotScale =
                armPivot.localScale;
        }
    }

    void Update()
    {
        if (
            monster == null ||
            monsterSprite == null ||
            armPivot == null
        )
        {
            return;
        }

        if (monsterSprite.flipX)
        {
            armPivot.localScale =
                new Vector3(
                    -Mathf.Abs(startPivotScale.x),
                    startPivotScale.y,
                    startPivotScale.z
                );
        }
        else
        {
            armPivot.localScale =
                new Vector3(
                    Mathf.Abs(startPivotScale.x),
                    startPivotScale.y,
                    startPivotScale.z
                );
        }
    }

    public void PlayAttackAnimation()
    {
        if (armAnimator == null)
        {
            return;
        }

        armAnimator.ResetTrigger("Attack");
        armAnimator.SetTrigger("Attack");
    }

    public void DamagePlayer(Player player)
    {
        if (player == null)
        {
            return;
        }

        if (player.isDead)
        {
            return;
        }

        player.TakeDamage(
            damage
        );
    }
}