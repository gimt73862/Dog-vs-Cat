using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 11f;

    public string characterName;
    public int level = 1;
    public int killCount = 0;

    public float attack = 10f;
    public float defense = 10f;
    public float maxHp = 100f;
    public float hp = 100f;

    public float weaponAttackMultiplier = 1f;

    public int attackUpgradeLevel = 1;
    public int defenseUpgradeLevel = 1;
    public int hpUpgradeLevel = 1;

    public int gold;
    public int equipmentTickets;
    public int characterTickets;

    public float autoHuntDelay = 1f;
    public float autoJumpHeight = 2f;
    public float autoDropHeight = 2f;
    public float autoHorizontalTolerance = 0.2f;
    public float autoAttackDistance = 0.6f;
    public float autoPlatformMinHeight = 0.3f;
    public float autoPlatformMaxHeight = 4f;
    public float autoPlatformEdgeMargin = 0.4f;

    public HuntPlatform targetPlatform;

    public float dropDownSpeed = 3f;

    public bool isGrounded;
    public bool isDead;
    public bool isDroppingDown;

    public float lastInputTime;

    public Monster targetMonster;
    public Collider2D currentGroundCollider;

    public Vector2 deadColliderSize = new Vector2(1.2f, 0.5f);
    public Vector2 deadColliderOffset = new Vector2(0f, -0.25f);

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D capsuleCollider;
    public Weapon weapon;
    public GameObject armPivot;

    public GameObject blackScreen;

    public float corpseTime = 1f;
    public float blackScreenTime = 2f;

    public Vector2 aliveColliderSize;
    public Vector2 aliveColliderOffset;
    public CapsuleDirection2D aliveColliderDirection;

    public enum CharacterTrait
    {
        None,
        MoveSpeedUp,
        AttackSpeedUp
    }

    public CharacterTrait characterTrait;
    public float traitValue = 0.05f;

    public float baseMoveSpeed;
    public float baseDamageInterval;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        weapon = GetComponentInChildren<Weapon>();

        if (weapon != null)
        {
            armPivot = weapon.transform.parent.gameObject;
        }

        lastInputTime = Time.time;

        aliveColliderSize = capsuleCollider.size;
        aliveColliderOffset = capsuleCollider.offset;
        aliveColliderDirection = capsuleCollider.direction;

        ApplyCharacterTrait();
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
        bool downPressed = Input.GetKeyDown(KeyCode.S);

        if (horizontal != 0 || jumpPressed || downPressed)
        {
            lastInputTime = Time.time;
            targetMonster = null;

            ManualMovement(
                horizontal,
                jumpPressed,
                downPressed
            );
        }
        else if (Time.time - lastInputTime >= autoHuntDelay)
        {
            AutoHunt();
        }
        else
        {
            rb.linearVelocity =
                new Vector2(
                    0,
                    rb.linearVelocity.y
                );

            animator.SetBool(
                "isMoving",
                false
            );
        }
    }

    public void ApplyCharacterTrait()
    {
        baseMoveSpeed = moveSpeed;

        if (weapon != null)
        {
            baseDamageInterval =
                weapon.damageInterval;
        }

        if (
            characterTrait ==
            CharacterTrait.MoveSpeedUp
        )
        {
            moveSpeed =
                baseMoveSpeed *
                (1f + traitValue);
        }
        else if (
            characterTrait ==
            CharacterTrait.AttackSpeedUp
        )
        {
            if (weapon != null)
            {
                weapon.damageInterval =
                    baseDamageInterval /
                    (1f + traitValue);
            }
        }
    }

    public void ManualMovement(
        float horizontal,
        bool jumpPressed,
        bool downPressed
    )
    {
        rb.linearVelocity =
            new Vector2(
                horizontal * moveSpeed,
                rb.linearVelocity.y
            );

        if (
            downPressed &&
            isGrounded
        )
        {
            TryDropDown();
        }
        else if (
            jumpPressed &&
            isGrounded
        )
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

            isGrounded = false;
        }

        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        animator.SetBool(
            "isMoving",
            horizontal != 0
        );
    }

    public void AutoHunt()
    {
        if (isDroppingDown)
        {
            return;
        }

        if (
            targetMonster == null ||
            targetMonster.isDead
        )
        {
            FindClosestMonster();
            targetPlatform = null;
        }

        if (targetMonster == null)
        {
            rb.linearVelocity =
                new Vector2(
                    0,
                    rb.linearVelocity.y
                );

            animator.SetBool(
                "isMoving",
                false
            );

            return;
        }

        float heightDifference =
            targetMonster.transform.position.y -
            transform.position.y;

        if (
            targetPlatform != null &&
            currentGroundCollider ==
            targetPlatform.platformCollider
        )
        {
            targetPlatform = null;
        }

        if (
            heightDifference >
            autoJumpHeight
        )
        {
            if (targetPlatform == null)
            {
                targetPlatform =
                    FindNextUpperPlatform(
                        targetMonster.transform.position.y
                    );
            }

            if (targetPlatform != null)
            {
                MoveToPlatform(
                    targetPlatform
                );

                return;
            }
        }
        else
        {
            targetPlatform = null;
        }

        float monsterXDifference =
            targetMonster.transform.position.x -
            transform.position.x;

        if (
            Mathf.Abs(monsterXDifference) >
            0.01f
        )
        {
            spriteRenderer.flipX =
                monsterXDifference < 0f;
        }

        float directionToMonster =
            spriteRenderer.flipX ?
            -1f :
            1f;

        float targetX =
            targetMonster.transform.position.x -
            directionToMonster *
            autoAttackDistance;

        float xDifference =
            targetX -
            transform.position.x;

        if (
            heightDifference <
            -autoDropHeight &&
            isGrounded
        )
        {
            TryDropDown();
            return;
        }

        if (
            Mathf.Abs(xDifference) >
            autoHorizontalTolerance
        )
        {
            float direction =
                Mathf.Sign(xDifference);

            rb.linearVelocity =
                new Vector2(
                    direction * moveSpeed,
                    rb.linearVelocity.y
                );

            animator.SetBool(
                "isMoving",
                true
            );

            spriteRenderer.flipX =
                direction < 0f;
        }
        else
        {
            rb.linearVelocity =
                new Vector2(
                    0,
                    rb.linearVelocity.y
                );

            animator.SetBool(
                "isMoving",
                false
            );
        }

        if (
            heightDifference >
            autoJumpHeight &&
            isGrounded
        )
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

            isGrounded = false;
        }
    }

    public HuntPlatform FindNextUpperPlatform(
        float monsterY
    )
    {
        HuntPlatform[] platforms =
            FindObjectsByType<HuntPlatform>(
                FindObjectsSortMode.None
            );

        HuntPlatform bestPlatform = null;

        float currentPlatformY;

        if (currentGroundCollider != null)
        {
            currentPlatformY =
                currentGroundCollider.bounds.max.y;
        }
        else
        {
            currentPlatformY =
                capsuleCollider.bounds.min.y;
        }

        float bestHeight =
            Mathf.Infinity;

        float bestHorizontalDistance =
            Mathf.Infinity;

        foreach (
            HuntPlatform platform
            in platforms
        )
        {
            if (
                platform == null ||
                platform.platformCollider == null
            )
            {
                continue;
            }

            Collider2D platformCollider =
                platform.platformCollider;

            float platformY =
                platformCollider.bounds.max.y;

            float height =
                platformY -
                currentPlatformY;

            if (
                height <
                autoPlatformMinHeight
            )
            {
                continue;
            }

            if (
                height >
                autoPlatformMaxHeight
            )
            {
                continue;
            }

            if (
                platformY >
                monsterY + 1f
            )
            {
                continue;
            }

            float left =
                platformCollider.bounds.min.x +
                autoPlatformEdgeMargin;

            float right =
                platformCollider.bounds.max.x -
                autoPlatformEdgeMargin;

            float nearestX =
                Mathf.Clamp(
                    transform.position.x,
                    left,
                    right
                );

            float horizontalDistance =
                Mathf.Abs(
                    nearestX -
                    transform.position.x
                );

            if (
                height <
                bestHeight - 0.1f
            )
            {
                bestHeight =
                    height;

                bestHorizontalDistance =
                    horizontalDistance;

                bestPlatform =
                    platform;
            }
            else if (
                Mathf.Abs(
                    height -
                    bestHeight
                ) <= 0.1f &&
                horizontalDistance <
                bestHorizontalDistance
            )
            {
                bestHorizontalDistance =
                    horizontalDistance;

                bestPlatform =
                    platform;
            }
        }

        return bestPlatform;
    }

    public void MoveToPlatform(
        HuntPlatform platform
    )
    {
        if (
            platform == null ||
            platform.platformCollider == null
        )
        {
            targetPlatform = null;
            return;
        }

        Collider2D platformCollider =
            platform.platformCollider;

        float left =
            platformCollider.bounds.min.x +
            autoPlatformEdgeMargin;

        float right =
            platformCollider.bounds.max.x -
            autoPlatformEdgeMargin;

        bool insidePlatformX =
            transform.position.x >= left &&
            transform.position.x <= right;

        if (!insidePlatformX)
        {
            float targetX =
                transform.position.x < left ?
                left :
                right;

            float direction =
                Mathf.Sign(
                    targetX -
                    transform.position.x
                );

            rb.linearVelocity =
                new Vector2(
                    direction * moveSpeed,
                    rb.linearVelocity.y
                );

            spriteRenderer.flipX =
                direction < 0f;

            animator.SetBool(
                "isMoving",
                true
            );

            return;
        }

        rb.linearVelocity =
            new Vector2(
                0,
                rb.linearVelocity.y
            );

        animator.SetBool(
            "isMoving",
            false
        );

        if (isGrounded)
        {
            rb.linearVelocity =
                new Vector2(
                    0,
                    jumpForce
                );

            isGrounded = false;
        }
    }

    public void FindClosestMonster()
    {
        Monster[] monsters =
            FindObjectsByType<Monster>(
                FindObjectsSortMode.None
            );

        targetMonster = null;

        float closestDistance =
            Mathf.Infinity;

        foreach (
            Monster monster
            in monsters
        )
        {
            if (monster.isDead)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    monster.transform.position
                );

            if (
                distance <
                closestDistance
            )
            {
                closestDistance =
                    distance;

                targetMonster =
                    monster;
            }
        }
    }

    public void TryDropDown()
    {
        if (isDroppingDown)
        {
            return;
        }

        if (
            currentGroundCollider ==
            null
        )
        {
            return;
        }

        PlatformEffector2D platform =
            currentGroundCollider
            .GetComponentInParent<PlatformEffector2D>();

        if (platform == null)
        {
            return;
        }

        StartCoroutine(
            DropDown()
        );
    }

    public IEnumerator DropDown()
    {
        isDroppingDown = true;
        isGrounded = false;

        Collider2D platformCollider =
            currentGroundCollider;

        Physics2D.IgnoreCollision(
            capsuleCollider,
            platformCollider,
            true
        );

        rb.linearVelocity =
            new Vector2(
                rb.linearVelocity.x,
                -dropDownSpeed
            );

        while (
            platformCollider != null &&
            capsuleCollider.bounds.max.y >
            platformCollider.bounds.min.y
        )
        {
            yield return null;
        }

        if (
            platformCollider !=
            null
        )
        {
            Physics2D.IgnoreCollision(
                capsuleCollider,
                platformCollider,
                false
            );
        }

        isDroppingDown = false;
    }

    public float GetLevelMultiplier()
    {
        return
            1f +
            (level - 1) * 0.01f;
    }

    public float GetAttackDamage()
    {
        return
            attack *
            weaponAttackMultiplier *
            GetLevelMultiplier();
    }

    public float GetDefense()
    {
        return
            defense *
            GetLevelMultiplier();
    }

    public float GetMaxHp()
    {
        return
            maxHp *
            GetLevelMultiplier();
    }

    public float GetCombatPower()
    {
        return
            GetAttackDamage() +
            GetDefense() +
            GetMaxHp();
    }

    public int GetRequiredKills()
    {
        return
            10 *
            (int)Mathf.Pow(
                2,
                level - 1
            );
    }

    public int GetUpgradeCost(
        int upgradeLevel
    )
    {
        return
            20 +
            (upgradeLevel - 1) * 10;
    }

    public void AddKill()
    {
        killCount++;

        if (
            killCount >=
            GetRequiredKills()
        )
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        float oldMaxHp =
            GetMaxHp();

        level++;
        killCount = 0;

        float newMaxHp =
            GetMaxHp();

        hp +=
            newMaxHp -
            oldMaxHp;

        if (
            hp >
            newMaxHp
        )
        {
            hp =
                newMaxHp;
        }
    }

    public void TakeDamage(
        float damage
    )
    {
        if (isDead)
        {
            return;
        }

        float damageReduction =
            GetDefense() *
            0.001f;

        damageReduction =
            Mathf.Clamp(
                damageReduction,
                0f,
                1f
            );

        float finalDamage =
            damage *
            (1f - damageReduction);

        hp -= finalDamage;

        if (
            hp <= 0f
        )
        {
            hp = 0f;
            Die();
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        rb.linearVelocity =
            Vector2.zero;

        animator.SetBool(
            "isMoving",
            false
        );

        animator.SetBool(
            "isDead",
            true
        );

        if (
            armPivot != null
        )
        {
            armPivot.SetActive(
                false
            );
        }

        if (
            capsuleCollider !=
            null
        )
        {
            ChangeDeadCollider();
        }

        rb.bodyType =
            RigidbodyType2D.Static;

        StartCoroutine(
            Revive()
        );
    }

    public IEnumerator Revive()
    {
        yield return
            new WaitForSecondsRealtime(
                corpseTime
            );

        StageManager stageManager =
            FindFirstObjectByType<StageManager>();

        if (stageManager != null)
        {
            stageManager.HideNormalUI();
        }

        if (
            blackScreen !=
            null
        )
        {
            blackScreen.SetActive(
                true
            );
        }

        yield return
            new WaitForSecondsRealtime(
                blackScreenTime
            );

        hp =
            GetMaxHp();

        capsuleCollider.direction =
            aliveColliderDirection;

        capsuleCollider.size =
            aliveColliderSize;

        capsuleCollider.offset =
            aliveColliderOffset;

        rb.bodyType =
            RigidbodyType2D.Dynamic;

        animator.SetBool(
            "isDead",
            false
        );

        if (
            armPivot != null
        )
        {
            armPivot.SetActive(
                true
            );
        }

        targetMonster =
            null;

        lastInputTime =
            Time.time;

        isGrounded =
            false;

        isDroppingDown =
            false;

        isDead =
            false;

        if (
            blackScreen !=
            null
        )
        {
            blackScreen.SetActive(
                false
            );
        }

        if (stageManager != null)
        {
            stageManager.ShowNormalUI();
        }
    }

    public void ChangeDeadCollider()
    {
        float oldBottom =
            capsuleCollider.offset.y -
            capsuleCollider.size.y /
            2f;

        capsuleCollider.direction =
            CapsuleDirection2D.Horizontal;

        capsuleCollider.size =
            deadColliderSize;

        capsuleCollider.offset =
            deadColliderOffset;

        float newBottom =
            capsuleCollider.offset.y -
            capsuleCollider.size.y /
            2f;

        float difference =
            oldBottom -
            newBottom;

        transform.position +=
            new Vector3(
                0,
                difference,
                0
            );
    }

    public void AddGold(
        int amount
    )
    {
        gold += amount;
    }

    public void AddEquipmentTicket(
        int amount
    )
    {
        equipmentTickets +=
            amount;
    }

    public void AddCharacterTicket(
        int amount
    )
    {
        characterTickets +=
            amount;
    }

    public void UpgradeAttack()
    {
        int cost =
            GetUpgradeCost(
                attackUpgradeLevel
            );

        if (
            gold >= cost
        )
        {
            gold -= cost;

            attack += 1f;

            attackUpgradeLevel++;
        }
    }

    public void UpgradeDefense()
    {
        int cost =
            GetUpgradeCost(
                defenseUpgradeLevel
            );

        if (
            gold >= cost
        )
        {
            gold -= cost;

            defense += 1f;

            defenseUpgradeLevel++;
        }
    }

    public void UpgradeHp()
    {
        int cost =
            GetUpgradeCost(
                hpUpgradeLevel
            );

        if (
            gold >= cost
        )
        {
            float oldMaxHp =
                GetMaxHp();

            gold -= cost;

            maxHp += 10f;

            hpUpgradeLevel++;

            float newMaxHp =
                GetMaxHp();

            hp +=
                newMaxHp -
                oldMaxHp;

            if (
                hp >
                newMaxHp
            )
            {
                hp =
                    newMaxHp;
            }
        }
    }

    void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        if (
            collision.gameObject
            .CompareTag("Ground")
        )
        {
            isGrounded = true;

            currentGroundCollider =
                collision.collider;
        }
    }

    void OnCollisionExit2D(
        Collision2D collision
    )
    {
        if (
            collision.gameObject
            .CompareTag("Ground")
        )
        {
            isGrounded = false;

            if (
                currentGroundCollider ==
                collision.collider
            )
            {
                currentGroundCollider =
                    null;
            }
        }
    }
}