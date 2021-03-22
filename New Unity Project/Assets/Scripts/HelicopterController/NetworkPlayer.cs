using System.Collections;
using Cinemachine;
using GameManagement;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using TMPro;
using UnityEngine;
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
        void Start()
        {
        
        }

        private void OnGUI()
        {
            if (_hasConnected)
            {
                GUI.Box (new Rect (Screen.width - 100,0,100,50), SpawnManager.GetLocalPlayerObject().OwnerClientId.ToString());
            
            }
        }

        [ClientRPC]
        public void Testi()
        {
            Debug.Log("TESTI TESTI TESTI");
        }
        
        [ClientRPC]
        public void SpawnHelicopter(Vector3 pos, ulong target)
        {
            
            
            if (NetworkingManager.Singleton.LocalClientId == target)
            {
                
                Vector3 randVector = new Vector3(pos.x + Random.Range(5f, -5f), pos.y, pos.z + Random.Range(5f, -5f));
                GameObject gb = Instantiate(physicalObject, randVector, Quaternion.identity, transform);
                gameObject.GetComponentInChildren<GameManager>().mainFuselage =
                    gb.GetComponentInChildren<FuselageController>().gameObject;
                gameObject.GetComponentInChildren<GameManager>().mainRotor =
                    gb.GetComponentInChildren<RotorController>().gameObject;

                gameObject.GetComponentInChildren<CinemachineFreeLook>().Follow =
                    gameObject.GetComponentInChildren<CinemachineFreeLook>().LookAt =
                        gb.GetComponentInChildren<FuselageController>().transform;
               Debug.Log("Tämän viestin lähetti " +NetworkingManager.Singleton.LocalClientId , gameObject);
                Debug.Log("Helicopter Spawned for "+ target, gb);
            }
           
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

            Debug.Log("yea");
            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>().text = "Strike";
            }

            if (GameObject.Find("TopText"))
            {
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().enabled = false;
            }

            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = false;
            }

            yield return new WaitForSeconds(2);

            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>().text = "Vehicle Destroyed";
                GameObject.Find("MiddleText").SetActive(false);
            }

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
                    Debug.Log("found "+ variable);
                    variable.GetComponentInChildren<Camera>().enabled = true;
                    variable.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
                    return;
                }
            }
        
        }

        IEnumerator WaitBeforeDc()
        {
            yield return new  WaitForSeconds(IsHost ? 3 : 2);
            MultiplayerManager mp = GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>();
            mp.isAtStartup = true;
            mp.StopClient();
        }

        [ClientRPC]
        void SetSpectatorUI()
        {
            Debug.Log("SpectatorUI");
            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().text = "";
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().text = "Spectating";
            }
        }
    
        [ClientRPC]
        void ResetUI()
        {
            Debug.Log("PlayerUI");
            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().text = "Press any key";
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().text = "Player In control";

            }
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
