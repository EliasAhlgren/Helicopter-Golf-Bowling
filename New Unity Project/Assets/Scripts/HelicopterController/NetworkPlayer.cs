using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using GameManagement;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkedVar;
using MLAPI.Prototyping;
using MLAPI.Spawning;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HelicopterController
{
    public class NetworkPlayer : NetworkedBehaviour
    {
        private MultiplayerManager _multiplayerManager;
        
        public float yMovement;
        public float xRotation;
        public float yRotation;
        public float zRotation;
        [SerializeField] private string yMoveControl;
        [SerializeField] private string xRotationControl;
        [SerializeField] private string yRotationControl;
        [SerializeField] private string zRotationControl;

        private bool _hasConnected;

        public GameObject physicalObject;

        public StudioEventEmitter emitter;

        public NetworkedVar<float> myScore;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            Destroy(GameObject.Find("AttemptConnectionCanvas"));
            
            _multiplayerManager = GameObject.FindWithTag("ScoreManager").GetComponent<MultiplayerManager>();
            
            yield return new  WaitForSeconds(2f);
            
            SetCamera(GameObject.FindWithTag("StartingPoint").transform.position);
            
            SpawnHelicopter(_multiplayerManager.spawnPosition);
            
            if (_multiplayerManager.isOfflineGame)
            {
                Debug.Log("Is offline");
                foreach (var VARIABLE in FindObjectsOfType<NetworkedTransform>())
                {
                    Debug.Log(VARIABLE + " Found, disabling net transform");
                    VARIABLE.enabled = false;
                }
            }
            else
            {
                 string m_name = PlayerPrefs.GetString("PlayerName");
                 InvokeServerRpc(SendName, m_name);
            }

           

        }

        private void OnGUI()
        {
            if (_hasConnected && !_multiplayerManager.isOfflineGame)
            {
                GUI.Box (new Rect (Screen.width - 100,0,100,50), SpawnManager.GetLocalPlayerObject().OwnerClientId.ToString());
            }
        }

        [ServerRPC(RequireOwnership = false)]
        public void HostNextPlayer()
        {
            Debug.Log("Host got the message, Next Player turn");
            _multiplayerManager.NextPlayerTurn();
        }

         [ServerRPC(RequireOwnership = false)]
         public void SendName(string _name)
         {
             Debug.Log("Sending a clients name to server " + _name);
             _multiplayerManager.SetName(_name, gameObject);
         }
        
        [ClientRPC]
        public void GetNames(string newName1, int index)
        {
            Debug.Log("Getting name "+ index + " name is " + newName1);
           
                ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                scoreManager.scoresUguis[index].text = newName1;
            
        }
        
       
        
        [ClientRPC]
        public void SpawnHelicopter(Vector3 pos)
        {
                physicalObject.SetActive(true);
                //physicalObject.transform.position = pos;
                gameObject.GetComponentInChildren<GameManager>().mainFuselage =
                    physicalObject.GetComponentInChildren<FuselageController>().gameObject;
                gameObject.GetComponentInChildren<GameManager>().mainRotor =
                    physicalObject.GetComponentInChildren<RotorController>().gameObject;

                gameObject.GetComponentInChildren<CinemachineFreeLook>().Follow =
                    gameObject.GetComponentInChildren<CinemachineFreeLook>().LookAt =
                        physicalObject.GetComponentInChildren<FuselageController>().transform;
                if (_multiplayerManager.isOfflineGame)
                {
                    SetCurrentPlayerCamera(null);
                }
                Debug.Log("Helicopter spawned");
                
               
        }
    
        [ClientRPC]
        public void SetCamera(Vector3 pos)
        {
            _hasConnected = true;

            if (!_multiplayerManager.isOfflineGame)
            {
                SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<Camera>().enabled = true;
                SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled =
                    true;
            }
            else
            {
                gameObject.GetComponentInChildren<Camera>().enabled = true;
                gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
            }
            
            
        }

        [ClientRPC]
        void SetCineCamera()
        {
            
        }
    
        [ClientRPC]
        void ClientGetNextPlayer(int crntPlayer, int newScore)
        {
            StartCoroutine(TextHomma(crntPlayer, newScore));
        
            ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            for (int i = 0; i < scoreManager.scoresUguis.Length; i++)
            {
                if (i == crntPlayer)
                {
                    scoreManager.scoresUguis[i].color = Color.red;
                }
                else
                {
                    scoreManager.scoresUguis[i].color = Color.white;
                }
            }
        
        }

        public IEnumerator TextHomma(int crntPlayer, int newScore)
        {
            ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            for (int i = 0; i < scoreManager.scoresUguis.Length; i++)
            {
                if (i == crntPlayer)
                {
                    scoreManager.scoresUguis[i].color = Color.red;
                    scoreManager.scoresUguis[i].text = newScore.ToString();
                }
                else
                {
                    scoreManager.scoresUguis[i].color = Color.white;
                }
            }
            
            yield return new WaitForSeconds(2);

            foreach (var variable in GameObject.FindGameObjectsWithTag("Pin"))
            {
                variable.GetComponent<TargetScript>().ResetTransform();
            }
                
        }

        [ClientRPC]
        public void GetCurrentScore()
        {
        
        }

   
    
        [ClientRPC]
        public void SetCurrentPlayerCamera(NetworkedObject jyrki)
        {

            if (_multiplayerManager.isOfflineGame)
            {
                Debug.Log("Offline Camera Set");
                gameObject.GetComponentInChildren<GameManager>().isCurrentPLayer = true;
                gameObject.GetComponentInChildren<GameManager>().currentUIstate = UIstate.Awaiting;
                gameObject.GetComponentInChildren<Camera>().enabled = true;
                gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
            }
            else
            {
                Debug.Log("Setting gameras to  " + jyrki);

                foreach (var variable in SpawnManager.SpawnedObjectsList)
                {
                    if (variable != jyrki)
                    {
                        variable.GetComponentInChildren<GameManager>().isCurrentPLayer = false;
                        Debug.Log("Not found " + variable);
                        variable.GetComponentInChildren<Camera>().enabled = false;
                        variable.GetComponentInChildren<CinemachineFreeLook>().enabled = false;
                        variable.GetComponentInChildren<StudioListener>().enabled = false;
                    }
                    else
                    {

                        variable.gameObject.GetComponentInChildren<GameManager>().isCurrentPLayer = true;
                        variable.gameObject.GetComponentInChildren<GameManager>().currentUIstate = UIstate.Awaiting;
                        Debug.Log("found " + variable);
                        variable.gameObject.GetComponentInChildren<Camera>().enabled = true;
                        variable.gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
                        variable.GetComponentInChildren<StudioListener>().enabled = true;
                        return;
                    }
                }
            }
        }

        [ClientRPC]
        void ScoreManagerGetScore()
        {
            Debug.Log("Online score RPC v.69");
            _multiplayerManager.GetComponent<ScoreManager>().FindScores();
        }
        
        [ClientRPC]
        void SetEasyMode()
        {
            Debug.Log("Clientti sai easyModen", gameObject);
            PlayerPrefs.SetInt("EasyMode", 1);
            gameObject.GetComponentInChildren<FuselageController>().shouldMouseRot = true;
        }
        
        IEnumerator WaitBeforeDc()
        {
            Debug.Log("Disconnecting from server, wait started");
            yield return new  WaitForSeconds(IsHost ? 3 : 2);
            MultiplayerManager mp = _multiplayerManager;
            mp.isAtStartup = true;
            mp.StopClient();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }

        
        [ClientRPC]
        public void StopAndDisconnect()
        {
            StartCoroutine(WaitBeforeDc());
        }
    
        // Update is called once per frame
        void FixedUpdate()
        {
            
            yMovement = Input.GetAxisRaw(yMoveControl);
            xRotation = Input.GetAxisRaw(xRotationControl);
            yRotation = Input.GetAxisRaw(yRotationControl);
            zRotation = Input.GetAxisRaw(zRotationControl);

            emitter.Params[0].Value = physicalObject.GetComponentInChildren<RotorController>().yVelocity / 100;

        }
    }
}
