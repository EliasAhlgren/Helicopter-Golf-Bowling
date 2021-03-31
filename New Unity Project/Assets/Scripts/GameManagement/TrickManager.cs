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

        private void Start()
        {
            _gameManager = transform.root.GetComponentInChildren<GameManager>();
        }

        private void OnGUI()
        {
            GUI.Box (new Rect (0,0,100,50), rotationDone.ToString());
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
            rotationDone = Mathf.Abs(_lastRotation - transform.rotation.eulerAngles.y );
            _lastRotation = transform.rotation.eulerAngles.x;
        }
    }
}