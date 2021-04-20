using System;
using MLAPI;
using MLAPI.NetworkedVar;
using UnityEngine;

namespace GameManagement
{
    public class ScoreHolder : NetworkedBehaviour
    {
        private NetworkedVar<float> p0Score = new NetworkedVar<float>();
        private NetworkedVar<float> p1Score = new NetworkedVar<float>();
        private NetworkedVar<float> p2Score = new NetworkedVar<float>();
        private NetworkedVar<float> p3Score = new NetworkedVar<float>();


        private int _currentPlayer;

        private void Start()
        {
            p0Score.OnValueChanged += LogNewValue;
            p1Score.OnValueChanged += LogNewValue;
            p2Score.OnValueChanged += LogNewValue;
            p3Score.OnValueChanged += LogNewValue;
        }

        void LogNewValue(float value, float value1)
        {
            Debug.Log(value + " " + value1);
        }
        
        
    }
}