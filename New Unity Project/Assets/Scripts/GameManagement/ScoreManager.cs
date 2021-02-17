using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentPlayer;
    
    public float[] playerScores;

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
        
    }
}
