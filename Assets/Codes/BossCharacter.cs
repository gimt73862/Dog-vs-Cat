using UnityEngine;
using UnityEngine.UI;

public class BossCharacter : MonoBehaviour
{
    public BossCharacterData characterData;

    public float attack;
    public float defense;
    public float maxHp;
    public float hp;

    public GameObject hpBar;

    public float ultimateGauge = 0f;

    public bool isDead = false;

    public Image hpFillImage;

    public void SetupCharacter(
        BossCharacterData data,
        Player player
    )
    {
        characterData = data;

        attack =
            data.baseAttack *
            player.GetLevelMultiplier();

        defense =
            data.baseDefense *
            player.GetLevelMultiplier();

        maxHp =
            data.baseHp *
            player.GetLevelMultiplier();

        attack +=
            player.attack -
            10f;

        defense +=
            player.defense -
            10f;

        maxHp +=
            player.maxHp -
            100f;

        attack *=
            player.weaponAttackMultiplier;

        hp = maxHp;

        ultimateGauge = 0f;

        isDead = false;

        UpdateHpBar();

        if (hpBar != null)
        {
            hpBar.SetActive(true);
        }
    }

    public void SetupEnemyCharacter(
        BossCharacterData data,
        int enemyLevel,
        float attackBonus,
        float defenseBonus,
        float hpBonus
    )
    {
        characterData = data;

        float levelMultiplier =
            1f + (enemyLevel - 1) * 0.01f;

        attack =
            data.baseAttack *
            levelMultiplier +
            attackBonus;

        defense =
            data.baseDefense *
            levelMultiplier +
            defenseBonus;

        maxHp =
            data.baseHp *
            levelMultiplier +
            hpBonus;

        hp = maxHp;

        ultimateGauge = 0f;

        isDead = false;

        UpdateHpBar();

        if (hpBar != null)
        {
            hpBar.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        float finalDamage =
            damage *
            (1f - defense * 0.001f);

        if (finalDamage < 0f)
        {
            finalDamage = 0f;
        }

        hp -= finalDamage;

        if (hp <= 0f)
        {
            hp = 0f;

            Die();
        }

        UpdateHpBar();
    }

    public void UpdateHpBar()
    {
        if (hpFillImage == null)
        {
            return;
        }

        if (maxHp <= 0f)
        {
            hpFillImage.fillAmount = 0f;

            return;
        }

        hpFillImage.fillAmount =
            hp / maxHp;
    }

    public void Die()
    {
        isDead = true;
    }
}