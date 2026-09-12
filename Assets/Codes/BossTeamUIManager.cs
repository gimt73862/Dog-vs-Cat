using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTeamUIManager : MonoBehaviour
{
    public GameObject bossTeamPanel;
    public GameObject characterSelectPanel;
    public GameObject characterDetailPanel;

    public Image slot1Image;
    public Image slot2Image;
    public Image slot3Image;

    public TMP_Text slot1Name;
    public TMP_Text slot2Name;
    public TMP_Text slot3Name;

    public Sprite nureongiSprite;
    public Sprite huindongiSprite;

    public Image detailImage;
    public TMP_Text detailNameText;
    public TMP_Text detailText;

    public int selectedSlot = 0;
    public string selectedCharacterName;
    public Sprite selectedCharacterSprite;

    public string slot1CharacterName;
    public string slot2CharacterName;
    public string slot3CharacterName;

    void Start()
    {
        if (bossTeamPanel != null)
        {
            bossTeamPanel.SetActive(false);
        }

        if (characterSelectPanel != null)
        {
            characterSelectPanel.SetActive(false);
        }

        if (characterDetailPanel != null)
        {
            characterDetailPanel.SetActive(false);
        }

        ClearSlot(slot1Image, slot1Name);
        ClearSlot(slot2Image, slot2Name);
        ClearSlot(slot3Image, slot3Name);
    }

    public void OpenBossTeamPanel()
    {
        bossTeamPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseBossTeamPanel()
    {
        bossTeamPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSlot(int slotNumber)
    {
        selectedSlot = slotNumber;

        characterSelectPanel.SetActive(true);
    }

    public void CloseCharacterSelectPanel()
    {
        characterSelectPanel.SetActive(false);
    }

    public void OpenNureongiDetail()
    {
        selectedCharacterName = "(R) 누렁이";
        selectedCharacterSprite = nureongiSprite;

        detailImage.sprite = nureongiSprite;
        detailImage.gameObject.SetActive(true);

        detailNameText.text = "(R) 누렁이";

        detailText.text =
            "기본 스킬 1 : 야수의 송곳니\n" +
            "적 1명에게 공격력의 120%만큼 데미지를 준다.\n\n" +

            "기본 스킬 2 : 맹수의 돌격\n" +
            "적 1명에게 공격력의 100%만큼 데미지를 주고, 다음에 누렁이가 해당 적에게 주는 데미지가 20% 증가한다.\n\n" +

            "궁극기 : 야성 폭주\n" +
            "적 1명에게 공격력의 250%만큼 데미지를 준다.";

        characterDetailPanel.SetActive(true);
    }

    public void OpenHuindongiDetail()
    {
        selectedCharacterName = "(R) 흰둥이";
        selectedCharacterSprite = huindongiSprite;

        detailImage.sprite = huindongiSprite;
        detailImage.gameObject.SetActive(true);

        detailNameText.text = "(R) 흰둥이";

        detailText.text =
            "기본 스킬 1 : 수호의 일격\n" +
            "적 1명에게 공격력의 80%만큼 데미지를 주고, 흰둥이가 다음에 받는 데미지를 20% 감소시킨다.\n\n" +

            "기본 스킬 2 : 보호 본능\n" +
            "흰둥이를 포함해 현재 체력 비율이 가장 낮은 아군 1명이 다음에 받는 데미지를 20% 감소시킨다.\n\n" +

            "궁극기 : 불굴의 수호\n" +
            "모든 생존 아군에게 각 캐릭터 최대 체력의 20%만큼 보호막을 부여한다.";

        characterDetailPanel.SetActive(true);
    }

    public void CloseCharacterDetailPanel()
    {
        characterDetailPanel.SetActive(false);
    }

    public void UseCharacter()
    {
        if (
            selectedSlot == 0 ||
            selectedCharacterSprite == null
        )
        {
            return;
        }

        if (
            selectedSlot != 1 &&
            slot1CharacterName == selectedCharacterName
        )
        {
            return;
        }

        if (
            selectedSlot != 2 &&
            slot2CharacterName == selectedCharacterName
        )
        {
            return;
        }

        if (
            selectedSlot != 3 &&
            slot3CharacterName == selectedCharacterName
        )
        {
            return;
        }

        if (selectedSlot == 1)
        {
            SetSlot(
                slot1Image,
                slot1Name
            );

            slot1CharacterName =
                selectedCharacterName;
        }
        else if (selectedSlot == 2)
        {
            SetSlot(
                slot2Image,
                slot2Name
            );

            slot2CharacterName =
                selectedCharacterName;
        }
        else if (selectedSlot == 3)
        {
            SetSlot(
                slot3Image,
                slot3Name
            );

            slot3CharacterName =
                selectedCharacterName;
        }

        characterDetailPanel.SetActive(false);
    }

    public void SetSlot(
        Image slotImage,
        TMP_Text slotName
    )
    {
        slotImage.sprite =
            selectedCharacterSprite;

        slotImage.gameObject.SetActive(true);

        slotName.text =
            selectedCharacterName;
    }

    public void ClearSlot(
        Image slotImage,
        TMP_Text slotName
    )
    {
        if (slotImage != null)
        {
            slotImage.sprite = null;
            slotImage.gameObject.SetActive(false);
        }

        if (slotName != null)
        {
            slotName.text = "비어있음";
        }
    }

    public void ClearSelectedSlot()
    {
        if (selectedSlot == 1)
        {
            ClearSlot(
                slot1Image,
                slot1Name
            );

            slot1CharacterName = "";
        }
        else if (selectedSlot == 2)
        {
            ClearSlot(
                slot2Image,
                slot2Name
            );

            slot2CharacterName = "";
        }
        else if (selectedSlot == 3)
        {
            ClearSlot(
                slot3Image,
                slot3Name
            );

            slot3CharacterName = "";
        }

        characterSelectPanel.SetActive(false);
    }
}