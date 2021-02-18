using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentPlayer;
    
    public float[] playerScores;
    public TextMeshProUGUI[] scoresUguis;
    
    public float scoreMultiplier;
    public int par;
    
    
    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplier = par;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerScores.Length; i++)
        {
            scoresUguis[i].text = "Player " + (i + 1) + ": " + playerScores[i];
        }    
    }
}
