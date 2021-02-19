

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private Vector3 _ogPos;
    private Quaternion _ogRot;
    
    private ScoreManager _scoreManager;

    public float targetScore = 10f;

    public bool hasFallen;
    
    private void Start()
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().startEvent.AddListener(ResetTransform);
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
        yield return new WaitForSeconds(2f);
        
        Debug.Log("Ending Round");
        
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().hasInvincibility = true;
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().HelicopterDestroyed(0.1f);
        
        foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
        {
            VARIABLE.GetComponent<TargetScript>().StopAllCoroutines();
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Helicopter"))
        {
            if (!hasFallen)
            {
                 Debug.Log("Adding score");
                 _scoreManager.playerScores[_scoreManager.currentPlayer] += targetScore * _scoreManager.scoreMultiplier;
                 hasFallen = true;
                 StartCoroutine(waitBeforeReset());
                 //targetScore = 0;
            }
           
        }
    }
}
