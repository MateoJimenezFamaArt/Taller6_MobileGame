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

        SkinnedMeshRenderer meshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
        BeatExploder exploder = obj.GetComponent<BeatExploder>();
        RotateObject rotator = obj.GetComponent<RotateObject>();

        meshRenderer.enabled = false;
        exploder.enabled = false;
        rotator.enabled = false;

        ParticleSystem particles = Instantiate(spawnParticles, spawnPoint.position, Quaternion.identity, obj.transform);
        ParticleSystem.EmissionModule emission = particles.emission;

        emission.rateOverTime = 10;

        StartCoroutine(HandleObjectBeats(obj, particles, meshRenderer, exploder, rotator));

        activeObjects.Add(obj);

        // Auto return object after some time
        StartCoroutine(ReturnAfterTime(obj, SingletonBeatManager.Instance.GetBeatInterval() * 8));
    }

    IEnumerator HandleObjectBeats(GameObject obj, ParticleSystem particles, SkinnedMeshRenderer meshRenderer, BeatExploder exploder, RotateObject rotator)
    {
        int localBeatCounter = 0;

        while (localBeatCounter < emissionOnBeats)
        {
            yield return new WaitForSeconds(SingletonBeatManager.Instance.GetBeatInterval());
            localBeatCounter++;
            ParticleSystem.EmissionModule emission = particles.emission;
            emission.rateOverTime = 10 + (localBeatCounter * 5);
            if (localBeatCounter == emissionOnBeats)
            {
                particles.Stop();
                particles.Clear();
                Destroy(particles.gameObject);

                meshRenderer.enabled = true;
                exploder.enabled = true;
                rotator.enabled = true;
            }
        }
    }

    IEnumerator ReturnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool(obj);
    }
}
