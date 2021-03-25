using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Prototyping;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using NetworkPlayer = HelicopterController.NetworkPlayer;
/*
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
*/
using Color = UnityEngine.Color;


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

        [FormerlySerializedAs("helicopters")] public List<GameObject> networkPlayers;

        public int serverCapacity;
        
        public Camera lookAtCamera;

        public Vector3 spawnPosition; 
        
        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        public void StartHost()
        {
            spawnPosition = Vector3.zero;
            
            NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
            NetworkingManager.Singleton.StartHost(spawnPosition,Quaternion.identity,true, SpawnManager.GetPrefabHashFromIndex(0));
            
            //GameObject teuvo = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            //teuvo.GetComponent<NetworkedObject>().SpawnWithOwnership(NetworkingManager.Singleton.ServerClientId);
            
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            _gameManager.isOfflineGame = isOfflineGame;
            //_gameManager.transform.root.gameObject.GetComponentInChildren<Camera>().enabled = true;
            networkPlayers.Add(_gameManager.transform.root.gameObject); 
            networkPlayers[0].name = "Player " + NetworkingManager.Singleton.ConnectedClients.Count;

            isHost = true;
            
            List<ulong> ids = new List<ulong> {SpawnManager.GetLocalPlayerObject().OwnerClientId};
            SpawnManager.GetLocalPlayerObject()
                .GetComponent<NetworkedBehaviour>()
                .InvokeClientRpc("SetCamera", ids, spawnPosition);
            //SpawnManager.GetLocalPlayerObject().GetComponent<NetworkedBehaviour>().InvokeClientRpc("SpawnHelicopter", ids, spawnPosition, NetworkingManager.Singleton.LocalClientId);
            
            //GameObject.Find("CineCamera").GetComponent<NetworkedTransform>().enabled = true;
            //spawnPosition += Vector3.left * 10;
        }

        void Disconnected(ulong obj)
        {
            Debug.Log("Disconnected");
            
        }
        
        private void ClientConnected(ulong obj)
        {
                if (NetworkingManager.Singleton.ConnectedClientsList.Count <= playerCount)
                {
                    NetworkingManager.Singleton.OnClientDisconnectCallback += Disconnected;
                    spawnPosition += Vector3.forward * 10;
                    UnetTransport unetTransport = gameObject.GetComponent<UnetTransport>();
                    Debug.Log("Connected to: " + unetTransport.ConnectAddress + " rtt " + unetTransport.GetCurrentRtt(unetTransport.ServerClientId));

                    Debug.Log("Client " + obj + " connected");

                    //GameObject jeff = GameObject.Find("Player(Clone)");
                    GameObject jeff = SpawnManager.SpawnedObjects[obj].gameObject;
                    jeff.name = "Player " + NetworkingManager.Singleton.ConnectedClients.Count;
                    networkPlayers.Add(jeff);
                    
                    if (isHost)
                    {
                        Debug.Log("Host: Client spawned");
                        List<ulong> ids = new List<ulong> {jeff.GetComponent<NetworkedObject>().OwnerClientId};
                        //jeff.GetComponentInChildren<Camera>().enabled = false;
                        /*SpawnManager.GetLocalPlayerObject()
                            .GetComponent<NetworkedBehaviour>()
                            .InvokeClientRpc("SetCamera", ids, spawnPosition);
                        */
                        //SpawnManager.GetLocalPlayerObject().GetComponent<NetworkedBehaviour>().InvokeClientRpc("SpawnHelicopter", ids, spawnPosition, obj);
                        SpawnManager.GetLocalPlayerObject()
                            .GetComponent<NetworkedBehaviour>()
                            .InvokeClientRpc("Testi", ids);
                        Debug.Log(networkPlayers.Count + " pelaajaa" + serverCapacity + " tarvitaan");
                        if (networkPlayers.Count == serverCapacity)
                        {
                            Debug.Log("Nyt mennää");
                            StartGame();
                        }
                    }
                }
                else
                {
                    NetworkingManager.Singleton.DisconnectClient(obj);
                }
                
            
            
            
        }

        
        async void StartGame()
        {
            await Task.Delay(TimeSpan.FromSeconds(2.5f));
            Debug.Log(NetworkingManager.Singleton.LocalClientId + " le number");
            SpawnManager.GetLocalPlayerObject().GetComponent<NetworkPlayer>().InvokeClientRpcOnEveryone("SetCurrentPlayerCamera", SpawnManager.GetLocalPlayerObject(), 0);
        }
        
        public void StartClient()
        {
            NetworkingManager.Singleton.StartClient();
            
        }

        public void StopClient()
        {
            Debug.Log("jees");

            if (isHost)
            {
                NetworkingManager.Singleton.StopHost();
                networkPlayers = new List<GameObject>();
                currentPlayer = 0;
                spawnPosition = Vector3.zero;
                isHost = false;
                lookAtCamera = null;
            }
            else
            {
                NetworkingManager.Singleton.StopClient();
                //gameObject.GetComponent<UnetTransport>().Shutdown();   
            }
        }
        
        private void Start()
        {
            /*
            String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            String range = "Class Data!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
            service.Spreadsheets.Values.Get(spreadsheetId, range);
            */

            if (!isOfflineGame)
            {
                lookAtCamera = GameObject.Find("CineCamera").GetComponent<Camera>();
                lookAtCamera.enabled = true;
            }
            
            
            //Destroy(GameObject.FindGameObjectsWithTag("ScoreManager")[1]);
           
            _scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
            _scoreManager.scoresUguis[0].color = Color.red;

            if (!isOfflineGame)
            {
                if (PlayerPrefs.GetInt("ShouldStartClient") == 1)
                {
                    try
                    {
                        gameObject.GetComponent<UnetTransport>().ConnectAddress = PlayerPrefs.GetString("HostIp");
                        StartClient();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        SceneManager.LoadScene(0);
                        throw;
                    }
                    
                }
                else
                {
                    serverCapacity = PlayerPrefs.GetInt("Capacity");
                    StartHost();
                }
            }
            else //offline game setup
            {
                //Instantiate()
            }
            
        }

        private void Update()
        {
        }

        public void NextPlayerTurn()
        {

            if (!isOfflineGame && isHost)
            {
                networkPlayers[currentPlayer].transform.position = -Vector3.one * 69f;
                foreach (var VARIABLE in networkPlayers[currentPlayer].GetComponentsInChildren<Rigidbody>())
                {
                    VARIABLE.isKinematic = true;
                }
                
                
            }

            if (currentPlayer + 1 <= playerCount)
            {
                Debug.Log("Next player");
               
                    currentPlayer++;

                    for (int i = 0; i < networkPlayers.Count; i++)
                    {
                        Debug.Log("Index: "+i+ " player: " +networkPlayers[i]);
                    }
                    
                _scoreManager.currentPlayer++;
                networkPlayers[currentPlayer].transform.position = Vector3.zero;
                foreach (var VARIABLE in networkPlayers[currentPlayer].GetComponentsInChildren<Rigidbody>())
                {
                    VARIABLE.isKinematic = false;
                }
                SpawnManager.GetLocalPlayerObject().GetComponent<NetworkPlayer>().InvokeClientRpcOnEveryone("SetCurrentPlayerCamera", networkPlayers[currentPlayer].GetComponent<NetworkedObject>(), _scoreManager.playerScores[currentPlayer - 1]);

                List<ulong> currentTarget = new List<ulong>();
                currentTarget.Add(networkPlayers[currentPlayer].GetComponent<NetworkedObject>().OwnerClientId);
                
                
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
                Debug.Log("Last player");
                SpawnManager.GetLocalPlayerObject().GetComponent<NetworkPlayer>().InvokeClientRpcOnEveryone("StopAndDisconnect");
            }

            if (!isOfflineGame && isHost)
            {
               // lookAtCamera.GetComponentInChildren<CinemachineVirtualCamera>().LookAt =
                //    networkPlayers[currentPlayer].transform;
            }
            
            List<ulong> ids = new List<ulong>();

            foreach (var VARIABLE in NetworkingManager.Singleton.ConnectedClientsList)
            {
                if (VARIABLE.ClientId != SpawnManager.GetLocalPlayerObject().OwnerClientId)
                {
                    ids.Add(VARIABLE.ClientId);
                }
            }
                
            SpawnManager.GetLocalPlayerObject().GetComponent<NetworkPlayer>().InvokeClientRpc("ClientGetNextPlayer", ids, currentPlayer);
            
        }
        
    }
}