using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public Player player;

    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text combatPowerText;
    public TMP_Text hpText;

    public Image hpFill;

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

        nameText.text =
            player.characterName;

        levelText.text =
            "Lv. " + player.level;

        combatPowerText.text =
            "ХѕБо : " +
            player.GetCombatPower().ToString("F1");

        hpText.text =
            "Hp " +
            player.hp.ToString("F1") +
            " / " +
            player.GetMaxHp().ToString("F1");

        hpFill.fillAmount =
            player.hp / player.GetMaxHp();
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