using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    public float spawnTimer;
    public Player player;

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

        if (player.isDead)
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnMonster();
            spawnTimer = 0f;
        }
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

    public void SpawnMonster()
    {
        int randomMonsterIndex =
            Random.Range(0, monsterPrefabs.Length);

        int randomSpawnIndex =
            Random.Range(0, spawnPoints.Length);

        Instantiate(
            monsterPrefabs[randomMonsterIndex],
            spawnPoints[randomSpawnIndex].position,
            Quaternion.identity
        );
    }
}