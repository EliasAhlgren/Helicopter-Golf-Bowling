using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MLAPI;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using MLAPI.Spawning;
using TMPro;
using UnityEngine.SceneManagement;

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

    private bool hasConnected;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        if (hasConnected)
        {
            GUI.Box (new Rect (Screen.width - 100,0,100,50), SpawnManager.GetLocalPlayerObject().OwnerClientId.ToString());
            
        }
    }

   
    
    [ClientRPC]
    public void SetCamera(Vector3 pos)
    {
        hasConnected = true;
        transform.position = pos;
        Debug.Log("Hello");
        SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<Camera>().enabled = true;
        SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        foreach (var VARIABLE in SpawnManager.SpawnedObjectsList)
        {
            Debug.Log(VARIABLE + " ID " + VARIABLE.OwnerClientId);
        }
    }

    [ClientRPC]
    void SetCineCamera()
    {
        
    }
    
    [ClientRPC]
    void ClientGetNextPlayer(int crntPlayer, int newScore)
    {
            StartCoroutine(textHomma(crntPlayer, newScore));
        
            ScoreManager _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            for (int i = 0; i < _scoreManager.scoresUguis.Length; i++)
            {
                if (i == crntPlayer)
                {
                    _scoreManager.scoresUguis[i].color = Color.red;
                }
                else
                {
                    _scoreManager.scoresUguis[i].color = Color.white;
                }
            }
        
    }

    public IEnumerator textHomma(int crntPlayer, int newScore)
    {
        Debug.Log("new score: " + newScore);
        ScoreManager _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        for (int i = 0; i < _scoreManager.scoresUguis.Length; i++)
        {
            if (i == crntPlayer)
            {
                _scoreManager.scoresUguis[i].color = Color.red;
                _scoreManager.scoresUguis[i].text = newScore.ToString();
            }
            else
            {
                _scoreManager.scoresUguis[i].color = Color.white;
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

        foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
        {
            VARIABLE.GetComponent<TargetScript>().ResetTransform();
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
        
        foreach (var VARIABLE in SpawnManager.SpawnedObjectsList)
        {
            if (VARIABLE != jyrki)
            {
                VARIABLE.GetComponentInChildren<GameManager>().isCurrentPLayer = false;
                Debug.Log("Not found "+ VARIABLE);
                VARIABLE.GetComponentInChildren<Camera>().enabled = false;
                VARIABLE.GetComponentInChildren<CinemachineFreeLook>().enabled = false;
                
            }
            else
            {
                VARIABLE.GetComponentInChildren<GameManager>().isCurrentPLayer = true;
                Debug.Log("found "+ VARIABLE);
                VARIABLE.GetComponentInChildren<Camera>().enabled = true;
                VARIABLE.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
                return;
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsHost)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Disconnecting");
                if (NetworkingManager.Singleton.ConnectedClientsList.Count > 1)
                {
                    for (var index = 0; index < NetworkingManager.Singleton.ConnectedClientsList.Count; index++)
                    {
                        var VARIABLE = NetworkingManager.Singleton.ConnectedClientsList[index];
                        NetworkingManager.Singleton.DisconnectClient(VARIABLE.ClientId);
                    }

                    NetworkingManager.Singleton.StopHost();
                }
                else
                {
                    NetworkingManager.Singleton.StopHost();
                }
                
                
            }
        }
        
        yMovement = Input.GetAxisRaw(yMoveControl);
        xRotation = Input.GetAxisRaw(xRotationControl);
        yRotation = Input.GetAxisRaw(yRotationControl);
        zRotation = Input.GetAxisRaw(zRotationControl);
    }
}
