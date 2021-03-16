

using System;
using System.Collections;
using GameManagement;
using TMPro;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] private Vector3 _ogPos;
    private Quaternion _ogRot;
    
    [SerializeField] private ScoreManager _scoreManager;

    [SerializeField] private GameManager _gameManager;
    
    public float targetScore = 10f;

    public bool hasFallen;
    
    private void Start()
    {
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
        MultiplayerManager mp = GameObject.FindWithTag("ScoreManager").GetComponent<MultiplayerManager>();
        if (other.gameObject.CompareTag("Helicopter") && mp.isHost)
        {
            _gameManager = mp.helicopters[mp.currentPlayer].GetComponentInChildren<GameManager>();
            _gameManager.startEvent.AddListener(ResetTransform);
            _scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
            
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
