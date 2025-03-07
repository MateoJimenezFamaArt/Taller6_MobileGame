using UnityEngine;
using UnityEngine.Pool;

public class BeatSpawner : MonoBehaviour
{
    [Tooltip("Prefab to display")]
    [SerializeField] private Beat beatPrefab;

    private IObjectPool<Beat> beatPool;
    [SerializeField] private int defaultPoolSize = 3;
    [SerializeField] private int maxPoolSize = 5;
    [SerializeField] private bool collectionCheck = true;


    private void Awake()
    {
        beatPool = new ObjectPool<Beat>(
            CreateBeat, OnGetFromPool, OnReleaseToPool,
            OnDestroyPooledObject, collectionCheck, defaultPoolSize, maxPoolSize);
    }

    private void OnEnable()
    {
        BeatManager.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        BeatManager.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        Beat beat = beatPool.Get();
        beat.transform.position = transform.position;
        beat.Deactive();
    }

    private Beat CreateBeat()
    {
        Beat beatInstance = Instantiate(beatPrefab, transform.parent);
        beatInstance.BeatPool = beatPool;
        return beatInstance;
    }
    private void OnReleaseToPool(Beat beat)
    {
        beat.gameObject.SetActive(false);
    }
    private void OnGetFromPool(Beat beat)
    {
        beat.gameObject.SetActive(true);
    }
    private void OnDestroyPooledObject(Beat beat)
    {
        Destroy(beat.gameObject);
    }
}
