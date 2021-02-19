using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityTemplateProjects.GameManagement
{

    
    public class MultiplayerManager : MonoBehaviour
    {
        private GameManager gameManager;

        private ScoreManager scoreManager;
        
        public int playerCount;

        public int currentPlayer;

        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //Destroy(GameObject.FindGameObjectsWithTag("ScoreManager")[1]);
            gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
            scoreManager.scoresUguis[0].color = Color.red;
        }

        public void NextPlayerTurn()
        {
            if (currentPlayer + 1 <= playerCount)
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
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }
        
    }
}