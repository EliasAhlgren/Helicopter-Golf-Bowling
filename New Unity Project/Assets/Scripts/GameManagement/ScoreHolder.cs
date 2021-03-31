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
        
        public void GetCurrentScores(out float[] scores)
        {
            scores = new float[4];
            scores[0] = p0Score.Value;
            scores[1] = p1Score.Value;
            scores[2] = p2Score.Value;
            scores[3] = p3Score.Value;
            return;
        }

        public void SetCurrentScores(float newScore)
        {
            switch (_currentPlayer)
            {
                case 0:
                    p0Score.Value = newScore;
                    break;
                case 1:
                    p1Score.Value = newScore;
                    break;
                case 2:
                    p2Score.Value = newScore;
                    break;
                case 3:
                    p3Score.Value = newScore;
                    break;
            }

            _currentPlayer++;
        }
        
    }
}