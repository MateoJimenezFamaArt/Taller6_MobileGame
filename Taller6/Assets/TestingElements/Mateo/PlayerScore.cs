using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class PlayerScore : MonoBehaviour
{

    public float CurrentScore;

    public TMP_Text scoreText;

    void Start()
    {
        CurrentScore = 0;
    }
    
    void Update()
    { 
        scoreText.text = CurrentScore.ToString();

    }

    public void AddScore(float score)
    {
        CurrentScore+=score;
        
    }
    
}
