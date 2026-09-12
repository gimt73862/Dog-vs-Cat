using TMPro;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public Player player;

    public GameObject levelUpPanel;

    public TMP_Text attackLevelText;
    public TMP_Text attackValueText;
    public TMP_Text attackNextValueText;
    public TMP_Text attackCostText;

    public TMP_Text defenseLevelText;
    public TMP_Text defenseValueText;
    public TMP_Text defenseNextValueText;
    public TMP_Text defenseCostText;

    public TMP_Text hpLevelText;
    public TMP_Text hpValueText;
    public TMP_Text hpNextValueText;
    public TMP_Text hpCostText;

    void Start()
    {
        FindPlayer();

        levelUpPanel.SetActive(false);
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

    public void OpenLevelUpPanel()
    {
        FindPlayer();

        if (player == null)
        {
            return;
        }

        levelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        UpdateUI();
    }

    public void CloseLevelUpPanel()
    {
        levelUpPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void UpgradeAttack()
    {
        FindPlayer();

        if (player == null)
        {
            return;
        }

        player.UpgradeAttack();

        UpdateUI();
    }

    public void UpgradeDefense()
    {
        FindPlayer();

        if (player == null)
        {
            return;
        }

        player.UpgradeDefense();

        UpdateUI();
    }

    public void UpgradeHp()
    {
        FindPlayer();

        if (player == null)
        {
            return;
        }

        player.UpgradeHp();

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (player == null)
        {
            FindPlayer();

            if (player == null)
            {
                return;
            }
        }

        attackLevelText.text =
            "Lv. " + player.attackUpgradeLevel;

        attackValueText.text =
            player.attack.ToString("F1");

        attackNextValueText.text =
            (player.attack + 1f).ToString("F1");

        attackCostText.text =
            player.GetUpgradeCost(
                player.attackUpgradeLevel
            ).ToString();

        defenseLevelText.text =
            "Lv. " + player.defenseUpgradeLevel;

        defenseValueText.text =
            player.defense.ToString("F1");

        defenseNextValueText.text =
            (player.defense + 1f).ToString("F1");

        defenseCostText.text =
            player.GetUpgradeCost(
                player.defenseUpgradeLevel
            ).ToString();

        hpLevelText.text =
            "Lv. " + player.hpUpgradeLevel;

        hpValueText.text =
            player.maxHp.ToString("F1");

        hpNextValueText.text =
            (player.maxHp + 10f).ToString("F1");

        hpCostText.text =
            player.GetUpgradeCost(
                player.hpUpgradeLevel
            ).ToString();
    }
}