using UnityEngine;
using UnityEngine.UI;

public class BossBattleManager : MonoBehaviour
{
    public BossCharacter ally1;
    public BossCharacter ally2;
    public BossCharacter ally3;

    public BossCharacter enemy1;
    public BossCharacter enemy2;
    public BossCharacter enemy3;

    public bool isPlayerTurn = true;

    public int selectedActionCount = 0;

    public Image skill1Image;
    public Image skill2Image;
    public Image ultimateImage;

    public GameObject skill1Button;
    public GameObject skill2Button;
    public GameObject ultimateButton;

    public Image action1Image;
    public Image action2Image;
    public Image action3Image;

    public Sprite nureongiSkill1Sprite;
    public Sprite nureongiSkill2Sprite;
    public Sprite nureongiUltimateSprite;

    public Sprite huindongiSkill1Sprite;
    public Sprite huindongiSkill2Sprite;
    public Sprite huindongiUltimateSprite;

    public GameObject skillDescriptionPanel;

    public Image descriptionSkillImage;

    public TMPro.TextMeshProUGUI descriptionSkillName;
    public TMPro.TextMeshProUGUI descriptionSkillText;

    public float longPressTime = 0.4f;

    public float skillPressStartTime;

    public Coroutine skillLongPressCoroutine;

    public int pressedSkillNumber;

    public BossCharacter selectedCharacter;

    public void StartBattle()
    {
        isPlayerTurn = true;

        ResetSelectedActions();

        skill1Image.enabled = false;
        skill2Image.enabled = false;
        ultimateImage.enabled = false;
    }

    public void SelectAlly1()
    {
        if (
            ally1 == null ||
            ally1.characterData == null ||
            ally1.isDead
        )
        {
            return;
        }

        selectedCharacter = ally1;

        UpdateSkillImages();
    }

    public void SelectAlly2()
    {
        if (
            ally2 == null ||
            ally2.characterData == null ||
            ally2.isDead
        )
        {
            return;
        }

        selectedCharacter = ally2;

        UpdateSkillImages();
    }

    public void SelectAlly3()
    {
        if (
            ally3 == null ||
            ally3.characterData == null ||
            ally3.isDead
        )
        {
            return;
        }

        selectedCharacter = ally3;

        UpdateSkillImages();
    }

    public void UpdateSkillImages()
    {
        if (
            selectedCharacter == null ||
            selectedCharacter.characterData == null
        )
        {
            return;
        }

        string characterName =
            selectedCharacter.characterData.characterName;

        if (characterName == "누렁이")
        {
            skill1Image.sprite =
                nureongiSkill1Sprite;

            skill2Image.sprite =
                nureongiSkill2Sprite;

            ultimateImage.sprite =
                nureongiUltimateSprite;
        }
        else if (characterName == "흰둥이")
        {
            skill1Image.sprite =
                huindongiSkill1Sprite;

            skill2Image.sprite =
                huindongiSkill2Sprite;

            ultimateImage.sprite =
                huindongiUltimateSprite;
        }

        skill1Button.SetActive(true);
        skill2Button.SetActive(true);
        ultimateButton.SetActive(true);
    }

    public void SelectSkill1()
    {
        if (selectedCharacter == null)
        {
            return;
        }

        AddSelectedAction(
            skill1Image.sprite
        );
    }

    public void SelectSkill2()
    {
        if (selectedCharacter == null)
        {
            return;
        }

        AddSelectedAction(
            skill2Image.sprite
        );
    }

    public void SelectUltimate()
    {
        if (selectedCharacter == null)
        {
            return;
        }

        AddSelectedAction(
            ultimateImage.sprite
        );
    }

    public void AddSelectedAction(
    Sprite skillSprite
)
    {
        if (!isPlayerTurn)
        {
            return;
        }

        if (skillSprite == null)
        {
            return;
        }

        if (selectedActionCount >= 3)
        {
            return;
        }

        if (
            action1Image.sprite == skillSprite ||
            action2Image.sprite == skillSprite ||
            action3Image.sprite == skillSprite
        )
        {
            return;
        }

        if (selectedActionCount == 0)
        {
            action1Image.sprite =
                skillSprite;

            action1Image.enabled =
                true;
        }
        else if (selectedActionCount == 1)
        {
            action2Image.sprite =
                skillSprite;

            action2Image.enabled =
                true;
        }
        else if (selectedActionCount == 2)
        {
            action3Image.sprite =
                skillSprite;

            action3Image.enabled =
                true;
        }

        selectedActionCount++;

        if (selectedActionCount == 3)
        {
            ExecuteSelectedActions();
        }
    }

    public void ResetSelectedActions()
    {
        selectedActionCount = 0;

        if (action1Image != null)
        {
            action1Image.sprite = null;
            action1Image.enabled = false;
        }

        if (action2Image != null)
        {
            action2Image.sprite = null;
            action2Image.enabled = false;
        }

        if (action3Image != null)
        {
            action3Image.sprite = null;
            action3Image.enabled = false;
        }
    }

