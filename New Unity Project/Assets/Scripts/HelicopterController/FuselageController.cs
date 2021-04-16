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

    public bool showGUI;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = transform.root.GetComponentInChildren<GameManager>();
        
        _networkPlayer = transform.root.GetComponent<NetworkPlayer>();

        gameManager = transform.root.GetComponentInChildren<GameManager>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        gameManager.startingPos = transform.position;
        gameManager.startingRot = transform.rotation;

        shouldMouseRot = PlayerPrefs.GetInt("EasyMode") == 1;
    }

    public void ResetHealth()
    {
        helicopterHealth = 100f;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (gameManager.isCurrentPLayer && !gameManager.isReseting)
        {
            if (!isPlayerControlled && !gameManager.waitingForInput && other.gameObject.CompareTag("Enviroment") &&
                !gameManager.hasInvincibility)
            {
                gameManager.ResetFromPos(transform.position, gameObject, other.contacts[0].normal);
            }

            if (isPlayerControlled && other.gameObject.CompareTag("Enviroment") && !gameManager.hasInvincibility && !gameManager.waitingForInput)
            {
                Debug.Log("Collision");
                gameManager.StartCoroutine(gameManager.HelicopterDestroyed(2f));
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
                if (framesOnGround > 300)
                {
                    showGUI = true;
                    Debug.Log("No movement " + other.gameObject);
                }
            }
            else
            {
                transform.up = other.contacts[0].normal;
            }
        }
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        if (showGUI)
        {
            GUIStyle textstyle = new GUIStyle();
            textstyle.richText = true;
            GUI.Box (new Rect (Screen.width / 2 - 200,Screen.height / 2 - 50,200,50), "You appear to be stuck! Press 'R' to surrender to the spirits", textstyle);
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
        if (showGUI && !gameManager.waitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(gameManager.HelicopterDestroyed(3f));
                gameManager.waitingForInput = true;
            }
        }
        
        if (gameManager.waitingForInput && shouldMouseRot)
        {
            MouseRot();
        }
        
        if (helicopterHealth <= 0 && !gameManager.hasInvincibility)
        {
            //TODO HP homma kuntoon 
            //Debug.Log("Low hp destroyed");
            //StartCoroutine(gameManager.HelicopterDestroyed(3));
            //gameManager.hasInvincibility = true;
        }
        
        if (isPlayerControlled && gameManager.isCurrentPLayer)
        {
             _rigidbody.AddRelativeTorque(0, -_networkPlayer.yRotation * rotSpeed,0,ForceMode.Force);
        }
       
    }
}
