using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public ObstaclePattern[] obstaclePatterns;
    public Transform cactusSpawnPoint;
    public Transform birdSpawnPoint;

    public float baseMinInterval = 1.2f;
    public float baseMaxInterval = 2.5f;

    private float spawnTimer;

    private ObstaclePattern lastUsedPattern;


    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameRunning)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                StartCoroutine(SpawnPattern());
                spawnTimer = GetAdjustedSpawnInterval();
            }
        }
    }

    float GetAdjustedSpawnInterval()
    {
        float speedFactor = GameManager.Instance.gameSpeed / GameManager.Instance.maxGameSpeed;
        float interval = Mathf.Lerp(baseMaxInterval, baseMinInterval, speedFactor);
        return Random.Range(interval, interval + 0.5f);
    }

    IEnumerator SpawnPattern()
    {
        if (obstaclePatterns.Length == 0)
            yield break;

        ObstaclePattern pattern = null;

        if (obstaclePatterns.Length == 1)
        {
            pattern = obstaclePatterns[0]; // only one option
        }
        else
        {
            int attempt = 0;
            do
            {
                pattern = obstaclePatterns[Random.Range(0, obstaclePatterns.Length)];
                attempt++;
            }
            while (pattern == lastUsedPattern && attempt < 10); // avoid infinite loop
        }

        lastUsedPattern = pattern;


        foreach (GameObject prefab in pattern.sequence)
        {
            Vector3 spawnPos = prefab.name.ToLower().Contains("bird") ? birdSpawnPoint.position : cactusSpawnPoint.position;
            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(pattern.spacing / GameManager.Instance.gameSpeed);
        }
    }
}
