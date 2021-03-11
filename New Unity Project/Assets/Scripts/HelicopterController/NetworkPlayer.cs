using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MLAPI;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.Spawning;


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
    public void SetCamera()
    {
            Debug.Log("Hello");
            SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<Camera>().enabled = true;
            SpawnManager.GetLocalPlayerObject().gameObject.GetComponentInChildren<CinemachineVirtualCamera>().enabled = true;
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
