using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 7f;
    public float detectionRange = 8f;
    public float stopDistance = 0.8f;

    public float hp = 100f;

    public float platformHeightTolerance = 0.5f;
    public float platformEdgeMargin = 0.4f;
    public float dropDownSpeed = 3f;

    public Vector2 deadColliderSize =
        new Vector2(1.2f, 0.5f);

    public Vector2 deadColliderOffset =
        new Vector2(0f, -0.25f);

    public GameObject goldPrefab;
    public GameObject equipmentTicketPrefab;
    public GameObject characterTicketPrefab;

    public int goldReward = 10;

    public float equipmentTicketChance = 10f;
    public float characterTicketChance = 5f;

    public Transform player;
    public Player playerScript;
    public StageManager stageManager;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public MonsterWeapon weapon;
    public GameObject armPivot;
    public Rigidbody2D rb;

    public CapsuleCollider2D capsuleCollider;
    public CapsuleCollider2D playerCollider;

    public Collider2D currentGroundCollider;

    public HuntPlatform targetPlatform;

    public bool isGrounded;
    public bool isDead;
    public bool isDroppingDown;

    public List<ItemDrop> droppedItems =
        new List<ItemDrop>();

    void Start()
    {
        animator =
            GetComponent<Animator>();

        spriteRenderer =
            GetComponent<SpriteRenderer>();

        weapon =
            GetComponentInChildren<MonsterWeapon>();

        if (weapon != null)
        {
            armPivot =
                weapon.transform.parent.gameObject;
        }

        rb =
            GetComponent<Rigidbody2D>();

        capsuleCollider =
            GetComponent<CapsuleCollider2D>();

        stageManager =
            FindFirstObjectByType<StageManager>();

        FindPlayer();
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (
            player == null ||
            playerScript == null
        )
        {
            FindPlayer();

            if (
                player == null ||
                playerScript == null
            )
            {
                return;
            }
        }

        if (playerScript.isDead)
        {
            rb.linearVelocity =
                Vector2.zero;

            animator.SetBool(
                "isMoving",
                false
            );

            return;
        }

        if (isDroppingDown)
        {
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distance > detectionRange)
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

        HandlePlatformMovement();
    }

    public void FindPlayer()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (playerObject == null)
        {
            player = null;
            playerScript = null;
            return;
        }

        player =
            playerObject.transform;

        playerScript =
            playerObject.GetComponent<Player>();

        playerCollider =
            playerObject.GetComponent<CapsuleCollider2D>();

        if (
            capsuleCollider != null &&
            playerCollider != null
        )
        {
            Physics2D.IgnoreCollision(
                capsuleCollider,
                playerCollider,
                true
            );
        }
    }

    public void HandlePlatformMovement()
    {
        float monsterGroundY =
            GetMonsterGroundY();

        float playerGroundY =
            GetPlayerGroundY();

        bool playerAbove =
            playerGroundY >
            monsterGroundY +
            platformHeightTolerance;

        bool playerBelow =
            playerGroundY <
            monsterGroundY -
            platformHeightTolerance;

        if (playerAbove)
        {
            HuntPlatform nextPlatform =
                FindNextUpperPlatform(
                    playerGroundY
                );

            if (nextPlatform != null)
            {
                targetPlatform =
                    nextPlatform;

                MoveToUpperPlatform(
                    targetPlatform
                );

                return;
            }
        }

        if (playerBelow)
        {
            targetPlatform = null;

            MoveToDropPosition();

            return;
        }

        targetPlatform = null;

        ChasePlayer();
    }

    public void MoveToDropPosition()
    {
        if (currentGroundCollider == null)
        {
            ChasePlayer();
            return;
        }

        PlatformEffector2D platform =
            currentGroundCollider
            .GetComponentInParent<PlatformEffector2D>();

        if (platform == null)
        {
            ChasePlayer();
            return;
        }

        float leftEdge =
            currentGroundCollider.bounds.min.x +
            platformEdgeMargin;

        float rightEdge =
            currentGroundCollider.bounds.max.x -
            platformEdgeMargin;

        float targetX =
            Mathf.Clamp(
                player.position.x,
                leftEdge,
                rightEdge
            );

        float xDifference =
            targetX -
            transform.position.x;

        if (
            Mathf.Abs(xDifference) >
            stopDistance
        )
        {
            float direction =
                Mathf.Sign(xDifference);

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
            TryDropDown();
        }
    }

    public float GetMonsterGroundY()
    {
        if (currentGroundCollider != null)
        {
            return
                currentGroundCollider
                .bounds.max.y;
        }

        if (capsuleCollider != null)
        {
            return
                capsuleCollider
                .bounds.min.y;
        }

        return transform.position.y;
    }

    public float GetPlayerGroundY()
    {
        if (
            playerScript != null &&
            playerScript.currentGroundCollider != null
        )
        {
            return
                playerScript
                .currentGroundCollider
                .bounds.max.y;
        }

        if (
            playerScript != null &&
            playerScript.capsuleCollider != null
        )
        {
            return
                playerScript
                .capsuleCollider
                .bounds.min.y;
        }

        return player.position.y;
    }

    public HuntPlatform FindNextUpperPlatform(
        float playerGroundY
    )
    {
        HuntPlatform[] platforms =
            FindObjectsByType<HuntPlatform>(
                FindObjectsSortMode.None
            );

        HuntPlatform bestPlatform = null;

        float monsterGroundY =
            GetMonsterGroundY();

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

            if (
                platformY <=
                monsterGroundY +
                platformHeightTolerance
            )
            {
                continue;
            }

            if (
                platformY >
                playerGroundY +
                platformHeightTolerance
            )
            {
                continue;
            }

            float heightDifference =
                platformY -
                monsterGroundY;

            float left =
                platformCollider.bounds.min.x +
                platformEdgeMargin;

            float right =
                platformCollider.bounds.max.x -
                platformEdgeMargin;

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
                heightDifference <
                bestHeight - 0.1f
            )
            {
                bestHeight =
                    heightDifference;

                bestHorizontalDistance =
                    horizontalDistance;

                bestPlatform =
                    platform;
            }
            else if (
                Mathf.Abs(
                    heightDifference -
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

    public void MoveToUpperPlatform(
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
            platformEdgeMargin;

        float right =
            platformCollider.bounds.max.x -
            platformEdgeMargin;

        bool insidePlatformX =
            transform.position.x >= left &&
            transform.position.x <= right;

        if (!insidePlatformX)
        {
            float targetX;

            if (transform.position.x < left)
            {
                targetX = left;
            }
            else
            {
                targetX = right;
            }

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

    public void ChasePlayer()
    {
        float xDifference =
            player.position.x -
            transform.position.x;

        float targetX =
            player.position.x;

        if (
            currentGroundCollider != null &&
            currentGroundCollider
            .GetComponentInParent<PlatformEffector2D>() != null
        )
        {
            float leftEdge =
                currentGroundCollider.bounds.min.x +
                platformEdgeMargin;

            float rightEdge =
                currentGroundCollider.bounds.max.x -
                platformEdgeMargin;

            targetX =
                Mathf.Clamp(
                    targetX,
                    leftEdge,
                    rightEdge
                );
        }

        float moveDifference =
            targetX -
            transform.position.x;

        if (
            Mathf.Abs(moveDifference) <=
            stopDistance
        )
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
        else
        {
            float direction =
                Mathf.Sign(
                    moveDifference
                );

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
    }

    public void TryDropDown()
    {
        if (isDroppingDown)
        {
            return;
        }

        if (currentGroundCollider == null)
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

        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(
                capsuleCollider,
                platformCollider,
                false
            );
        }

        currentGroundCollider = null;
        isDroppingDown = false;
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
                currentGroundCollider = null;
            }
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

        hp -= damage;

        if (hp <= 0f)
        {
            hp = 0f;

            StartCoroutine(
                Die()
            );
        }
    }

    public IEnumerator Die()
    {
        isDead = true;

        if (playerScript != null)
        {
            playerScript.AddKill();
        }

        if (stageManager != null)
        {
            stageManager.AddStageKill();
        }

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

        if (armPivot != null)
        {
            armPivot.SetActive(false);
        }

        if (capsuleCollider != null)
        {
            ChangeDeadCollider();

            capsuleCollider.isTrigger =
                true;
        }

        rb.bodyType =
            RigidbodyType2D.Static;

        DropItems();

        yield return
            new WaitForSeconds(1f);

        foreach (
            ItemDrop item
            in droppedItems
        )
        {
            if (item != null)
            {
                item.StartMovingToPlayer();
            }
        }

        Destroy(gameObject);
    }

    public void DropItems()
    {
        if (goldPrefab != null)
        {
            GameObject goldObject =
                Instantiate(
                    goldPrefab,
                    transform.position +
                    Vector3.up * 0.5f,
                    Quaternion.identity
                );

            ItemDrop goldItem =
                goldObject
                .GetComponent<ItemDrop>();

            if (goldItem != null)
            {
                goldItem.amount =
                    goldReward;

                droppedItems.Add(
                    goldItem
                );
            }
        }

        float equipmentRoll =
            Random.Range(
                0f,
                100f
            );

        if (
            equipmentRoll <
            equipmentTicketChance &&
            equipmentTicketPrefab != null
        )
        {
            GameObject ticketObject =
                Instantiate(
                    equipmentTicketPrefab,
                    transform.position +
                    new Vector3(
                        -0.4f,
                        0.5f,
                        0f
                    ),
                    Quaternion.identity
                );

            ItemDrop item =
                ticketObject
                .GetComponent<ItemDrop>();

            if (item != null)
            {
                droppedItems.Add(
                    item
                );
            }
        }

        float characterRoll =
            Random.Range(
                0f,
                100f
            );

        if (
            characterRoll <
            characterTicketChance &&
            characterTicketPrefab != null
        )
        {
            GameObject ticketObject =
                Instantiate(
                    characterTicketPrefab,
                    transform.position +
                    new Vector3(
                        0.4f,
                        0.5f,
                        0f
                    ),
                    Quaternion.identity
                );

            ItemDrop item =
                ticketObject
                .GetComponent<ItemDrop>();

            if (item != null)
            {
                droppedItems.Add(
                    item
                );
            }
        }
    }

    public void ChangeDeadCollider()
    {
        float oldBottom =
            capsuleCollider.offset.y -
            capsuleCollider.size.y / 2f;

        capsuleCollider.direction =
            CapsuleDirection2D.Horizontal;

        capsuleCollider.size =
            deadColliderSize;

        capsuleCollider.offset =
            deadColliderOffset;

        float newBottom =
            capsuleCollider.offset.y -
            capsuleCollider.size.y / 2f;

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
}