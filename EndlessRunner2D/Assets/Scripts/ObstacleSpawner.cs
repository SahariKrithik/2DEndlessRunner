using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public Transform cactusSpawnPoint;
    public Transform birdSpawnPoint;

    public float baseMinInterval = 1.2f;
    public float baseMaxInterval = 2.5f;
    public float minSpacing = 6f;

    private float spawnTimer;
    private GameObject lastSpawnedObstacle;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameRunning)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f && CanSpawnNewObstacle())
            {
                SpawnObstacle();
                spawnTimer = GetAdjustedSpawnInterval();
            }
        }
    }

    bool CanSpawnNewObstacle()
    {
        if (lastSpawnedObstacle == null) return true;

        float distance = Mathf.Abs(lastSpawnedObstacle.transform.position.x - cactusSpawnPoint.position.x);
        return distance >= minSpacing;
    }

    float GetAdjustedSpawnInterval()
    {
        float speedFactor = GameManager.Instance.gameSpeed / GameManager.Instance.maxGameSpeed;
        float interval = Mathf.Lerp(baseMaxInterval, baseMinInterval, speedFactor);
        return Random.Range(interval, interval + 0.5f);
    }

    void SpawnObstacle()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject prefab = obstaclePrefabs[index];

        Vector3 spawnPos = prefab.name.ToLower().Contains("bird")
            ? birdSpawnPoint.position
            : cactusSpawnPoint.position;

        GameObject obstacle = Instantiate(prefab, spawnPos, Quaternion.identity);
        lastSpawnedObstacle = obstacle;
    }
}
