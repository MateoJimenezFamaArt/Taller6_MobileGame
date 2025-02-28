using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    public int poolSize = 20;
    public int maxObjectsOnGrid = 1; // Max objects active at once

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private List<Transform> spawnPoints;
    private BeatManager beatManager;

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

        spawnPoints = gridManager.GetSpawnPoints();
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
    if (spawnPoints.Count == 0 || activeObjects.Count >= maxObjectsOnGrid) return;

    // Filtrar solo los puntos en los bordes (fila 0, 7 y columna 0, 7)
    List<Transform> borderSpawnPoints = new List<Transform>();

    foreach (Transform point in spawnPoints)
    {
        Vector3 pos = point.position;

        // Convertir coordenadas del mundo a índices de la grid
        int x = Mathf.RoundToInt((pos.x - transform.position.x) / 2) + 4;
        int y = Mathf.RoundToInt((pos.z - transform.position.z) / 2) + 4;

        if (x == 0 || x == 7 || y == 0 || y == 7) // Solo bordes
        {
            borderSpawnPoints.Add(point);
        }
    }

    if (borderSpawnPoints.Count == 0) return; // Si no hay puntos válidos, salir

    // Seleccionar un punto aleatorio de los bordes
    Transform spawnPoint = borderSpawnPoints[Random.Range(0, borderSpawnPoints.Count)];
    GameObject obj = GetPooledObject();
    obj.transform.position = spawnPoint.position;

    // Decidir aleatoriamente si el láser será vertical u horizontal
    bool isHorizontal = Random.value > 0.5f;

    if (isHorizontal)
    {
        obj.transform.rotation = Quaternion.Euler(0, 90, 0); // Girar 90° en Y
        
    }
    else
    {
        obj.transform.rotation = Quaternion.identity; // Mantener vertical
       
    }

    obj.name = "SpawnedLaser_" + activeObjects.Count;
    activeObjects.Add(obj);
    StartCoroutine(ReturnAfterTime(obj, beatManager.GetBeatInterval() * 8));
}




    IEnumerator ReturnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToLaserPool(obj);
    }
}