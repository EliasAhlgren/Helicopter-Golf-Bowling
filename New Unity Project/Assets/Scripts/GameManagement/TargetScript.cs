﻿

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private Vector3 _ogPos;
    private Quaternion _ogRot;
    
    private ScoreManager _scoreManager;

    private GameManager _gameManager;
    
    public float targetScore = 10f;

    public bool hasFallen;
    
    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _gameManager.startEvent.AddListener(ResetTransform);
        _ogPos = transform.position;
        _ogRot = transform.rotation;
        _scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
    }

    public void ResetTransform()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        var gb = Instantiate(gameObject, _ogPos, _ogRot);
        gb.GetComponent<Collider>().enabled = true;
        
        Destroy(gameObject);
    }

    public IEnumerator waitBeforeReset()
    {
        yield return new WaitForSeconds(0.1f);
        
        Debug.Log("Ending Round");
        
        
        _gameManager.HelicopterDestroyed(0.1f);
        
        foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
        {
            VARIABLE.GetComponent<TargetScript>().StopAllCoroutines();
        }
    }
    //
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Helicopter"))
        {
            if (!hasFallen && _gameManager.isReseting == false)
            {
                 Debug.Log("Adding score " + other.gameObject);
                 _scoreManager.playerScores[_scoreManager.currentPlayer] += targetScore * _scoreManager.scoreMultiplier;
                 hasFallen = true;
                 GameObject.Find("fuseFront").GetComponent<FuselageController>().helicopterHealth = 100f;
                 _gameManager.hasInvincibility = true;
                 _gameManager.StartCoroutine(_gameManager.Strike(2f));
                 //StartCoroutine(waitBeforeReset());
                 //targetScore = 0;
            }
           
        }
    }
}
