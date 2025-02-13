using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float BPM = 120f;

    public static event Action OnBeat; // Event to notify scripts when a beat occurs

    private float beatInterval;
    private float nextBeatTime;
    private int beatCount = 0;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("BeatManager: No AudioSource assigned!");
            return;
        }

        beatInterval = 60f / BPM;
        nextBeatTime = Time.time + beatInterval;
    }

    void Update()
    {
        if (audioSource.isPlaying && Time.time >= nextBeatTime)
        {
            beatCount++;
            OnBeat?.Invoke(); // Notify all listeners that a beat happened
            nextBeatTime = Time.time + beatInterval;
        }
    }

    public float GetBeatInterval() => beatInterval;
    public int GetBeatCount() => beatCount;
}