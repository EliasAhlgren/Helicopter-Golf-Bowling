using System;
using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = transform.root.GetComponentInChildren<GameManager>();
        
        _networkPlayer = transform.root.GetComponent<NetworkPlayer>();

        gameManager = transform.root.GetComponentInChildren<GameManager>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        gameManager.startingPos = transform.position;
        gameManager.startingRot = transform.rotation;

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
                gameManager.ResetFromPos(transform.position, gameObject);
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
        if (gameManager.waitingForInput)
        {
            //MouseRot();
        }
        
        if (helicopterHealth <= 0 && !gameManager.hasInvincibility)
        {
            Debug.Log("Low hp destroyed");
            StartCoroutine(gameManager.HelicopterDestroyed(3));
        }
        
        if (isPlayerControlled && gameManager.isCurrentPLayer)
        {
             _rigidbody.AddRelativeTorque(0, -_networkPlayer.yRotation * rotSpeed,0,ForceMode.Force);
        }
       
    }
}
