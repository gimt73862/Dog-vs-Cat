using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTeamManager : MonoBehaviour
{
    public GameObject characterSelectPanel;

    public Image slot1Image;
    public Image slot2Image;
    public Image slot3Image;

    public TMP_Text slot1Name;
    public TMP_Text slot2Name;
    public TMP_Text slot3Name;

    public int selectedSlot = 0;

    public void SelectSlot(int slotNumber)
    {
        selectedSlot = slotNumber;

        if (characterSelectPanel != null)
        {
            characterSelectPanel.SetActive(true);
        }
    }

    public void SelectCharacter(
        Sprite characterSprite,
        string characterName
    )
    {
        if (selectedSlot == 1)
        {
            slot1Image.sprite = characterSprite;
            slot1Image.gameObject.SetActive(true);
            slot1Name.text = characterName;
        }
        else if (selectedSlot == 2)
        {
            slot2Image.sprite = characterSprite;
            slot2Image.gameObject.SetActive(true);
            slot2Name.text = characterName;
        }
        else if (selectedSlot == 3)
        {
            slot3Image.sprite = characterSprite;
            slot3Image.gameObject.SetActive(true);
            slot3Name.text = characterName;
        }

        if (characterSelectPanel != null)
        {
            characterSelectPanel.SetActive(false);
        }
    }

    public Sprite nureongiSprite;
    public Sprite huindongiSprite;

    public void SelectNureongi()
    {
        SelectCharacter(
            nureongiSprite,
            "¥©∑∑¿Ã"
        );
    }

    public void SelectHuindongi()
    {
        SelectCharacter(
            huindongiSprite,
            "»Úµ’¿Ã"
        );
    }
}