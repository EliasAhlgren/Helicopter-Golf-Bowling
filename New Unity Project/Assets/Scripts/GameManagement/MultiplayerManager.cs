using System;
using TMPro;
using UnityEngine;

namespace UnityTemplateProjects.GameManagement
{
    public class MultiplayerManager : MonoBehaviour
    {

        private GameManager gameManager;

        private ScoreManager scoreManager;
        
        public int playerCount;

        public int currentPlayer;

        private void Start()
        {
            gameManager = gameObject.GetComponent<GameManager>();
            scoreManager = gameObject.GetComponent<ScoreManager>();
        }

        public void NextPlayerTurn()
        {
            currentPlayer++;
            scoreManager.currentPlayer++;
            for (int i = 0; i < scoreManager.scoresUguis.Length; i++)
            {
                if (i == currentPlayer)
                {
                    scoreManager.scoresUguis[i].color = Color.red;
                }
                else
                {
                    scoreManager.scoresUguis[i].color = Color.white;
                }
            }
        }
        
    }
}