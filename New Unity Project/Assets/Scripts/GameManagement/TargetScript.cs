

using System;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private ScoreManager _scoreManager;

    public float targetScore = 10f;
    
    private void Start()
    {
        _scoreManager = Camera.main.GetComponent<ScoreManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Helicopter"))
        {
            _scoreManager.playerScores[_scoreManager.currentPlayer] += targetScore * _scoreManager.scoreMultiplier;
        }
    }
}
