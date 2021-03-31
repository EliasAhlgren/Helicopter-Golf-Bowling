using System;
using System.Collections;
using System.Collections.Generic;
using GameManagement;
using MLAPI;
using MLAPI.NetworkedVar;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentPlayer;
    
    public float[] playerScores;
    public TextMeshProUGUI[] scoresUguis;
    
    public float scoreMultiplier;
    public int par;

    public GameObject playerTestObject;

    private bool isOfflineGame;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplier = par;
        isOfflineGame = gameObject.GetComponent<MultiplayerManager>().isOfflineGame;
    }

    public void GetScoresFromServer()
    {
        float[] newScores;
        GameObject.FindWithTag("ScoreHolder").GetComponent<ScoreHolder>().GetCurrentScores(out newScores);
        SetScores(newScores);
    }
    
    public void SetScores(float[] newScores)
    {
        for (int i = 0; i < newScores.Length; i++)
        {
            playerScores[i] = newScores[i];
        }
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
