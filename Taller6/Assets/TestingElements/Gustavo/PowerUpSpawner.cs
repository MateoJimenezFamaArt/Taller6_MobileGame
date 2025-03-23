using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
      [SerializeField] private  GameObject spawnPrefab;
      [SerializeField] private  int poolSize = 20;
      [SerializeField] private  int maxObjectsOnGrid = 1; // Max objects active at once

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private List<Transform> spawnPoints;
    private SingletonBeatManager beatManager;

    void Start()
    {
        beatManager = FindObjectOfType<SingletonBeatManager>();
        if (beatManager == null)
        {
            Debug.LogError("ObjectSpawner: No BeatManager found!");
            return;
        }

        GridManager gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("ObjectSpawner: No GridManager found!");
            return;
        }

        spawnPoints = gridManager.GetSpawnPoints();
        InitializePool();

        // Subscribe to the beat event
        SingletonBeatManager.Instance.OnBeat += SpawnObjectOnBeat;
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SingletonBeatManager.Instance.OnBeat -= SpawnObjectOnBeat;
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(spawnPrefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    GameObject GetPooledObject()
    {
        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(spawnPrefab);
            return newObj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
        activeObjects.Remove(obj);
    }

    void SpawnObjectOnBeat()
    {
        if (spawnPoints.Count == 0 || activeObjects.Count >= maxObjectsOnGrid) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject obj = GetPooledObject();
        obj.transform.position = spawnPoint.position;
        obj.name = "SpawnedObject_" + activeObjects.Count;

        activeObjects.Add(obj);

        // Auto return object after some time
        //StartCoroutine(ReturnAfterTime(obj, beatManager.GetBeatInterval() * 8));
    }

    IEnumerator ReturnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool(obj);
    }
}
