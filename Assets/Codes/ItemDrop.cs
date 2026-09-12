using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public enum ItemType
    {
        Gold,
        EquipmentTicket,
        CharacterTicket
    }

    public ItemType itemType;
    public int amount = 1;
    public float moveSpeed = 8f;

    private Transform player;
    private bool moveToPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!moveToPlayer)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    public void StartMovingToPlayer()
    {
        moveToPlayer = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Player playerScript = other.GetComponent<Player>();

        if (playerScript == null)
        {
            return;
        }

        if (itemType == ItemType.Gold)
        {
            playerScript.AddGold(amount);
        }
        else if (itemType == ItemType.EquipmentTicket)
        {
            playerScript.AddEquipmentTicket(amount);
        }
        else if (itemType == ItemType.CharacterTicket)
        {
            playerScript.AddCharacterTicket(amount);
        }

        Destroy(gameObject);
    }
}