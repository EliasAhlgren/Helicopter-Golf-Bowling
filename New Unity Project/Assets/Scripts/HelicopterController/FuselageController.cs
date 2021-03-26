using System;
using System.Collections;
using System.Collections.Generic;
using GameManagement;
using MLAPI.Spawning;
using UnityEngine;
using NetworkPlayer = HelicopterController.NetworkPlayer;

public class FuselageController : MonoBehaviour
{
    public bool isPlayerControlled = true;
    
    private Rigidbody _rigidbody;

    public float rotSpeed = 1;
    
    GameManager gameManager;

    public float helicopterHealth = 100f;

    public float collisionDamageMultiplier = 2;

    public float framesOnGround;

    private NetworkPlayer _networkPlayer;

    public bool shouldMouseRot;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = transform.root.GetComponentInChildren<GameManager>();
        
        _networkPlayer = transform.root.GetComponent<NetworkPlayer>();

        gameManager = transform.root.GetComponentInChildren<GameManager>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        gameManager.startingPos = transform.position;
        gameManager.startingRot = transform.rotation;

        Debug.Log("startupissa easy mode on: " + shouldMouseRot);
        shouldMouseRot = PlayerPrefs.GetInt("EasyMode") == 1;
    }

    public void ResetHealth()
    {
        helicopterHealth = 100f;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (gameManager.isCurrentPLayer)
        {
            if (!isPlayerControlled && !gameManager.waitingForInput && other.gameObject.CompareTag("Enviroment") &&
                !gameManager.hasInvincibility)
            {
                gameManager.ResetFromPos(transform.position, gameObject, other.contacts[0].normal);
            }
            else if (gameManager.hasInvincibility)
            {
                helicopterHealth -= other.relativeVelocity.magnitude * collisionDamageMultiplier;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        framesOnGround = 0;
    }

    private void OnCollisionStay(Collision other)
    {
        if (gameManager.isCurrentPLayer)
        {
            if (!gameManager.waitingForInput)
            {
                framesOnGround++;
                if (framesOnGround > 200)
                {
                    Debug.Log("No movement");
                    StartCoroutine(gameManager.HelicopterDestroyed(3f));
                    gameManager.waitingForInput = true;
                }
            }
        }
    }

    
    public void Relase()
    {
        isPlayerControlled = !isPlayerControlled;
    }

    void MouseRot()
    {
        Vector3 mouseFaceRot = new Vector3(0, Camera.main.transform.eulerAngles.y - 90f,0f);
                    transform.eulerAngles = mouseFaceRot;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!transform.root.GetComponent<MultiplayerManager>().isHost)
        {
            shouldMouseRot = SpawnManager.SpawnedObjectsList[0].GetComponentInChildren<FuselageController>()
                .shouldMouseRot;
        }
        
        if (gameManager.waitingForInput && shouldMouseRot)
        {
            MouseRot();
        }
        
        if (helicopterHealth <= 0 && !gameManager.hasInvincibility)
        {
            Debug.Log("Low hp destroyed");
            StartCoroutine(gameManager.HelicopterDestroyed(3));
            gameManager.hasInvincibility = true;
        }
        
        if (isPlayerControlled && gameManager.isCurrentPLayer)
        {
             _rigidbody.AddRelativeTorque(0, -_networkPlayer.yRotation * rotSpeed,0,ForceMode.Force);
        }
       
    }
}
