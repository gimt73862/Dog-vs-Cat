using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public Player player;

    public TMP_Text goldText;
    public TMP_Text equipmentTicketText;
    public TMP_Text characterTicketText;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();

            if (player == null)
            {
                return;
            }
        }

        goldText.text =
            player.gold.ToString();

        equipmentTicketText.text =
            player.equipmentTickets.ToString();

        characterTicketText.text =
            player.characterTickets.ToString();
    }

    public void FindPlayer()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player =
                playerObject.GetComponent<Player>();
        }
    }
}