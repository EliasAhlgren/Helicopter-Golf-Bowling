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
        FindObjectOfType<ScoreHolder>().GetCurrentScores(out newScores);
        SetScores(newScores);
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
        for (int i = 0; i < newScores.Length; i++)
        {
            playerScores[i] = newScores[i];
        }
    }

    private void Update()
    {
        scoreMultiplierGUI.text = "Score Multiplier = " + scoreMultiplier;
    }

    // Update is called once per frame
    public void UpdateStatStrings()
    {
        for (int i = 0; i < scoresUguis.Length; i++)
        {
            scoresUguis[i].text = "Player " + (i + 1).ToString() + " " + playerScores[i];
        }  
    }
}
