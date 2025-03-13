using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LaserSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    public int poolSize = 20;
    public int maxObjectsOnGrid = 1; // Max objects active at once

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private List<Transform> borderspawnPoints;
    private BeatManager beatManager;
    [SerializeField] private ParticleSystem laserParticles;
    private ParticleSystem laserParticlesInstance;
    void Start()
    {
        beatManager = FindObjectOfType<BeatManager>();
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

        borderspawnPoints = gridManager.GetBorderSpawnPoints();
        InitializePool();

        // Subscribe to the beat event
        BeatManager.OnBeat += SpawnObjectOnBeat;
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        BeatManager.OnBeat -= SpawnObjectOnBeat;
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
    GameObject obj;

    if (objectPool.Count > 0)
    {
        obj = objectPool.Dequeue();
    }
    else
    {
        obj = Instantiate(spawnPrefab);
    }

    obj.SetActive(true);
    obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, 64); // Fijar el Z en 64

    return obj;
}


    public void ReturnToLaserPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
        activeObjects.Remove(obj);
    }

    void SpawnObjectOnBeat()
    {
        if (borderspawnPoints.Count == 0 || activeObjects.Count >= maxObjectsOnGrid) return;

        //List<Transform> borderSpawnPoints = new List<Transform>();
        float mostCommonX1, mostCommonX2;
        float mostCommonZ1, mostCommonZ2;

        Dictionary<float, int> conteoX = new Dictionary<float, int>();
        Dictionary<float, int> conteoZ = new Dictionary<float, int>();

        foreach (Transform point in borderspawnPoints)
        {
            float x = point.position.x;
            float z = point.position.z;

            if (conteoX.ContainsKey(x)) conteoX[x]++;
            else conteoX[x] = 1;

            if (conteoZ.ContainsKey(z)) conteoZ[z]++;
            else conteoZ[z] = 1;
        }

        mostCommonX1 = conteoX.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).FirstOrDefault();
        mostCommonX2 = conteoX.OrderByDescending(kv => kv.Value).Skip(1).Select(kv => kv.Key).FirstOrDefault();

        mostCommonZ1 = conteoZ.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).FirstOrDefault();
        mostCommonZ2 = conteoZ.OrderByDescending(kv => kv.Value).Skip(1).Select(kv => kv.Key).FirstOrDefault();

        Transform spawnPoint = borderspawnPoints[Random.Range(0, borderspawnPoints.Count)];

        Vector3 spawnPosition = spawnPoint.transform.position;

        GameObject obj = GetPooledObject();
        obj.transform.position = new Vector3(spawnPoint.position.x, 1, spawnPoint.position.z);
        obj.name = "SpawnedLaser_" + activeObjects.Count;
        activeObjects.Add(obj);

        LineRenderer lineRenderer = obj.GetComponent<LineRenderer>();
        //BoxCollider boxCollider = obj.GetComponent<BoxCollider>();

        //cambiooooo
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = true;
        }

        lineRenderer.SetPosition(0, spawnPosition);

        if (spawnPoint.position.x == mostCommonX1 || spawnPoint.position.x == mostCommonX2)
        {
            foreach (Transform point in borderspawnPoints)
            {
                float x = point.position.x;
                float z = point.position.z;

                if (z == spawnPoint.position.z && x != spawnPoint.position.x)
                {
                    Vector3 endPosition = point.position;
                    lineRenderer.SetPosition(1, endPosition);
                    obj.transform.Rotate(0, 90, 0);
                }
            }
        }
        else if (spawnPoint.position.z == mostCommonZ1 || spawnPoint.position.z == mostCommonZ2)
        {
            foreach (Transform point in borderspawnPoints)
            {
                float x = point.position.x;
                float z = point.position.z;

                if (x == spawnPoint.position.x && z != spawnPoint.position.z)
                {
                    Vector3 endPosition = point.position;
                    lineRenderer.SetPosition(1, endPosition);
                }
            }
        }

        laserParticlesInstance = Instantiate(laserParticles, obj.transform);

        StartCoroutine(ReturnAfterTime(obj, beatManager.GetBeatInterval() * 8));      
}


    IEnumerator ReturnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToLaserPool(obj);
    }
}