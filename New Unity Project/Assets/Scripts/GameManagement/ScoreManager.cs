using System;
using System.Collections;
using System.Collections.Generic;
using GameManagement;
using MLAPI;
using MLAPI.NetworkedVar;
using MLAPI.Spawning;
using TMPro;
using UnityEngine;
using NetworkPlayer = HelicopterController.NetworkPlayer;

public class ScoreManager : MonoBehaviour
{
    public int currentPlayer;
    
    public float[] playerScores;
    public TextMeshProUGUI[] scoresUguis;
    public TextMeshProUGUI scoreMultiplierGUI;
    
    public float scoreMultiplier;
    public int par;

    public GameObject playerTestObject;

    private bool isOfflineGame;

    public string emptySlotName;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplier = par + 1;
        isOfflineGame = gameObject.GetComponent<MultiplayerManager>().isOfflineGame;
        UpdateStatStrings();
    }

    public void GetScoresFromServer()
    {
        Debug.Log("Got Scores");
        float[] newScores;
        UpdateStatStrings();
    }

    public void FindScores()
    {
        for (int i = 0; i < SpawnManager.SpawnedObjectsList.Count; i++)
        {
            playerScores[i] = SpawnManager.SpawnedObjectsList[i].GetComponent<NetworkPlayer>().myScoreFloat;
            scoresUguis[i].text = GetComponent<MultiplayerManager>()._playerNames[i] + ": " +
                                  SpawnManager.SpawnedObjectsList[i].GetComponent<NetworkPlayer>().myScoreFloat;
            Debug.Log("Got score for player " + i + " Score is " + SpawnManager.SpawnedObjectsList[i].GetComponent<NetworkPlayer>().myScoreFloat);
        }
        UpdateStatStrings();
    }
    
    public void RemoveEmptySlots()
    {
        foreach (var VARIABLE in scoresUguis)
        {
            if (VARIABLE.text == emptySlotName)
            {
                VARIABLE.enabled = false;
            }
        }
    }
    
    private void SetScores(float[] newScores)
    {
        if (!GetComponent<MultiplayerManager>().isOfflineGame)
        {
            for (int i = 0; i < newScores.Length; i++)
            {
                playerScores[i] = newScores[i];
            }
        }
        
    }

    private void Update()
    {
        scoreMultiplierGUI.text = "Score Multiplier = " + scoreMultiplier;
    }

    // Update is called once per frame
    public void UpdateStatStrings()
    {
        if (GetComponent<MultiplayerManager>().isOfflineGame)
        {
            for (int i = 0; i < scoresUguis.Length; i++)
            {
                scoresUguis[i].text = "Player " + (i + 1).ToString() + " " + playerScores[i];
            } 
        }
        else
        {
            for (int i = 0; i < GetComponent<MultiplayerManager>()._playerNames.Count; i++)
            {
                scoresUguis[i].text = GetComponent<MultiplayerManager>()._playerNames[i] + " " + playerScores[i];
            } 
        }
         
    }
}
