using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damageInterval = 0.2f;

    public SpriteRenderer spriteRenderer;

    public Player player;
    public SpriteRenderer playerSprite;

    public Animator armAnimator;
    public Transform armPivot;

    public Vector3 startPivotScale;

    void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        player =
            GetComponentInParent<Player>();

        if (player != null)
        {
            playerSprite =
                player.GetComponent<SpriteRenderer>();
        }

        armAnimator =
            GetComponentInParent<Animator>();

        if (armAnimator != null)
        {
            armPivot =
                armAnimator.transform;
        }

        if (armPivot != null)
        {
            startPivotScale =
                armPivot.localScale;
        }
    }

    void Update()
    {
        if (
            player == null ||
            playerSprite == null ||
            armPivot == null
        )
        {
            return;
        }

        if (playerSprite.flipX)
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

    public void ChangeWeapon(
        Sprite newSprite,
        float attackMultiplier,
        Vector3 newScale
    )
    {
        spriteRenderer.sprite =
            newSprite;

        transform.localScale =
            newScale;

        player.weaponAttackMultiplier =
            attackMultiplier;
    }

    public void DamageMonster(Monster monster)
    {
        if (monster == null)
        {
            return;
        }

        if (monster.isDead)
        {
            return;
        }

        monster.TakeDamage(
            player.GetAttackDamage()
        );
    }
}