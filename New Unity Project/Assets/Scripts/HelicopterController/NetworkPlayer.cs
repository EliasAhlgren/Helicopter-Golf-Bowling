using System.Collections;
using Cinemachine;
using GameManagement;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HelicopterController
{
    public class NetworkPlayer : NetworkedBehaviour
    {
     
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

        
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new  WaitForSeconds(2f);
            SpawnHelicopter(GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().spawnPosition);
            SetCamera(Vector3.zero);
        }

        private void OnGUI()
        {
            if (_hasConnected)
            {
                GUI.Box (new Rect (Screen.width - 100,0,100,50), SpawnManager.GetLocalPlayerObject().OwnerClientId.ToString());
            
            }
        }

        [ServerRPC(RequireOwnership = false)]
        public void HostNextPlayer()
        {
            Debug.Log("Host got the message");
            GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().NextPlayerTurn();
        }
        
        [ClientRPC]
        public void Testi()
        {
            Debug.Log("TESTI TESTI TESTI");
        }
        
        [ClientRPC]
        public void SpawnHelicopter(Vector3 pos)
        {
                Vector3 randVector = new Vector3(pos.x + Random.Range(5f, -5f), pos.y, pos.z + Random.Range(5f, -5f));
                physicalObject.SetActive(true);
                physicalObject.transform.position = randVector;
                gameObject.GetComponentInChildren<GameManager>().mainFuselage =
                    physicalObject.GetComponentInChildren<FuselageController>().gameObject;
                gameObject.GetComponentInChildren<GameManager>().mainRotor =
                    physicalObject.GetComponentInChildren<RotorController>().gameObject;

                gameObject.GetComponentInChildren<CinemachineFreeLook>().Follow =
                    gameObject.GetComponentInChildren<CinemachineFreeLook>().LookAt =
                        physicalObject.GetComponentInChildren<FuselageController>().transform;
               Debug.Log("Tämän viestin lähetti " +NetworkingManager.Singleton.LocalClientId , gameObject);
        }
    
        [ClientRPC]
        public void SetCamera(Vector3 pos)
        {
            _hasConnected = true;
        
            Debug.Log("Hello");
            SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<Camera>().enabled = true;
            SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
            foreach (var variable in SpawnManager.SpawnedObjectsList)
            {
                Debug.Log(variable + " ID " + variable.OwnerClientId);
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
            Debug.Log("new score: " + newScore);
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
        public void SetCurrentPlayerCamera(NetworkedObject jyrki, bool EasyMode)
        {
            /*
        foreach (var VARIABLE in SpawnManager.SpawnedObjects)
        {
            Debug.Log("Object: " + VARIABLE);
        }
        */
        
        
            Debug.Log("Setting gameras to  " + jyrki);
        
            foreach (var variable in SpawnManager.SpawnedObjectsList)
            {
                if (variable != jyrki)
                {
                    variable.GetComponentInChildren<GameManager>().isCurrentPLayer = false;
                    Debug.Log("Not found "+ variable);
                    variable.GetComponentInChildren<Camera>().enabled = false;
                    variable.GetComponentInChildren<CinemachineFreeLook>().enabled = false;
                
                }
                else
                {
                    variable.GetComponentInChildren<GameManager>().isCurrentPLayer = true;
                    variable.GetComponentInChildren<GameManager>().currentUIstate = UIstate.Awaiting;
                    Debug.Log("found "+ variable);
                    variable.GetComponentInChildren<Camera>().enabled = true;
                    variable.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
                    return;
                }
            }

            gameObject.GetComponentInChildren<FuselageController>().shouldMouseRot = EasyMode;
            if (EasyMode)
            {
                Debug.Log("Client has easymode");
                PlayerPrefs.SetInt("EasyMode", 1);
            }

        }

        IEnumerator WaitBeforeDc()
        {
            yield return new  WaitForSeconds(IsHost ? 3 : 2);
            Debug.Log("Disconnecting from server");
            MultiplayerManager mp = GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>();
            mp.isAtStartup = true;
            mp.StopClient();
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
            if (IsHost)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log("Disconnecting");
                    InvokeClientRpcOnEveryone(StopAndDisconnect);
                }
            }
        
            yMovement = Input.GetAxisRaw(yMoveControl);
            xRotation = Input.GetAxisRaw(xRotationControl);
            yRotation = Input.GetAxisRaw(yRotationControl);
            zRotation = Input.GetAxisRaw(zRotationControl);
        }
    }
}
