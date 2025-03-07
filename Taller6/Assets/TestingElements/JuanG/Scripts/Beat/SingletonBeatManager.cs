using System;
using UnityEngine;

public class SingletonBeatManager : MonoBehaviour
{
    public static SingletonBeatManager Instance { get; private set; }

    [SerializeField] AudioSource audioSource;
    [Tooltip("Beats per minute")]
    [SerializeField] private float bpm = 120f;
    private float beatInterval;
    private float nextBeatTime;
    private int beatCount = 0;
    private bool isOnBeat = false;

    public event Action OnBeat;
    public event Action OutBeat;

    private void Awake()
    {
        if ( Instance != null)
        {
            Debug.LogError("There's mora than one SingletonBeatManager" 
                + transform.position + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        beatInterval = 60f / bpm;
        nextBeatTime = Time.time + beatInterval;
        Debug.Log("Beat interval: " + beatInterval);
        Debug.Log("Next beat time: " + nextBeatTime);
    }

    void Update()
    {
        if (Time.time >= nextBeatTime - 0.2f && Time.time <= nextBeatTime + 0.2f)
        {
            if (!isOnBeat)
            {
                isOnBeat = true;
                OnBeat?.Invoke();
            } 
        }
        else
        {
            isOnBeat = false;
            OutBeat?.Invoke();
        }

        if (audioSource.isPlaying && Time.time >= nextBeatTime)
        {
            beatCount++;
            nextBeatTime = Time.time + beatInterval;
        }
    }
    public float GetBeatInterval() => beatInterval;
    public int GetBeatCount() => beatCount;
}
