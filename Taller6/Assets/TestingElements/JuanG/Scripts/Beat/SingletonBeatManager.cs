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
        beatInterval = 60f / bpm; // Calcula el intervalo entre beats en segundos
        nextBeatTime = Time.time + beatInterval; // Establece el tiempo del primer beat
    }

    void Update()
    {
        if (audioSource.isPlaying && Time.time >= nextBeatTime)
        {
            beatCount++;
            OnBeat?.Invoke();
            nextBeatTime = Time.time + beatInterval;
        }
        OutBeat?.Invoke();
    }
    public float GetBeatInterval() => beatInterval;
    public int GetBeatCount() => beatCount;
}
