using System;
using UnityEngine;

namespace GameManagement
{
    public class TrickManager : MonoBehaviour
    {
        private float _startRotationY;

        public float rotationDone;

        private float _lastRotation;
        
        private GameManager _gameManager;

        public float mFloat;
        
        private void Start()
        {
            _gameManager = transform.root.GetComponentInChildren<GameManager>();
        }

        private void OnGUI()
        {
            GUI.Box (new Rect (mFloat,0,100,50), Math.Round(rotationDone).ToString());
            GUI.Box (new Rect (300,0,100,50), transform.rotation.eulerAngles.y.ToString());
        }

        private void OnCollisionEnter(Collision other)
        {
            rotationDone = 0;
        }

        private void OnCollisionExit(Collision other)
        {
            rotationDone = 0;
            _startRotationY = transform.rotation.eulerAngles.y;
        }

        private void FixedUpdate()
        {
            rotationDone += (_lastRotation - transform.rotation.eulerAngles.y );
            _lastRotation = transform.rotation.eulerAngles.y;
            if (rotationDone == _startRotationY + 360)
            {
                 ScoreManager _scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
                 _scoreManager.playerScores[_scoreManager.currentPlayer] += 5;
                 rotationDone = 0;
            }
            
            if (rotationDone > 0)
            {
                if (rotationDone >= 360)
                {
                   
                }
            }
        }
    }
}