using System;
using System.Collections.Generic;
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

        public List<GameObject> helicopters;

        public int serverCapacity;
        
        public Camera lookAtCamera;
        
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
            //_gameManager.transform.root.gameObject.GetComponentInChildren<Camera>().enabled = true;
            helicopters.Add(_gameManager.transform.root.gameObject); 
            helicopters[0].name = "Player " + NetworkingManager.Singleton.ConnectedClients.Count;
            
            isHost = true;
        }

        private void ClientConnected(ulong obj)
        {
            Debug.Log("Client connected");

            GameObject jeff = GameObject.Find("Player(Clone)");
            jeff.name = "Player " + NetworkingManager.Singleton.ConnectedClients.Count;
            helicopters.Add(jeff);
            List<ulong> ids = new  List<ulong>();
            ids.Add(jeff.GetComponent<NetworkedObject>().OwnerClientId);
            if (isHost)
            {
                //SpawnManager.GetLocalPlayerObject().GetComponent<NetworkedBehaviour>().InvokeClientRpc("SetCamera",ids, jeff);
            }
        }

        
        
        public void StartClient()
        {
            NetworkingManager.Singleton.StartClient();
            
        }
        
        
        private void Start()
        {

            lookAtCamera = GameObject.Find("CineCamera").GetComponent<Camera>();
            lookAtCamera.enabled = true;
            
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
            if (isHost && NetworkingManager.Singleton.ConnectedClientsList.Count > 1)
            {
                List<ulong> ids = new List<ulong>(0);
                ids.Add(helicopters[currentPlayer].GetComponent<NetworkedBehaviour>().OwnerClientId);

                helicopters[0].GetComponent<NetworkPlayer>().InvokeClientRpcOnEveryone(helicopters[0].GetComponent<NetworkPlayer>().SetCamera);
                
                helicopters[currentPlayer].GetComponent<NetworkedBehaviour>().InvokeClientRpc("SetCamera" ,ids);
                
            }
            
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