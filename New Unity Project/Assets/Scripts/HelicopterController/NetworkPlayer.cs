using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MLAPI;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.Spawning;
using TMPro;

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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ClientRPC]
    public void SetCamera(Vector3 pos)
    {
        transform.position = pos;
        Debug.Log("Hello");
        SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<Camera>().enabled = true;
        SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
    }

    [ClientRPC]
    void ClientGetNextPlayer(int crntPlayer)
    {

        
        
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

    IEnumerator textHomma()
    {
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
                
                yield return new  WaitForSeconds(2);
                
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
                Debug.Log("Not found "+ VARIABLE);
                VARIABLE.GetComponentInChildren<Camera>().enabled = false;
                VARIABLE.GetComponentInChildren<CinemachineFreeLook>().enabled = false;
                
            }
            else
            {
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
        yMovement = Input.GetAxisRaw(yMoveControl);
        xRotation = Input.GetAxisRaw(xRotationControl);
        yRotation = Input.GetAxisRaw(yRotationControl);
        zRotation = Input.GetAxisRaw(zRotationControl);
    }
}
