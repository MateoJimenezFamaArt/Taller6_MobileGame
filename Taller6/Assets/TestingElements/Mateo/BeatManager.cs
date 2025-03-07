using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float BPM = 120f;

    

    private float beatInterval;
    private float nextBeatTime;
    private int beatCount = 0;
    private static bool isOnBeat = false;

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
            nextBeatTime = Time.time + beatInterval;
            isOnBeat = true;
        }
        isOnBeat = false;
    }

    public float GetBeatInterval() => beatInterval;
    public int GetBeatCount() => beatCount;
    public static bool IsOnBeat()
    {
        return isOnBeat;
    }
}