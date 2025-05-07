using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public int maxSize = 10; // Default cap
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, int> poolSizes;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolSizes = new Dictionary<string, int>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            poolSizes.Add(pool.tag, pool.size); // track initial size
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPooler] Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = null;
        Queue<GameObject> poolQueue = poolDictionary[tag];

        // Try to find an inactive object in the queue
        int initialCount = poolQueue.Count;
        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = poolQueue.Dequeue();
            if (!obj.activeInHierarchy)
            {
                objectToSpawn = obj;
                break;
            }
            poolQueue.Enqueue(obj);
        }

        // If none available, attempt to expand pool
        if (objectToSpawn == null)
        {
            Pool poolData = pools.Find(p => p.tag == tag);
            if (poolData != null)
            {
                int currentSize = poolSizes[tag];
                if (currentSize < poolData.maxSize)
                {
                    objectToSpawn = Instantiate(poolData.prefab);
                    objectToSpawn.SetActive(false);
                    poolQueue.Enqueue(objectToSpawn);
                    poolSizes[tag]++;
                    Debug.Log($"[ObjectPooler] Auto-expanded pool '{tag}' (new size: {poolSizes[tag]})");
                }
                else
                {
                    Debug.LogWarning($"[ObjectPooler] Pool '{tag}' reached max size ({poolData.maxSize}). Spawn skipped.");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"[ObjectPooler] Could not find pool data for tag '{tag}'");
                return null;
            }
        }

        // Activate and return
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        return objectToSpawn;
    }
}
