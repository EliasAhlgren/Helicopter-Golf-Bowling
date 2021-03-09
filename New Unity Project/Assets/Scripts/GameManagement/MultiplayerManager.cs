using System;
using Cinemachine;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;

namespace GameManagement
{

    
    public class MultiplayerManager : MonoBehaviour
    {
        public bool isOfflineGame;
        
        private GameManager _gameManager;

        private ScoreManager _scoreManager;
        
        public int playerCount;

        public int currentPlayer;

        public bool isAtStartup = true;

        public bool isHost;

        public GameObject playerPrefab;
        
        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        public void StartHost()
        {
            
            
            NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
            NetworkingManager.Singleton.StartHost(Vector3.zero,Quaternion.identity,true, SpawnManager.GetPrefabHashFromIndex(0));
            
            //GameObject teuvo = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            //teuvo.GetComponent<NetworkedObject>().SpawnWithOwnership(NetworkingManager.Singleton.ServerClientId);
            
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            _gameManager.isOfflineGame = isOfflineGame;
            _gameManager.transform.root.gameObject.GetComponentInChildren<Camera>().enabled = true;
            _gameManager.transform.root.name = "Player " + NetworkingManager.Singleton.ConnectedClients.Count;
            
            isHost = true;
        }

        private void ClientConnected(ulong obj)
        {
            Debug.Log("Client connected");

            GameObject jeff = GameObject.Find("Player(Clone)");
            jeff.name = "Player " + NetworkingManager.Singleton.ConnectedClients.Count;
            SetPlayerPositions(jeff, NetworkingManager.Singleton.ConnectedClientsList[0].PlayerObject.transform.position, NetworkingManager.Singleton.ConnectedClientsList[0].PlayerObject.transform.rotation);
        }

        [ClientRPC]
        public void SetPlayerPositions(GameObject obj, Vector3 pos, Quaternion rot)
        {
            Debug.Log("Hello");
            if (!isHost)
            {
                obj.GetComponentInChildren<Camera>().enabled = true;
            }
        }
        
        public void StartClient()
        {
            NetworkingManager.Singleton.StartClient();
            
        }
        
        
        private void Start()
        {
            
            
            //Destroy(GameObject.FindGameObjectsWithTag("ScoreManager")[1]);
            if (isOfflineGame)
            {
                 _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
                 _gameManager.isOfflineGame = isOfflineGame;
            }
            _scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
            
           
            _scoreManager.scoresUguis[0].color = Color.red;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && isAtStartup)
            {
                StartHost();
                isAtStartup = false;
                return;
            }
            if (Input.GetKeyDown(KeyCode.C) && isAtStartup)
            {
                StartClient();
                isAtStartup = false;
            }

            if (isHost)
            {
            
            }

        }

        public void NextPlayerTurn()
        {
            
            
            if (currentPlayer + 1 <= playerCount)
            {
                currentPlayer++;
                _scoreManager.currentPlayer++;
                for (int i = 0; i < _scoreManager.scoresUguis.Length; i++)
                {
                    if (i == currentPlayer)
                    {
                        _scoreManager.scoresUguis[i].color = Color.red;
                    }
                    else
                    {
                        _scoreManager.scoresUguis[i].color = Color.white;
                    }
                }
            }
            else
            {
               // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }
        
    }
}