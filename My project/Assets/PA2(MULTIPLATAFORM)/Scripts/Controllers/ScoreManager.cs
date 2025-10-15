using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private int _actualScore;

    private void Awake()
    {
        _actualScore =  0;
    }

    private void UpdateScore()
    {
        scoreText.text = _actualScore.ToString();
    }

    public int GetScore()
    {
        return _actualScore;
    }

    private void SubmitScore()
    {
        UpdateScore();
    }

    public void AddScore(int score)
    {
        if(score < 0)
        { 
            score = 0;
        }
        
        _actualScore += score;
        UpdateScore();
    }
    public void SubtractScore(int score)
    {
        if(score < 0)
        { 
            score = 0;
        }
        
        _actualScore -= score;
        UpdateScore();
    }
}

