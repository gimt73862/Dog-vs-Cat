using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EquipmentData
{
    public string equipmentName;
    public Sprite equipmentSprite;
    public string description;
    public float attackMultiplier = 1f;
    public Vector3 weaponScale = new Vector3(0.4f, 0.4f, 1f);
}

public class EquipmentUI : MonoBehaviour
{
    public Player player;
    public Weapon weapon;

    public GameObject equipmentPanel;
    public GameObject detailPanel;

    public Image detailImage;
    public TMP_Text detailNameText;
    public TMP_Text detailText;

    public EquipmentData[] equipments;

    public int selectedEquipmentIndex = -1;

    void Start()
    {
        player =
            GameObject
            .FindGameObjectWithTag("Player")
            .GetComponent<Player>();

        weapon =
            player.GetComponentInChildren<Weapon>();

        if (equipments.Length > 0)
        {
            EquipEquipment(0);
        }

        equipmentPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    public void OpenEquipmentPanel()
    {
        equipmentPanel.SetActive(true);
        detailPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void CloseEquipmentPanel()
    {
        detailPanel.SetActive(false);
        equipmentPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void SelectEquipment(int index)
    {
        if (index < 0 || index >= equipments.Length)
        {
            return;
        }

        selectedEquipmentIndex = index;

        EquipmentData data =
            equipments[index];

        detailImage.sprite =
            data.equipmentSprite;

        detailNameText.text =
            data.equipmentName;

        detailText.text =
            data.description;

        detailPanel.SetActive(true);
    }

    public void EquipSelectedEquipment()
    {
        if (
            selectedEquipmentIndex < 0 ||
            selectedEquipmentIndex >= equipments.Length
        )
        {
            return;
        }

        EquipEquipment(
            selectedEquipmentIndex
        );

        detailPanel.SetActive(false);
    }

    public void EquipEquipment(int index)
    {
        EquipmentData data =
            equipments[index];

        weapon.ChangeWeapon(
            data.equipmentSprite,
            data.attackMultiplier,
            data.weaponScale
        );
    }

    public void CloseDetailPanel()
    {
        detailPanel.SetActive(false);
    }
}