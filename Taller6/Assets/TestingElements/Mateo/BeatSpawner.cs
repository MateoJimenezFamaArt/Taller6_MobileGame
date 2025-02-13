using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject spawnPrefab;
    public GameObject tilePrefab;
    public float BPM = 120f;
    public int gridSize = 8;
    public float spacing = 2f;
    public int poolSize = 20;

    private Transform[] spawnPoints;
    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private float beatInterval;
    private float nextBeatTime;
    private int beatCount = 0;

    void Start()
    {
        if (audioSource == null || spawnPrefab == null || tilePrefab == null)
        {
            Debug.LogError("Assign an AudioSource, a Prefab to spawn, and a Tile Prefab.");
            return;
        }

        GenerateSpawnPoints();
        InitializePool();
        beatInterval = 60f / BPM;
        nextBeatTime = Time.time + beatInterval;
    }

    void Update()
    {
        if (audioSource.isPlaying && Time.time >= nextBeatTime)
        {
            beatCount++;
            SpawnObjectsOnBeat();
            nextBeatTime = Time.time + beatInterval;
        }
    }

    void GenerateSpawnPoints()
    {
        spawnPoints = new Transform[gridSize * gridSize];
        Vector3 startPosition = transform.position - new Vector3(gridSize / 2 * spacing, 0, gridSize / 2 * spacing);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject point = new GameObject("SpawnPoint_" + (x * gridSize + y));
                point.transform.position = startPosition + new Vector3(x * spacing, 0, y * spacing);
                point.transform.parent = transform;
                spawnPoints[x * gridSize + y] = point.transform;

                GameObject tile = Instantiate(tilePrefab, point.transform.position, Quaternion.identity);
                tile.transform.parent = transform;
            }
        }
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

    void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.localScale = new Vector3(1, 1, 1);
        objectPool.Enqueue(obj);
    }

    void SpawnObjectsOnBeat()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject obj = GetPooledObject();
        obj.transform.position = spawnPoint.position;
        obj.name = "BeatObject_Beat_" + beatCount;

        // Attach or reset ItemGrow behavior
        ItemGrow itemGrow = obj.GetComponent<ItemGrow>();
        if (itemGrow == null)
        {
            itemGrow = obj.AddComponent<ItemGrow>();
        }
        itemGrow.Initialize(ReturnToPool, beatInterval);
    }
}
