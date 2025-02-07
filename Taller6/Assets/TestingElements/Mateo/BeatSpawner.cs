using UnityEngine;
using System.Collections.Generic;

public class BeatSpawner : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource with an audio clip
    public GameObject spawnPrefab; // The object to spawn on beats
    public GameObject tilePrefab; // The visual representation of each tile
    public float BPM = 120f; // Set BPM (Beats Per Minute) manually
    public int gridSize = 8; // Grid size for predefined positions (8x8 = 64 points)
    public float spacing = 2f; // Spacing between points in the grid
    public int poolSize = 20; // Initial size of object pool

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
        beatInterval = 60f / BPM; // Convert BPM to seconds per beat
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

                // Create a visual representation of the tile
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
            // If the pool is empty, expand it
            GameObject newObj = Instantiate(spawnPrefab);
            return newObj;
        }
    }

    void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    void SpawnObjectsOnBeat()
    {
        if (beatCount % 1 == 0) SpawnInstance(1);
        if (beatCount % 2 == 0) SpawnInstance(2);
        if (beatCount % 3 == 0) SpawnInstance(3);
        if (beatCount % 4 == 0) SpawnInstance(4);
    }

    void SpawnInstance(int interval)
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject obj = GetPooledObject();
        obj.transform.position = spawnPoint.position;
        obj.name = "BeatObject_" + interval + "_Beat";

        // Return to pool after a set duration (e.g., 2 seconds)
        StartCoroutine(DeactivateAfterTime(obj, 2f));
    }

    System.Collections.IEnumerator DeactivateAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool(obj);
    }
}