    public void ExecuteSelectedActions()
    {
    }

    public bool IsAllyAlive()
    {
        if (
            ally1 != null &&
            ally1.characterData != null &&
            !ally1.isDead
        )
        {
            return true;
        }

        if (
            ally2 != null &&
            ally2.characterData != null &&
            !ally2.isDead
        )
        {
            return true;
        }

        if (
            ally3 != null &&
            ally3.characterData != null &&
            !ally3.isDead
        )
        {
            return true;
        }

        return false;
    }

    public bool IsEnemyAlive()
    {
        if (
            enemy1 != null &&
            enemy1.characterData != null &&
            !enemy1.isDead
        )
        {
            return true;
        }

        if (
            enemy2 != null &&
            enemy2.characterData != null &&
            !enemy2.isDead
        )
        {
            return true;
        }

        if (
            enemy3 != null &&
            enemy3.characterData != null &&
            !enemy3.isDead
        )
        {
            return true;
        }

        return false;
    }

    public void ShowSkillDescription(int skillNumber)
    {
        if (
            selectedCharacter == null ||
            selectedCharacter.characterData == null
        )
        {
            return;
        }

        skillDescriptionPanel.SetActive(true);

        string characterName =
            selectedCharacter.characterData.characterName;

        if (characterName == "누렁이")
        {
            if (skillNumber == 1)
            {
                descriptionSkillImage.sprite =
                    nureongiSkill1Sprite;

                descriptionSkillName.text =
                    "야수의 송곳니";

                descriptionSkillText.text =
                    "적 1명에게 공격력의 120% 피해를 준다.";
            }
            else if (skillNumber == 2)
            {
                descriptionSkillImage.sprite =
                    nureongiSkill2Sprite;

                descriptionSkillName.text =
                    "맹수의 돌격";

                descriptionSkillText.text =
                    "적 1명에게 공격력의 100% 피해를 주고, 다음 공격 피해가 20% 증가한다.";
            }
            else if (skillNumber == 3)
            {
                descriptionSkillImage.sprite =
                    nureongiUltimateSprite;

                descriptionSkillName.text =
                    "야성 폭주";

                descriptionSkillText.text =
                    "적 1명에게 공격력의 250% 피해를 준다.";
            }
        }
        else if (characterName == "흰둥이")
        {
            if (skillNumber == 1)
            {
                descriptionSkillImage.sprite =
                    huindongiSkill1Sprite;

                descriptionSkillName.text =
                    "수호의 일격";

                descriptionSkillText.text =
                    "적 1명에게 공격력의 80% 피해를 주고, 다음에 받는 피해를 20% 감소시킨다.";
            }
            else if (skillNumber == 2)
            {
                descriptionSkillImage.sprite =
                    huindongiSkill2Sprite;

                descriptionSkillName.text =
                    "보호 본능";

                descriptionSkillText.text =
                    "현재 체력 비율이 가장 낮은 아군의 다음 피해를 20% 감소시킨다.";
            }
            else if (skillNumber == 3)
            {
                descriptionSkillImage.sprite =
                    huindongiUltimateSprite;

                descriptionSkillName.text =
                    "불굴의 수호";

                descriptionSkillText.text =
                    "모든 생존 아군에게 각자 최대 체력의 20%만큼 보호막을 부여한다.";
            }
        }
    }

    public void HideSkillDescription()
    {
        skillDescriptionPanel.SetActive(false);
    }

    public void SkillPointerDown(int skillNumber)
    {
        pressedSkillNumber =
            skillNumber;

        skillPressStartTime =
            Time.unscaledTime;

        if (skillLongPressCoroutine != null)
        {
            StopCoroutine(
                skillLongPressCoroutine
            );
        }

        skillLongPressCoroutine =
            StartCoroutine(
                SkillLongPressRoutine(skillNumber)
            );
    }

    public System.Collections.IEnumerator SkillLongPressRoutine(
        int skillNumber
    )
    {
        yield return new WaitForSecondsRealtime(
            longPressTime
        );

        ShowSkillDescription(
            skillNumber
        );
    }

    public void SkillPointerUp(int skillNumber)
    {
        float pressTime =
            Time.unscaledTime -
            skillPressStartTime;

        if (skillLongPressCoroutine != null)
        {
            StopCoroutine(
                skillLongPressCoroutine
            );

            skillLongPressCoroutine =
                null;
        }

        HideSkillDescription();

        if (pressTime >= longPressTime)
        {
            return;
        }

        if (skillNumber == 1)
        {
            SelectSkill1();
        }
        else if (skillNumber == 2)
        {
            SelectSkill2();
        }
        else if (skillNumber == 3)
        {
            SelectUltimate();
        }
    }
}