using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public string grade;
    public GameObject characterPrefab;
    public Sprite characterImage;

    public string traitName;
    public string traitDescription;
}

public class CharacterUI : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject detailPanel;

    public Image detailImage;
    public TMP_Text detailNameText;
    public TMP_Text detailText;

    public CharacterData[] characters;

    public int selectedCharacterIndex = -1;

    public Player currentPlayer;

    public Transform spawnPoint;

    public GameObject blackScreen;

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            currentPlayer =
                playerObject.GetComponent<Player>();
        }

        characterPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    public void OpenCharacterPanel()
    {
        characterPanel.SetActive(true);
        detailPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void CloseCharacterPanel()
    {
        detailPanel.SetActive(false);
        characterPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length)
        {
            return;
        }

        selectedCharacterIndex = index;

        CharacterData data =
            characters[index];

        detailImage.sprite =
            data.characterImage;

        detailNameText.text =
            data.grade + " " +
            data.characterName;

        detailText.text =
            "Æ¯¼º: " +
            data.traitName +
            "\n" +
            data.traitDescription;

        detailPanel.SetActive(true);
    }

    public void UseSelectedCharacter()
    {
        if (
            selectedCharacterIndex < 0 ||
            selectedCharacterIndex >= characters.Length
        )
        {
            return;
        }

        ChangeCharacter(
            selectedCharacterIndex
        );

        detailPanel.SetActive(false);
        characterPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ChangeCharacter(int index)
    {
        CharacterData data =
            characters[index];

        Vector3 spawnPosition =
            Vector3.zero;

        Quaternion spawnRotation =
            Quaternion.identity;

        int level = 1;
        int killCount = 0;

        float attack = 10f;
        float defense = 10f;
        float maxHp = 100f;
        float hp = 100f;

        int attackUpgradeLevel = 1;
        int defenseUpgradeLevel = 1;
        int hpUpgradeLevel = 1;

        int gold = 0;
        int equipmentTickets = 0;
        int characterTickets = 0;

        Sprite equippedWeaponSprite = null;
        Vector3 equippedWeaponScale =
            new Vector3(0.4f, 0.4f, 1f);

        float weaponAttackMultiplier = 1f;

        if (currentPlayer != null)
        {
            spawnPosition =
                currentPlayer.transform.position;

            spawnRotation =
                currentPlayer.transform.rotation;

            level =
                currentPlayer.level;

            killCount =
                currentPlayer.killCount;

            attack =
                currentPlayer.attack;

            defense =
                currentPlayer.defense;

            maxHp =
                currentPlayer.maxHp;

            hp =
                currentPlayer.hp;

            attackUpgradeLevel =
                currentPlayer.attackUpgradeLevel;

            defenseUpgradeLevel =
                currentPlayer.defenseUpgradeLevel;

            hpUpgradeLevel =
                currentPlayer.hpUpgradeLevel;

            gold =
                currentPlayer.gold;

            equipmentTickets =
                currentPlayer.equipmentTickets;

            characterTickets =
                currentPlayer.characterTickets;

            weaponAttackMultiplier =
                currentPlayer.weaponAttackMultiplier;

            Weapon oldWeapon =
                currentPlayer.GetComponentInChildren<Weapon>();

            if (
                oldWeapon != null &&
                oldWeapon.spriteRenderer != null
            )
            {
                equippedWeaponSprite =
                    oldWeapon.spriteRenderer.sprite;

                equippedWeaponScale =
                    oldWeapon.transform.localScale;
            }

            Destroy(
                currentPlayer.gameObject
            );
        }
        else if (spawnPoint != null)
        {
            spawnPosition =
                spawnPoint.position;

            spawnRotation =
                spawnPoint.rotation;
        }

        GameObject newCharacter =
            Instantiate(
                data.characterPrefab,
                spawnPosition,
                spawnRotation
            );

        Player newPlayer =
            newCharacter.GetComponent<Player>();

        newPlayer.characterName =
            data.characterName;

        newPlayer.level =
            level;

        newPlayer.killCount =
            killCount;

        newPlayer.attack =
            attack;

        newPlayer.defense =
            defense;

        newPlayer.maxHp =
            maxHp;

        newPlayer.hp =
            hp;

        newPlayer.attackUpgradeLevel =
            attackUpgradeLevel;

        newPlayer.defenseUpgradeLevel =
            defenseUpgradeLevel;

        newPlayer.hpUpgradeLevel =
            hpUpgradeLevel;

        newPlayer.gold =
            gold;

        newPlayer.equipmentTickets =
            equipmentTickets;

        newPlayer.characterTickets =
            characterTickets;

        newPlayer.weaponAttackMultiplier =
            weaponAttackMultiplier;

        newPlayer.blackScreen =
            blackScreen;

        Weapon newWeapon =
            newCharacter.GetComponentInChildren<Weapon>();

        if (
            newWeapon != null &&
            equippedWeaponSprite != null
        )
        {
            newWeapon.ChangeWeapon(
                equippedWeaponSprite,
                weaponAttackMultiplier,
                equippedWeaponScale
            );
        }

        currentPlayer =
            newPlayer;
    }

    public void CloseDetailPanel()
    {
        detailPanel.SetActive(false);
    }
}