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
    
    [SyncedVar]
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

    
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerScores.Length; i++)
        {
            scoresUguis[i].text = "Player " + (i + 1) + ": " + playerScores[i];
        }    
    }
}
