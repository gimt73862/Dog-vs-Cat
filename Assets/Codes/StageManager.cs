using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int stage = 1;
    public int stageKillCount = 0;
    public int requiredKills = 10;

    public GameObject normalUI;
    public GameObject bossButton;
    public GameObject bossBattlePanel;
    public GameObject blackScreen;

    public BossTeamUIManager bossTeamUIManager;

    public UnityEngine.UI.Image ally1Image;
    public UnityEngine.UI.Image ally2Image;
    public UnityEngine.UI.Image ally3Image;

    public UnityEngine.UI.Image enemy1Image;
    public UnityEngine.UI.Image enemy2Image;
    public UnityEngine.UI.Image enemy3Image;

    public Sprite cheeseSprite;
    public Sprite godeungeoSprite;

    public BossCharacterData nureongiData;
    public BossCharacterData huindongiData;

    public BossCharacter ally1Character;
    public BossCharacter ally2Character;
    public BossCharacter ally3Character;

    public BossCharacter enemy1Character;
    public BossCharacter enemy2Character;
    public BossCharacter enemy3Character;

    public BossCharacterData cheeseData;
    public BossCharacterData godeungeoData;

    public GameObject skill1Button;
    public GameObject skill2Button;
    public GameObject ultimateButton;

    public BossBattleManager bossBattleManager;

    public float blackScreenTime = 1f;

    void Start()
    {
        UpdateBossButton();

        if (normalUI != null)
        {
            normalUI.SetActive(true);
        }

        if (bossBattlePanel != null)
        {
            bossBattlePanel.SetActive(false);
        }

        if (blackScreen != null)
        {
            blackScreen.SetActive(false);
        }
    }

    public bool CanEnterBoss()
    {
        return stageKillCount >= requiredKills;
    }

    public void AddStageKill()
    {
        stageKillCount++;

        UpdateBossButton();
    }

    public void UpdateBossButton()
    {
        if (bossButton == null)
        {
            return;
        }

        bossButton.SetActive(
            CanEnterBoss()
        );
    }

    public void EnterBossBattle()
    {
        if (!CanEnterBoss())
        {
            return;
        }

        StartCoroutine(
            EnterBossBattleRoutine()
        );
    }

    public IEnumerator EnterBossBattleRoutine()
    {
        if (normalUI != null)
        {
            normalUI.SetActive(false);
        }

        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(
            blackScreenTime
        );

        if (bossBattlePanel != null)
        {
            bossBattlePanel.SetActive(true);
        }

        if (bossBattleManager != null)
        {
            bossBattleManager.ResetSelectedActions();

            bossBattleManager.skill1Button.SetActive(false);
            bossBattleManager.skill2Button.SetActive(false);
            bossBattleManager.ultimateButton.SetActive(false);

            bossBattleManager.selectedCharacter = null;

            bossBattleManager.HideSkillDescription();
        }

        SetupBossTeam();
        SetupBossEnemies();
        SetupBossEnemiesStats();

        Time.timeScale = 0f;

        if (blackScreen != null)
        {
            blackScreen.SetActive(false);
        }
    }

    public void HideNormalUI()
    {
        if (normalUI != null)
        {
            normalUI.SetActive(false);
        }
    }

    public void ShowNormalUI()
    {
        if (normalUI != null)
        {
            normalUI.SetActive(true);
        }

        UpdateBossButton();
    }

    public void SetupBossTeam()
    {
        if (bossTeamUIManager == null)
        {
            return;
        }

        SetAlly(
            ally1Image,
            ally1Character,
            bossTeamUIManager.slot1CharacterName
        );

        SetAlly(
            ally2Image,
            ally2Character,
            bossTeamUIManager.slot2CharacterName
        );

        SetAlly(
            ally3Image,
            ally3Character,
            bossTeamUIManager.slot3CharacterName
        );
    }

    public void SetAlly(
        UnityEngine.UI.Image image,
        BossCharacter bossCharacter,
        string characterName
    )
    {
        if (
            image == null ||
            bossCharacter == null
        )
        {
            return;
        }

        Player player =
            FindFirstObjectByType<Player>();

        if (characterName == "(R) ¥©∑∑¿Ã")
        {
            image.sprite =
                bossTeamUIManager.nureongiSprite;

            image.gameObject.SetActive(true);

            bossCharacter.SetupCharacter(
                nureongiData,
                player
            );
        }
        else if (characterName == "(R) »Úµ’¿Ã")
        {
            image.sprite =
                bossTeamUIManager.huindongiSprite;

            image.gameObject.SetActive(true);

            bossCharacter.SetupCharacter(
                huindongiData,
                player
            );
        }
        else
        {
            image.sprite = null;
            image.gameObject.SetActive(false);

            bossCharacter.characterData = null;

            if (bossCharacter.hpBar != null)
            {
                bossCharacter.hpBar.SetActive(false);
            }
        }
    }

    public void SetupBossEnemies()
    {
        if (enemy1Image != null)
        {
            enemy1Image.sprite =
                cheeseSprite;

            enemy1Image.gameObject.SetActive(
                true
            );
        }

        if (enemy2Image != null)
        {
            enemy2Image.sprite =
                godeungeoSprite;

            enemy2Image.gameObject.SetActive(
                true
            );
        }

        if (enemy3Image != null)
        {
            enemy3Image.sprite = null;

            enemy3Image.gameObject.SetActive(false);

            if (
                enemy3Character != null &&
                enemy3Character.hpBar != null
            )
            {
                enemy3Character.hpBar.SetActive(false);
            }
        }
    }

    public void SetupBossEnemiesStats()
    {
        if (stage == 1)
        {
            enemy1Character.SetupEnemyCharacter(
                cheeseData,
                1,
                0f,
                0f,
                0f
            );

            enemy2Character.SetupEnemyCharacter(
                godeungeoData,
                1,
                0f,
                0f,
                0f
            );

            if (enemy3Character != null)
            {
                enemy3Character.characterData =
                    null;
            }
        }
        else if (stage == 2)
        {
            enemy1Character.SetupEnemyCharacter(
                cheeseData,
                3,
                2f,
                1f,
                20f
            );

            enemy2Character.SetupEnemyCharacter(
                godeungeoData,
                3,
                2f,
                1f,
                20f
            );

            if (enemy3Character != null)
            {
                enemy3Character.characterData =
                    null;
            }
        }
        else if (stage == 3)
        {
            enemy1Character.SetupEnemyCharacter(
                cheeseData,
                5,
                4f,
                2f,
                40f
            );

            enemy2Character.SetupEnemyCharacter(
                godeungeoData,
                5,
                4f,
                2f,
                40f
            );

            if (enemy3Character != null)
            {
                enemy3Character.characterData =
                    null;
            }
        }
    }
}