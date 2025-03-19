using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    public int poolSize = 20;
    public int maxObjectsOnGrid = 10; // Max objects active at once

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private List<Transform> spawnPoints;

    [SerializeField] private ParticleSystem spawnParticles;
    private ParticleSystem spawnParticlesInstance;
    public int emissionOnBeats = 3;
    private int beatCounter = 0;
    void Start()
    {
        GridManager gridManager = FindFirstObjectByType<GridManager>();
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
            obj.SetActive(false);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(spawnPrefab);
            newObj.SetActive(false);
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
        beatCounter++;
        Vector3 direction = Vector3.up;
        spawnParticlesInstance = Instantiate(spawnParticles, spawnPoint.transform.position,Quaternion.LookRotation(direction), obj.transform);
        ParticleSystem.EmissionModule emission = spawnParticlesInstance.emission;
        if(beatCounter == 0) emission.rateOverTime = 10;
        else { emission.rateOverTime = 15; }
        if (beatCounter == emissionOnBeats) { emission.rateOverTime = 20; }
        else if(beatCounter > emissionOnBeats){ emission.rateOverTime = 0;  obj.SetActive(true); }


        activeObjects.Add(obj);

        // Auto return object after some time
        StartCoroutine(ReturnAfterTime(obj, SingletonBeatManager.Instance.GetBeatInterval() * 8));
    }

    IEnumerator ReturnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool(obj);
    }
}
